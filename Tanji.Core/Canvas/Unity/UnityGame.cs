namespace Tanji.Core.Canvas.Unity;

// TODO: Push Wazzy to NuGet, then reference the package to enable Unity disassembly support.
//public sealed class UnityGame : HGame
//{
//    private WASMModule? _wasm;

//    public override bool IsPostShuffle => true;
//    public override int MinimumConnectionAttempts => 1;
//    public override HPlatform Platform => HPlatform.Unity;

//    public override IHFormat SendPacketFormat => IHFormat.EvaWireUnity;
//    public override IHFormat ReceivePacketFormat => IHFormat.EvaWireUnity;

//    public UnityGame(ReadOnlyMemory<byte> region, string path, string revision)
//        : base(HPatches.InjectKeyShouter)
//    {
//        if (region.IsEmpty)
//        {
//            ThrowHelper.ThrowArgumentException("The WebAssembly module can not be parsed with the specified buffer because it is empty.", nameof(region));
//        }

//        _wasm = new WASMModule(region);

//        Path = path;
//        Revision = revision;
//    }

//    #region Patching Methods
//    protected override bool? TryPatch(HPatches patch) => patch switch
//    {
//        HPatches.InjectKeyShouter => InjectKeyShouter(),
//        _ => null
//    };

//    private bool InjectKeyShouter()
//    {
//        if (IsDisposed || _wasm == null)
//        {
//            ThrowHelper.ThrowObjectDisposedException("The underlying WebAssembly module has already been disposed.");
//        }

//        for (int i = 0; i < _wasm.CodeSec.Count; i++)
//        {
//            // Begin searching for the ChaChaEngine.SetKey method.
//            var funcTypeIndex = (int)_wasm.FunctionSec[i];
//            FuncType functionType = _wasm.TypeSec[funcTypeIndex];
//            CodeSubsection codeSubSec = _wasm.CodeSec[i];

//            if (codeSubSec.Locals.Count != 1) continue;
//            if (functionType.ParameterTypes.Count != 4) continue;

//            bool hasValidParamTypes = true;
//            for (int j = 0; j < functionType.ParameterTypes.Count; j++)
//            {
//                if (functionType.ParameterTypes[j] == typeof(int)) continue;
//                hasValidParamTypes = false;
//                break;
//            }
//            if (!hasValidParamTypes) continue; // If all of the parameters are not of type int.

//            List<WASMInstruction> expression = codeSubSec.Parse();
//            if (expression[0].OP != OPCode.ConstantI32) continue;
//            if (expression[1].OP != OPCode.LoadI32_8S) continue;
//            if (expression[2].OP != OPCode.EqualZeroI32) continue;
//            if (expression[3].OP != OPCode.If) continue;

//            // Dig through the block/branching expressions
//            WASMInstruction[] expandedInstructions = null;
//            try
//            {
//                expandedInstructions = WASMInstruction.ConcatNestedExpressions(expression).ToArray();
//            }
//            catch { continue; }

//            for (int j = 0, k = expandedInstructions.Length - 2; j < expandedInstructions.Length; j++)
//            {
//                WASMInstruction instruction = expandedInstructions[j];
//                if (instruction.OP != OPCode.ConstantI32) continue;

//                var constanti32Ins = (ConstantI32Ins)instruction;
//                if (constanti32Ins.Constant != 12) continue;

//                if (expandedInstructions[++j].OP != OPCode.AddI32) continue;
//                if (expandedInstructions[++j].OP != OPCode.TeeLocal) continue;
//                if (expandedInstructions[++j].OP != OPCode.LoadI32) continue;
//                if (expandedInstructions[++j].OP != OPCode.ConstantI32) continue;
//                if (expandedInstructions[++j].OP != OPCode.SubtractI32) continue;

//                if (expandedInstructions[k--].OP != OPCode.Call) continue;
//                if (expandedInstructions[k--].OP != OPCode.ConstantI32) continue;
//                if (expandedInstructions[k--].OP != OPCode.ConstantI32) continue;
//                if (expandedInstructions[k--].OP != OPCode.ConstantI32) continue;

//                expression.InsertRange(0, new WASMInstruction[]
//                {
//                    new ConstantI32Ins(0),  // WebSocket Instance Id
//                    new GetLocalIns(1),     // Key Pointer
//                    new ConstantI32Ins(48), // Key Length
//                    new CallIns(126),       // _WebSocketSend
//                    new DropIns(),
//                });
//                codeSubSec.Bytecode = WASMInstruction.ToArray(expression);
//                return true;
//            }
//        }
//        return false;
//    }
//    #endregion

//    public override byte[] ToArray()
//    {
//        if (IsDisposed || _wasm == null)
//        {
//            ThrowHelper.ThrowObjectDisposedException("The underlying WebAssembly module has already been disposed.");
//        }
//        return _wasm.ToArray();
//    }
//    public override void Disassemble()
//    {
//        if (IsDisposed || _wasm == null)
//        {
//            ThrowHelper.ThrowObjectDisposedException("The underlying WebAssembly module has already been disposed.");
//        }
//        _wasm.Disassemble();
//    }

//    protected override void Dispose(bool disposing) => _wasm = null;
//}