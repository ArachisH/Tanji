using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Sulakore.Habbo;

using Wazzy;
using Wazzy.Types;
using Wazzy.Bytecode;
using Wazzy.Sections.Subsections;
using Wazzy.Bytecode.Instructions.Control;
using Wazzy.Bytecode.Instructions.Numeric;
using Wazzy.Bytecode.Instructions.Variable;
using Wazzy.Bytecode.Instructions.Parametric;

namespace Tanji.Habbo
{
    public class UnityGame : TGame
    {
        private WASMModule _wasm;
        private readonly Dictionary<string, MessageInfo> _outMessages, _inMessages;

        public override bool IsUnity => true;
        public override bool IsPostShuffle => true;
        public override bool HasPingInstructions => false;

        public UnityGame(ReadOnlyMemory<byte> gameBytes, string path, string revision)
        {
            _inMessages = new Dictionary<string, MessageInfo>();
            _outMessages = new Dictionary<string, MessageInfo>();
            if (!gameBytes.IsEmpty)
            {
                _wasm = new WASMModule(gameBytes);
            }

            Path = path;
            Revision = revision;

            // Not passing 'this' will initialize the messages by their default unity id values.
            In = new Incoming();
            Out = new Outgoing();
        }

        public void Patch()
        {
            InjectKeyShouter();
        }
        public void LoadMessagesInformation(string messagesInfoPath)
        {
            bool isOutgoing = true;
            foreach (string line in File.ReadAllLines(messagesInfoPath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] items = line.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (items.Length != 2)
                {
                    switch (items[0].ToLower())
                    {
                        case "[outgoing]": isOutgoing = true; break;
                        case "[incoming]": isOutgoing = false; break;
                    }
                    continue;
                }

                string[] subItems = items[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (subItems.Length != 2) continue; // Must have both hash, and id.

                string name = items[0];
                string hash = subItems[1];
                short id = short.Parse(subItems[0]);

                Dictionary<string, MessageInfo> messages = (isOutgoing ? _outMessages : _inMessages);
                messages.Add(name, new MessageInfo(id, hash, null, null, null, true));
            }
        }

        public byte[] ToArray() => _wasm.ToArray();
        public void Disassemble() => _wasm.Disassemble();

        private void InjectKeyShouter()
        {
            for (int i = 0; i < _wasm.CodeSec.Count; i++)
            {
                // Begin searching for the ChaChaEngine.SetKey method.
                var funcTypeIndex = (int)_wasm.FunctionSec[i];
                FuncType functionType = _wasm.TypeSec[funcTypeIndex];
                CodeSubsection codeSubSec = _wasm.CodeSec[i];

                if (codeSubSec.Locals.Count != 1) continue;
                if (functionType.ParameterTypes.Count != 4) continue;

                bool hasValidParamTypes = true;
                for (int j = 0; j < functionType.ParameterTypes.Count; j++)
                {
                    if (functionType.ParameterTypes[j] == typeof(int)) continue;
                    hasValidParamTypes = false;
                    break;
                }
                if (!hasValidParamTypes) continue; // If all of the parameters are not of type int.

                List<WASMInstruction> expression = codeSubSec.Parse();
                if (expression[0].OP != OPCode.ConstantI32) continue;
                if (expression[1].OP != OPCode.LoadI32_8S) continue;
                if (expression[2].OP != OPCode.EqualZeroI32) continue;
                if (expression[3].OP != OPCode.If) continue;

                // Dig through the block/branching expressions
                var expandedInstructions = WASMInstruction.ConcatNestedExpressions(expression).ToArray();
                for (int j = 0, k = expandedInstructions.Length - 2; j < expandedInstructions.Length; j++)
                {
                    WASMInstruction instruction = expandedInstructions[j];
                    if (instruction.OP != OPCode.ConstantI32) continue;

                    var constanti32Ins = (ConstantI32Ins)instruction;
                    if (constanti32Ins.Constant != 12) continue;

                    if (expandedInstructions[++j].OP != OPCode.AddI32) continue;
                    if (expandedInstructions[++j].OP != OPCode.TeeLocal) continue;
                    if (expandedInstructions[++j].OP != OPCode.LoadI32) continue;
                    if (expandedInstructions[++j].OP != OPCode.ConstantI32) continue;
                    if (expandedInstructions[++j].OP != OPCode.SubtractI32) continue;

                    if (expandedInstructions[k--].OP != OPCode.Call) continue;
                    if (expandedInstructions[k--].OP != OPCode.ConstantI32) continue;
                    if (expandedInstructions[k--].OP != OPCode.ConstantI32) continue;
                    if (expandedInstructions[k--].OP != OPCode.ConstantI32) continue;

                    expression.InsertRange(0, new WASMInstruction[]
                    {
                        new ConstantI32Ins(0),  // WebSocket Instance Id
                        new GetLocalIns(1),     // Key Pointer
                        new ConstantI32Ins(48), // Key Length
                        new CallIns(126),       // _WebSocketSend
                        new DropIns(),
                    });
                    codeSubSec.Bytecode = WASMInstruction.ToArray(expression);
                    return;
                }
            }
        }

        public override MessageInfo GetInformation(HMessage message)
        {
            if (message == null) return null;
            Dictionary<string, MessageInfo> messages = message.IsOutgoing ? _outMessages : _inMessages;
            messages.TryGetValue(message.Name, out MessageInfo msgInfo);
            return msgInfo;
        }
        public override short Resolve(string name, bool isOutgoing) => throw new NotImplementedException();

        protected override void Dispose(bool disposing)
        {
            _wasm = null;
        }
    }
}