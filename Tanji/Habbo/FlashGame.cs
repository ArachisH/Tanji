using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

using Sulakore.Habbo;

using Flazzy;
using Flazzy.IO;
using Flazzy.ABC;
using Flazzy.Tags;
using Flazzy.ABC.AVM2;
using Flazzy.ABC.AVM2.Instructions;

namespace Tanji.Habbo
{
    public class FlashGame : TGame
    {
        private ShockwaveFlash _flash;
        private List<ABCFile> _abcFiles;
        private Dictionary<DoABCTag, ABCFile> _abcFileTags;
        private Dictionary<ASClass, FlashMessage> _flashMsgs;
        private Dictionary<string, MessageInfo> _outMessages, _inMessages;

        private ASMethod _habboCommManagerConnect;
        private ASInstance _habboCommDemo, _habboCommManager;

        #region AS3 Reserved Names
        private static readonly string[] _reservedNames = new[]
        {
            "break", "case", "catch", "class", "continue",
            "default", "do", "dynamic", "each", "else",
            "extends", "false", "final", "finally", "for",
            "function", "get", "if", "implements", "import",
            "in", "include", "native", "null", "override",
            "package", "return", "set", "static", "super",
            "switch", "throw", "true", "try", "use",
            "var", "while", "with"
        };
        #endregion

        public FlashGame(string path)
            : this(new ShockwaveFlash(path), path)
        { }
        public FlashGame(string path, byte[] gameBytes)
            : this(new ShockwaveFlash(gameBytes), path)
        { }
        protected FlashGame(ShockwaveFlash flash, string path)
        {
            _flash = flash;
            _abcFiles = new List<ABCFile>();
            _abcFileTags = new Dictionary<DoABCTag, ABCFile>();
            _inMessages = new Dictionary<string, MessageInfo>();
            _outMessages = new Dictionary<string, MessageInfo>();
            _flashMsgs = new Dictionary<ASClass, FlashMessage>();

            Path = path;
            IsUnity = false;
            IsPostShuffle = true;
        }

        #region Message Hashing/Linking
        public void GenerateMessageHashes(string hashesPath)
        {
            bool isUnmatched = false;
            var names = new Dictionary<string, (string, bool)>();
            foreach (string line in File.ReadAllLines(hashesPath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] items = line.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (items.Length != 2)
                {
                    switch (items[0].ToLower())
                    {
                        case "[outgoing]": isUnmatched = false; break;
                        case "[incoming]": isUnmatched = false; break;
                        case "[unmatched]": isUnmatched = true; break;
                    }
                    continue;
                }

                string name = items[0];
                if (!isUnmatched)
                {
                    items = items[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    if (items.Length != 2) continue;
                }
                string hash = items[1];
                names.Add(hash, (name, isUnmatched)); // Duplicate hashes should no longer exist, so allow the exception to be thrown.
            }

            FindMessageReferences();
            var collisions = new Dictionary<string, int>();
            var messages = new Dictionary<string, FlashMessage>();
            foreach (FlashMessage flashMsg in _flashMsgs.Values)
            {
                string hash = GenerateHash(flashMsg);
                if (messages.ContainsKey(hash))
                {
                    if (!collisions.ContainsKey(hash))
                    {
                        collisions.Add(hash, 0);
                    }
                    collisions[hash]++;
                    hash = GenerateHash(flashMsg, collisions[hash]);
                }
                messages.Add(hash, flashMsg);
            }

            foreach ((string hash, (string name, bool unmatched)) in names)
            {
                if (!messages.TryGetValue(hash, out FlashMessage flashMsg)) continue;

                (flashMsg.IsOutgoing ? _outMessages : _inMessages)
                    .Add(name, new MessageInfo(flashMsg.Id, hash, flashMsg.Structure, flashMsg.MessageClass.QName.Name, flashMsg.ParserClass?.QName.Name, !unmatched));
            }

            In = new Incoming(this);
            Out = new Outgoing(this);
        }

        private void FindMessageReferences()
        {
            int classRank = 1;
            ABCFile abc = _abcFiles.Last();
            foreach (ASClass @class in abc.Classes)
            {
                if (_flashMsgs.ContainsKey(@class)) continue;

                ASInstance instance = @class.Instance;
                if (instance.Flags.HasFlag(ClassFlags.Interface)) continue;

                IEnumerable<ASMethod> methods = (new[] { @class.Constructor, instance.Constructor })
                    .Concat(instance.GetMethods())
                    .Concat(@class.GetMethods());

                int methodRank = 0;
                foreach (ASMethod fromMethod in methods)
                {
                    bool isStatic = fromMethod.Trait?.IsStatic ?? @class.Constructor == fromMethod;
                    var fromContainer = isStatic ? (ASContainer)@class : instance;

                    List<MessageReference> refernces = FindReferences(@class, fromContainer, fromMethod);
                    if (refernces.Count > 0)
                    {
                        methodRank++;
                    }
                    foreach (MessageReference reference in refernces)
                    {
                        reference.IsStatic = isStatic;
                        reference.ClassRank = classRank;
                        reference.MethodRank = methodRank;
                        reference.GroupCount = refernces.Count;
                    }
                }
                if (methodRank > 0)
                {
                    classRank++;
                }
            }

            var froms = new Dictionary<ASContainer, List<MessageReference>>();
            foreach (FlashMessage inFlashMsg in _flashMsgs.Values.Where(fm => !fm.IsOutgoing))
            {
                foreach (MessageReference reference in inFlashMsg.References)
                {
                    if (!froms.TryGetValue(reference.FromMethod.Container, out List<MessageReference> references))
                    {
                        references = new List<MessageReference>();
                        froms.Add(reference.FromMethod.Container, references);
                    }
                    if (!references.Contains(reference))
                    {
                        references.Add(reference);
                    }
                }
            }

            classRank = 1;
            foreach (ASClass @class in abc.Classes)
            {
                ASContainer container = null;
                if (froms.TryGetValue(@class, out List<MessageReference> references))
                {
                    container = @class;
                }
                else if (froms.TryGetValue(@class.Instance, out references))
                {
                    container = @class.Instance;
                }
                if (container == null) continue;

                var methodReferenceGroups = new Dictionary<ASMethod, List<MessageReference>>();
                foreach (MessageReference reference in references)
                {
                    reference.FromClass = @class;
                    reference.InstructionRank = -1;
                    reference.ClassRank = classRank;
                    reference.IsStatic = container.IsStatic;
                    reference.GroupCount = references.Count;

                    if (!methodReferenceGroups.TryGetValue(reference.FromMethod, out List<MessageReference> methodReferences))
                    {
                        methodReferences = new List<MessageReference>();
                        methodReferenceGroups.Add(reference.FromMethod, methodReferences);
                    }
                    methodReferences.Add(reference);
                }

                int methodRank = 1;
                foreach (ASMethod method in container.GetMethods())
                {
                    if (!methodReferenceGroups.TryGetValue(method, out List<MessageReference> methodReferences)) continue;
                    foreach (MessageReference reference in methodReferences)
                    {
                        reference.MethodRank = methodRank;
                    }
                    methodRank++;
                }
                classRank++;
            }
        }
        private List<MessageReference> FindReferences(ASClass fromClass, ASContainer fromContainer, ASMethod fromMethod)
        {
            int instructionRank = 0;
            ABCFile abc = fromMethod.GetABC();

            var nameStack = new Stack<ASMultiname>();
            var references = new List<MessageReference>();

            ASContainer container = null;
            ASCode code = fromMethod.Body.ParseCode();
            for (int i = 0; i < code.Count; i++)
            {
                int extraNamePopCount = 0;
                ASInstruction instruction = code[i];
                switch (instruction.OP)
                {
                    default: continue;
                    case OPCode.NewFunction:
                    {
                        var newFunction = (NewFunctionIns)instruction;
                        references.AddRange(FindReferences(fromClass, fromContainer, newFunction.Method));
                        continue;
                    }
                    case OPCode.GetProperty:
                    {
                        var getProperty = (GetPropertyIns)instruction;
                        nameStack.Push(getProperty.PropertyName);
                        continue;
                    }
                    case OPCode.GetLex:
                    {
                        var getLex = (GetLexIns)instruction;
                        container = abc.GetClass(getLex.TypeName);
                        continue;
                    }
                    case OPCode.GetLocal_0:
                    {
                        container = fromContainer;
                        continue;
                    }
                    case OPCode.ConstructProp:
                    {
                        var constructProp = (ConstructPropIns)instruction;

                        extraNamePopCount = constructProp.ArgCount;
                        nameStack.Push(constructProp.PropertyName);
                        break;
                    }
                }

                ASMultiname messageQName = nameStack.Pop();
                if (string.IsNullOrWhiteSpace(messageQName.Name)) continue;

                ASClass messageClass = abc.GetClass(messageQName);
                if (messageClass == null) continue;

                if (!_flashMsgs.TryGetValue(messageClass, out FlashMessage flashMsg)) continue;
                if (flashMsg.References.Any(r => r.FromMethod == fromMethod)) continue;

                var reference = new MessageReference();
                flashMsg.References.Add(reference);

                if (flashMsg.IsOutgoing)
                {
                    reference.FromClass = fromClass;
                    reference.FromMethod = fromMethod;
                    reference.InstructionRank = ++instructionRank;
                    reference.IsAnonymous = !fromMethod.IsConstructor && fromMethod.Trait == null;

                    references.Add(reference);
                }
                else
                {
                    ASMultiname methodName = nameStack.Pop();
                    ASMethod callbackMethod = fromContainer.GetMethod(methodName.Name);
                    if (callbackMethod == null)
                    {
                        callbackMethod = container.GetMethod(methodName.Name);
                        if (callbackMethod == null)
                        {
                            ASMultiname slotName = nameStack.Pop();

                            ASTrait hostTrait = container.GetTraits(TraitKind.Slot)
                                .FirstOrDefault(st => st.QName == slotName);

                            container = abc.GetInstance(hostTrait.Type);
                            callbackMethod = container.GetMethod(methodName.Name);
                        }
                    }
                    reference.FromMethod = callbackMethod;
                }
            }
            return references;
        }
        #endregion

        public override short Resolve(string name, bool isOutgoing)
        {
            Dictionary<string, MessageInfo> messages = isOutgoing ? _outMessages : _inMessages;
            if (messages.TryGetValue(name, out MessageInfo information))
            {
                return information.Id;
            }
            else return -1;
        }
        public override MessageInfo GetInformation(HMessage message)
        {
            if (message == null) return null;
            Dictionary<string, MessageInfo> msgInfos = message.IsOutgoing ? _outMessages : _inMessages;
            if (!msgInfos.TryGetValue(message.Name, out MessageInfo msgInfo)) return null;
            return msgInfo;
        }

        public bool DisableHandshake()
        {
            if (!DisableEncryption()) return false;
            ABCFile abc = _abcFiles.Last();

            ASInstance habboCommDemoInstance = GetHabboCommunicationDemo();
            if (habboCommDemoInstance == null) return false;

            ASCode initCryptoCode = null;
            int asInterfaceQNameIndex = 0;
            ASMethod initCryptoMethod = null;
            foreach (ASMethod method in habboCommDemoInstance.GetMethods(null, "void", 1))
            {
                ASParameter parameter = method.Parameters[0];
                if (initCryptoCode == null &&
                    parameter.IsOptional &&
                    parameter.Type.Name == "Event")
                {
                    initCryptoMethod = method;
                    initCryptoCode = method.Body.ParseCode();

                    int firstCoerceIndex = initCryptoCode.IndexOf(OPCode.Coerce);
                    asInterfaceQNameIndex = ((CoerceIns)initCryptoCode[firstCoerceIndex]).TypeNameIndex;
                }
                else if (parameter.TypeIndex == asInterfaceQNameIndex)
                {
                    int beforeExitIndex = initCryptoCode.Count - 6;
                    initCryptoCode.RemoveRange(beforeExitIndex, 5);
                    initCryptoCode.InsertRange(beforeExitIndex, new ASInstruction[]
                    {
                        new GetLocal0Ins(),
                        new GetLocal2Ins(),
                        new CallPropVoidIns(abc, method.Trait.QNameIndex, 1)
                    });
                    initCryptoMethod.Body.Code = initCryptoCode.ToArray();
                    return true;
                }
            }
            return false;
        }
        public bool DisableHostChecks()
        {
            ASMethod localHostCheckMethod = _abcFiles[0].Classes[0].GetMethod(null, "Boolean", 1);
            if (localHostCheckMethod == null) return false;

            ASInstance habboInstance = _abcFiles[1].GetInstance("Habbo");
            if (habboInstance == null) return false;

            ASMethod remoteHostCheckMethod = habboInstance.GetMethod(null, "Boolean", new[] { "String", "Object" });
            if (remoteHostCheckMethod == null) return false;

            localHostCheckMethod.Body.Code[0] = remoteHostCheckMethod.Body.Code[0] = (byte)OPCode.PushTrue;
            localHostCheckMethod.Body.Code[1] = remoteHostCheckMethod.Body.Code[1] = (byte)OPCode.ReturnValue;
            return LockInfoHostProperty(out _);
        }

        public bool InjectEndPointShouter(int messageId)
        {
            ABCFile abc = _abcFiles.Last();
            ASInstance socketConnInstance = abc.GetInstance("SocketConnection");
            if (socketConnInstance == null) return false;

            ASTrait sendFunction = InjectUniversalSendFunction(IsPostShuffle);
            if (sendFunction == null) return false;

            ASInstance habboCommDemoInstance = GetHabboCommunicationDemo();
            if (habboCommDemoInstance == null) return false;

            if (!InjectEndPointSaver(out ASTrait hostTrait, out ASTrait portTrait)) return false;
            foreach (ASMethod method in habboCommDemoInstance.GetMethods(null, "void", 1))
            {
                if (!IsPostShuffle && !method.Name.EndsWith("onConnectionEstablished")) continue;

                ASParameter parameter = method.Parameters[0];
                if (!parameter.IsOptional) continue;
                if (parameter.Type.Name != "Event") continue;

                ASCode code = method.Body.ParseCode();
                for (int i = 0; i < code.Count; i++)
                {
                    ASInstruction instruction = code[i];
                    if (instruction.OP != OPCode.GetLocal_2) continue;
                    if (code[i + 1].OP == OPCode.PushNull) continue;

                    // local2.sendMessage(messageId, local2.remoteHost, int(local2.remotePort))
                    code.InsertRange(i - 1, new ASInstruction[]
                    {
                        new GetLocal2Ins(),

                        new PushIntIns(abc, messageId),

                        new GetLocal2Ins(),
                        new GetPropertyIns(abc, hostTrait.QNameIndex),

                        new GetLocal2Ins(),
                        new GetPropertyIns(abc, portTrait.QNameIndex),
                        new ConvertIIns(),

                        new CallPropVoidIns(abc, sendFunction.QNameIndex, 3)
                    });

                    method.Body.MaxStack += 5;
                    method.Body.Code = code.ToArray();
                    return true;
                }
            }
            return false;
        }
        public bool InjectEndPoint(string host, int port)
        {
            ABCFile abc = _abcFiles.Last();

            ASInstance socketConnInstance = abc.GetInstance("SocketConnection");
            if (socketConnInstance == null) return false;

            ASMethod initMethod = socketConnInstance.GetMethod("init", "Boolean", 2);
            if (initMethod == null) return false;

            if (!InjectEndPointSaver(out _, out _)) return false;

            ASCode code = initMethod.Body.ParseCode();
            for (int i = 0; i < code.Count; i++)
            {
                ASInstruction instruction = code[i];
                if (instruction.OP != OPCode.CallPropVoid) continue;

                var callPropVoid = (CallPropVoidIns)instruction;
                if (callPropVoid.PropertyName.Name == "connect" && callPropVoid.ArgCount == 2)
                {
                    code.InsertRange(i, new ASInstruction[]
                    {
                        new PopIns(),
                        new PopIns(),
                        new PushStringIns(abc, host),
                        new PushIntIns(abc, port)
                    });
                    break;
                }
            }
            initMethod.Body.Code = code.ToArray();

            HasPingInstructions = GetConnectionInitiationCount() > 1;
            return true;
        }
        public bool InjectKeyShouter(int messageId = 4002)
        {
            ABCFile abc = _abcFiles.Last();
            ASInstance socketConnInstance = abc.GetInstance("SocketConnection");
            if (socketConnInstance == null) return false;

            ASTrait sendFunction = InjectUniversalSendFunction(true);
            if (sendFunction == null) return false;

            ASInstance habboCommDemoInstance = GetHabboCommunicationDemo();
            if (habboCommDemoInstance == null) return false;

            // TODO: Implement a more "dynamic" approach(scan each method for a pattern).
            ASMethod pubKeyVerifyMethod = habboCommDemoInstance.GetMethods(null, "void", 1)
                .Where(m => m.Body.MaxStack == 4 &&
                            m.Body.LocalCount == 10 &&
                            m.Body.MaxScopeDepth == 6 &&
                            m.Body.InitialScopeDepth == 5)
                .FirstOrDefault();
            if (pubKeyVerifyMethod == null) return false;

            int coereceCount = 0;
            ASCode pubKeyVerCode = pubKeyVerifyMethod.Body.ParseCode();
            foreach (ASInstruction instruct in pubKeyVerCode)
            {
                if (instruct.OP == OPCode.Coerce &&
                    (++coereceCount == 2))
                {
                    var coerceIns = (CoerceIns)instruct;
                    coerceIns.TypeNameIndex = socketConnInstance.QNameIndex;
                    break;
                }
            }
            pubKeyVerCode.InsertRange(pubKeyVerCode.Count - 5, new ASInstruction[]
            {
                // local2.sendMessage({messageId}, local6);
                new GetLocal2Ins(),
                new PushIntIns(abc, messageId),
                new GetLocalIns(6),
                new CallPropVoidIns(abc, sendFunction.QNameIndex, 2)
            });

            pubKeyVerifyMethod.Body.Code = pubKeyVerCode.ToArray();
            return true;
        }
        public bool InjectRSAKeys(string exponent, string modulus)
        {
            ABCFile abc = _abcFiles.Last();

            ASInstance habboCommDemoInstance = GetHabboCommunicationDemo();
            if (habboCommDemoInstance == null) return false;

            foreach (ASMethod method in habboCommDemoInstance.GetMethods(null, "void", 1))
            {
                if (method.Body.LocalCount < 10) continue;

                ASCode code = method.Body.ParseCode();
                for (int i = 0; i < code.Count; i++)
                {
                    ASInstruction instruction = code[i];

                    if (instruction.OP != OPCode.InitProperty) continue;
                    var initProperty = (InitPropertyIns)instruction;

                    if (code[i + 3].OP != OPCode.GetProperty) continue;
                    var getProperty = (GetPropertyIns)code[i + 3];

                    if (initProperty.PropertyNameIndex != getProperty.PropertyNameIndex) continue;
                    if (code[i + 8].OP != OPCode.CallPropVoid) continue;

                    var callPropVoid = (CallPropVoidIns)code[i + 8];
                    if (callPropVoid.ArgCount != 3) continue; // Don't use the 'verify' name, as it could get shuffled in the future

                    code.RemoveRange(i - 7, 6);
                    code.InsertRange(i - 7, new ASInstruction[]
                    {
                        new PushStringIns(abc, modulus),
                        new PushStringIns(abc, exponent),
                    });

                    method.Body.Code = code.ToArray();
                    return true;
                }
            }
            return false;
        }

        private void LoadMessages()
        {
            ABCFile abc = _abcFiles.Last();
            ASClass habboMessagesClass = null;
            foreach (ASClass @class in abc.Classes)
            {
                if (@class.Traits.Count != 2 || @class.Instance.Traits.Count != 2) continue;
                if (@class.Traits[0].Type.Name != "Map" || @class.Traits[1].Type.Name != "Map") continue;
                if (@class.Traits[0].Kind != TraitKind.Constant || @class.Traits[1].Kind != TraitKind.Constant) continue;

                habboMessagesClass = @class;
                break;
            }

            if (habboMessagesClass == null)
            {
                IsPostShuffle = false;
                foreach (ASClass @class in abc.Classes)
                {
                    if (@class.Traits.Count != 2) continue;
                    if (@class.Instance.Traits.Count != 3) continue;

                    habboMessagesClass = @class;
                    break;
                }
            }
            if (habboMessagesClass == null) return; // Neither pre/post shuffle client.

            ASCode code = habboMessagesClass.Constructor.Body.ParseCode();
            int inMapTypeIndex = habboMessagesClass.Traits[0].QNameIndex;
            int outMapTypeIndex = habboMessagesClass.Traits[1].QNameIndex;

            ASInstruction[] instructions = code
                .Where(i => i.OP == OPCode.GetLex ||
                            i.OP == OPCode.PushShort ||
                            i.OP == OPCode.PushByte).ToArray();

            for (int i = 0; i < instructions.Length; i += 3)
            {
                var getLexInst = instructions[i + 0] as GetLexIns;
                bool isOutgoing = getLexInst.TypeNameIndex == outMapTypeIndex;

                var primitive = instructions[i + 1] as Primitive;
                short id = Convert.ToInt16(primitive.Value);

                getLexInst = instructions[i + 2] as GetLexIns;
                ASClass messageClass = abc.GetClass(getLexInst.TypeName);

                ASClass parser = null;
                string structure = null;
                if (!isOutgoing)
                {
                    parser = GetMessageParser(messageClass);
                    if (parser != null)
                    {
                        structure = GetIncomingStructure(parser);
                    }
                }
                else structure = GetOutgoingStructure(messageClass, messageClass);

                var flashMsg = new FlashMessage(id, structure, isOutgoing, messageClass, parser, new List<MessageReference>());
                if (!_flashMsgs.ContainsKey(messageClass))
                {
                    _flashMsgs.Add(messageClass, flashMsg);
                }

                if (id == 4000 && isOutgoing)
                {
                    ASInstance messageInstance = messageClass.Instance;
                    ASMethod toArrayMethod = messageInstance.GetMethod(null, "Array", 0);

                    ASCode toArrayCode = toArrayMethod.Body.ParseCode();
                    int index = toArrayCode.IndexOf(OPCode.PushString);

                    if (index == -1) continue;
                    Revision = ((PushStringIns)toArrayCode[index]).Value;
                }
            }
        }
        private void SimplifySendCode(ABCFile abc, ASCode sendCode)
        {
            if (!IsPostShuffle)
            {
                sendCode.Deobfuscate();
            }

            bool isTrimming = true;
            for (int i = 0; i < sendCode.Count; i++)
            {
                ASInstruction instruction = sendCode[i];
                if (!isTrimming && Local.IsValid(instruction.OP))
                {
                    var local = (Local)instruction;
                    int newRegister = local.Register - (IsPostShuffle ? 1 : 3);
                    if (newRegister < 1) continue;

                    ASInstruction replacement = null;
                    if (Local.IsGetLocal(local.OP))
                    {
                        replacement = new GetLocalIns(newRegister);
                    }
                    else if (Local.IsSetLocal(local.OP))
                    {
                        replacement = new SetLocalIns(newRegister);
                    }
                    sendCode[i] = replacement;
                }
                else if (isTrimming)
                {
                    if (instruction.OP != OPCode.SetLocal) continue;

                    var setLocal = (SetLocalIns)instruction;
                    if (IsPostShuffle && setLocal.Register != 4) continue;
                    if (!IsPostShuffle && setLocal.Register != 6) continue;

                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (sendCode[j].OP != OPCode.GetLocal_0) continue;
                        sendCode.RemoveRange(0, j);
                        break;
                    }

                    if (!IsPostShuffle)
                    {
                        sendCode.RemoveAt(5);
                        ((CallPropertyIns)sendCode[5]).ArgCount = 2;
                    }

                    int idNameIndex = abc.Pool.AddConstant("id");
                    int valuesNameIndex = abc.Pool.AddConstant("values");

                    i = 0;
                    isTrimming = false;
                    sendCode.InsertRange(0, new ASInstruction[]
                    {
                        new GetLocal0Ins(),
                        new PushScopeIns(),
                        new DebugIns(abc, idNameIndex, 1, 0),
                        new DebugIns(abc, valuesNameIndex, 1, 1)
                    });
                }
            }
        }
        private ASTrait InjectUniversalSendFunction(bool disableCrypto)
        {
            ABCFile abc = _abcFiles.Last();
            if (disableCrypto && !DisableEncryption()) return null;

            ASInstance socketConnInstance = abc.GetInstance("SocketConnection");
            if (socketConnInstance == null) return null;

            ASMethod sendMethod = socketConnInstance.GetMethod("send", "Boolean", IsPostShuffle ? 1 : 2);
            if (sendMethod == null) return null;

            ASTrait sendFunctionTrait = socketConnInstance.GetMethod("sendMessage", "Boolean", 1)?.Trait;
            if (sendFunctionTrait != null) return sendFunctionTrait;

            ASCode sendCode = sendMethod.Body.ParseCode();
            SimplifySendCode(abc, sendCode);

            var sendMessageMethod = new ASMethod(abc);
            sendMessageMethod.Flags |= MethodFlags.NeedRest;
            sendMessageMethod.ReturnTypeIndex = sendMethod.ReturnTypeIndex;
            int sendMessageMethodIndex = abc.AddMethod(sendMessageMethod);

            // The parameters for the instructions to expect / use.
            var idParam = new ASParameter(abc.Pool, sendMessageMethod)
            {
                NameIndex = abc.Pool.AddConstant("id"),
                TypeIndex = abc.Pool.GetMultinameIndex("int")
            };
            sendMessageMethod.Parameters.Add(idParam);

            // The method body that houses the instructions.
            var sendMessageBody = new ASMethodBody(abc)
            {
                MethodIndex = sendMessageMethodIndex,
                Code = sendCode.ToArray(),
                InitialScopeDepth = 5,
                MaxScopeDepth = 6,
                LocalCount = 10,
                MaxStack = 5
            };
            abc.AddMethodBody(sendMessageBody);

            socketConnInstance.AddMethod(sendMessageMethod, "sendMessage");
            return sendMessageMethod.Trait;
        }

        private bool DisableEncryption()
        {
            ABCFile abc = _abcFiles.Last();

            ASInstance socketConnInstance = abc.GetInstance("SocketConnection");
            if (socketConnInstance == null) return false;

            ASMethod sendMethod = socketConnInstance.GetMethod("send", "Boolean", 1);
            if (sendMethod == null) return false;

            ASCode sendCode = sendMethod.Body.ParseCode();
            sendCode.Deobfuscate();

            ASTrait socketSlot = socketConnInstance.GetSlotTraits("Socket").FirstOrDefault();
            if (socketSlot == null) return false;

            int encodedLocal = -1;
            for (int i = 0; i < sendCode.Count; i++)
            {
                ASInstruction instruction = sendCode[i];
                if (instruction.OP != OPCode.CallProperty) continue;

                var callProperty = (CallPropertyIns)instruction;
                if (callProperty.PropertyName.Name != "encode") continue;

                instruction = sendCode[i += 2];
                if (!Local.IsSetLocal(instruction.OP)) continue;

                encodedLocal = ((Local)instruction).Register;
                sendCode.RemoveRange(i + 1, sendCode.Count - (i + 1));
                break;
            }
            if (encodedLocal == -1) return false;

            sendCode.AddRange(new ASInstruction[]
            {
                new GetLocal0Ins(),
                new GetPropertyIns(abc, socketSlot.QNameIndex),
                new GetLocalIns(encodedLocal),
                new CallPropVoidIns(abc, abc.Pool.GetMultinameIndex("writeBytes"), 1),

                new GetLocal0Ins(),
                new GetPropertyIns(abc, socketSlot.QNameIndex),
                new CallPropVoidIns(abc, abc.Pool.GetMultinameIndex("flush")),

                new PushTrueIns(),
                new ReturnValueIns()
            });
            sendMethod.Body.Code = sendCode.ToArray();
            return true;
        }
        private int GetConnectionInitiationCount()
        {
            int initiationCount = 0;
            if (!LockInfoHostProperty(out ASTrait infoHostSlot)) return initiationCount;

            ASMethod connectMethod = GetManagerConnectMethod();
            if (connectMethod == null) return initiationCount;

            ASCode connectCode = connectMethod.Body.ParseCode();
            for (int i = 0; i < connectCode.Count; i++)
            {
                ASInstruction instruction = connectCode[i];

                if (instruction.OP != OPCode.GetProperty) continue;
                var getPropertyIns = (GetPropertyIns)instruction;

                if (getPropertyIns.PropertyName != infoHostSlot.QName) continue;
                initiationCount++;
            }
            return initiationCount;
        }
        private bool LockInfoHostProperty(out ASTrait infoHostSlot)
        {
            infoHostSlot = null;
            ABCFile abc = _abcFiles.Last();

            ASMethod connectMethod = GetManagerConnectMethod();
            if (connectMethod == null) return false;

            ASCode connectCode = connectMethod.Body.ParseCode();
            int pushByteIndex = connectCode.IndexOf(OPCode.PushByte);

            ASInstance habboCommunicationManager = GetHabboCommunicationManager();
            if (habboCommunicationManager == null) return false;

            infoHostSlot = habboCommunicationManager.GetSlotTraits("String").FirstOrDefault();
            if (infoHostSlot == null) return false;

            int getPropertyIndex = abc.Pool.GetMultinameIndex("getProperty");
            if ((connectCode[pushByteIndex - 3] is PushStringIns pushStringIns) && pushStringIns.Value == "connection.info.host") return true; // Already locked
            connectCode.InsertRange(pushByteIndex, new ASInstruction[]
            {
                new GetLocal0Ins(),
                new FindPropStrictIns(abc, getPropertyIndex),
                new PushStringIns(abc, "connection.info.host"),
                new CallPropertyIns(abc, getPropertyIndex, 1),
                new InitPropertyIns(abc, infoHostSlot.QNameIndex)
            });

            // This portion prevents any suffix from being added to the host slot.
            int magicInverseIndex = abc.Pool.AddConstant(65290);
            for (int i = 0; i < connectCode.Count; i++)
            {
                ASInstruction instruction = connectCode[i];
                if (instruction.OP != OPCode.PushInt) continue;
                if (connectCode[i - 1].OP == OPCode.Add) continue;

                var pushIntIns = (PushIntIns)instruction;
                pushIntIns.ValueIndex = magicInverseIndex;
            }

            connectMethod.Body.MaxStack += 4;
            connectMethod.Body.Code = connectCode.ToArray();
            return true;
        }
        private bool InjectEndPointSaver(out ASTrait hostTrait, out ASTrait portTrait)
        {
            hostTrait = portTrait = null;
            ABCFile abc = _abcFiles.Last();

            ASInstance socketConnection = abc.GetInstance("SocketConnection");
            if (socketConnection == null) return false;

            foreach (ASTrait slot in socketConnection.GetTraits(TraitKind.Slot))
            {
                if (slot.QName.Name == "remoteHost" && slot.Type.Name == "String")
                {
                    hostTrait = slot;
                }
                else if (slot.QName.Name == "remotePort" && slot.Type.Name == "int")
                {
                    portTrait = slot;
                }
                if (hostTrait != null && portTrait != null) return true;
            }

            portTrait = AddPublicSlot(socketConnection, "remotePort", "int");
            hostTrait = AddPublicSlot(socketConnection, "remoteHost", "String");

            ASMethod init = socketConnection.GetMethod("init", "Boolean", 2);
            if (init == null) return false;

            ASCode code = init.Body.ParseCode();

            for (int i = 0; i < code.Count; i++)
            {
                ASInstruction instruction = code[i];
                switch (instruction.OP)
                {
                    case OPCode.CallPropVoid:
                    {
                        var callPropVoid = (CallPropVoidIns)instruction;
                        if (callPropVoid.PropertyName.Name != "connect" || callPropVoid.ArgCount != 2) continue;

                        code.InsertRange(i + 1, new ASInstruction[]
                        {
                            new GetLocal0Ins(),
                            new GetLocal1Ins(),
                            new SetPropertyIns(abc, hostTrait.QNameIndex),

                            new GetLocal0Ins(),
                            new GetLocal2Ins(),
                            new SetPropertyIns(abc, portTrait.QNameIndex)
                        });
                        init.Body.Code = code.ToArray();
                        return true;
                    }
                }
            }
            return false;
        }

        private ASMethod GetManagerConnectMethod()
        {
            if (_habboCommManagerConnect != null) return _habboCommManagerConnect;

            ASInstance habboCommunicationManager = GetHabboCommunicationManager();
            if (habboCommunicationManager == null) return null;

            ASTrait hostTrait = habboCommunicationManager.GetSlotTraits("String").FirstOrDefault();
            if (hostTrait == null) return null;

            ASMethod initComponent = habboCommunicationManager.GetMethod("initComponent", "void", 0);
            if (initComponent == null) return null;

            string connectMethodName = null;
            ASCode initComponentCode = initComponent.Body.ParseCode();
            for (int i = initComponentCode.Count - 1; i >= 0; i--)
            {
                ASInstruction instruction = initComponentCode[i];
                if (instruction.OP != OPCode.CallPropVoid) continue;

                var callPropVoidIns = (CallPropVoidIns)instruction;
                connectMethodName = callPropVoidIns.PropertyName.Name;
                break;
            }

            if (string.IsNullOrWhiteSpace(connectMethodName)) return null;
            return _habboCommManagerConnect = habboCommunicationManager.GetMethod(connectMethodName, "void", 0);
        }
        private ASInstance GetHabboCommunicationDemo()
        {
            if (_habboCommDemo == null)
            {
                _habboCommDemo = _abcFiles.Last().GetInstance("HabboCommunicationDemo");
                if (_habboCommDemo != null) return _habboCommDemo;

                foreach (ASInstance instance in _abcFiles.Last().Instances)
                {
                    if (instance.Super == null) continue;
                    if (!instance.Super.Name.ToLower().Equals("element") && !instance.Super.Name.ToLower().Equals("component")) continue;

                    if (instance.IsStatic) continue;
                    if (instance.InterfaceIndices.Count > 0) continue;
                    if (instance.Constructor.Parameters.Count != 3) continue;
                    if (instance.Traits.Count < 35 || instance.Traits.Count >= 50) continue;

                    _habboCommDemo = instance;
                    break;
                }
            }
            return _habboCommDemo;
        }
        private ASInstance GetHabboCommunicationManager()
        {
            if (_habboCommManager == null)
            {
                _habboCommManager = _abcFiles.Last().GetInstance("HabboCommunicationManager");
                if (_habboCommManager != null) return _habboCommManager;

                foreach (ASInstance instance in _abcFiles.Last().Instances)
                {
                    if (instance.Super == null) continue;
                    if (!instance.Super.Name.ToLower().Equals("element") && !instance.Super.Name.ToLower().Equals("component")) continue;

                    if (instance.InterfaceIndices.Count != 2) continue;
                    if (instance.Constructor.Parameters.Count != 3) continue;
                    if (instance.Traits.Count < 35 || instance.Traits.Count >= 50) continue;

                    _habboCommManager = instance;
                    break;
                }
            }
            return _habboCommManager;
        }

        public byte[] ToArray(CompressionKind compression)
        {
            using var assembledOutput = new MemoryStream((int)_flash.FileLength);
            using var writer = new FlashWriter(assembledOutput);
            {
                Assemble(writer, compression);
                writer.Flush();
                return assembledOutput.ToArray();
            }
        }
        public void Patch(short keyShouterId, string host, int port)
        {
            if (IsPostShuffle)
            {
                DisableHostChecks();
                InjectKeyShouter();
            }
            InjectEndPointShouter(keyShouterId);
            InjectEndPoint(host, port);
        }

        public void Disassemble()
        {
            _flash.Disassemble(t =>
            {
                if (t.Kind != TagKind.DoABC) return;

                var doABCTag = (DoABCTag)t;
                var abcFile = new ABCFile(doABCTag.ABCData);

                _abcFileTags[doABCTag] = abcFile;
                _abcFiles.Add(abcFile);
            });
            LoadMessages();
        }
        public void Assemble(FlashWriter output, CompressionKind compression) => _flash.Assemble(output, compression, t =>
        {
            if (t.Kind != TagKind.DoABC) return;

            var doABCTag = (DoABCTag)t;
            doABCTag.ABCData = _abcFileTags[doABCTag].ToArray();
        });

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _flash.Dispose();
                _abcFiles.Clear();
                _flashMsgs.Clear();
                _abcFileTags.Clear();
            }
            _flash = null;
            _abcFiles = null;
            _flashMsgs = null;
            _abcFileTags = null;
            _habboCommManagerConnect = null;
            _habboCommDemo = _habboCommManager = null;
        }

        private record FlashMessage(short Id, string Structure, bool IsOutgoing, ASClass MessageClass, ASClass ParserClass, List<MessageReference> References);

        private static string GenerateHash(FlashMessage flashMsg, int collisionRank = 0)
        {
            using var output = new MessageHasher(false);
            output.Write(flashMsg.IsOutgoing);
            if (collisionRank > 0)
            {
                output.Write(collisionRank);
            }
            if (!IsValidIdentifier(flashMsg.MessageClass.QName.Name, true))
            {
                output.Write(flashMsg.MessageClass.Instance, true);
                output.Write(flashMsg.MessageClass.Instance.Constructor);

                output.Write(flashMsg.References.Count);
                foreach (MessageReference reference in flashMsg.References)
                {
                    output.Write(reference.IsStatic);
                    output.Write(reference.IsAnonymous);

                    output.Write(reference.MethodRank);
                    output.Write(reference.InstructionRank);

                    output.Write(reference.FromMethod);

                    output.Write(reference.FromClass.Constructor);
                    output.Write(reference.FromClass.Instance.Constructor);
                }
                if (!flashMsg.IsOutgoing && flashMsg.ParserClass != null)
                {
                    output.Write(flashMsg.ParserClass.Instance, true);
                }
            }
            else output.Write(flashMsg.MessageClass.QName.Name);
            return output.Generate();
        }
        #region Structure Extraction
        private ASClass GetMessageParser(ASClass messageClass)
        {
            ABCFile abc = messageClass.GetABC();
            ASInstance instance = messageClass.Instance;

            ASInstance superInstance = abc.GetInstance(instance.Super);
            if (superInstance == null) superInstance = instance;

            ASMethod parserGetterMethod = superInstance.GetGetter("parser")?.Method;
            if (parserGetterMethod == null) return null;

            IEnumerable<ASMethod> methods = instance.GetMethods();
            foreach (ASMethod method in methods.Concat(new[] { instance.Constructor }))
            {
                ASCode code = method.Body.ParseCode();
                foreach (ASInstruction instruction in code)
                {
                    ASMultiname multiname = null;
                    if (instruction.OP == OPCode.FindPropStrict)
                    {
                        var findPropStrictIns = (FindPropStrictIns)instruction;
                        multiname = findPropStrictIns.PropertyName;
                    }
                    else if (instruction.OP == OPCode.GetLex)
                    {
                        var getLexIns = (GetLexIns)instruction;
                        multiname = getLexIns.TypeName;
                    }
                    else continue;

                    foreach (ASClass refClass in abc.GetClasses(multiname))
                    {
                        ASInstance refInstance = refClass.Instance;
                        if (refInstance.ContainsInterface(parserGetterMethod.ReturnType.Name))
                        {
                            return refClass;
                        }
                    }
                }
            }
            return null;
        }

        private string GetIncomingStructure(ASClass @class)
        {
            ASMethod parseMethod = @class.Instance.GetMethod("parse", "Boolean", 1);
            return GetIncomingStructure(@class.Instance, parseMethod);
        }
        private string GetIncomingStructure(ASInstance instance, ASMethod method)
        {
            if (method.Body.Exceptions.Count > 0) return null;

            ASCode code = method.Body.ParseCode();
            if (code.JumpExits.Count > 0 || code.SwitchExits.Count > 0) return null;

            string structure = null;
            ABCFile abc = method.GetABC();
            for (int i = 0; i < code.Count; i++)
            {
                ASInstruction instruction = code[i];
                if (instruction.OP != OPCode.GetLocal_1) continue;

                ASInstruction next = code[++i];
                switch (next.OP)
                {
                    case OPCode.CallProperty:
                    {
                        var callProperty = (CallPropertyIns)next;
                        if (callProperty.ArgCount > 0)
                        {
                            ASMultiname propertyName = null;
                            ASInstruction previous = code[i - 2];

                            switch (previous.OP)
                            {
                                case OPCode.GetLex:
                                {
                                    var getLex = (GetLexIns)previous;
                                    propertyName = getLex.TypeName;
                                    break;
                                }

                                case OPCode.ConstructProp:
                                {
                                    var constructProp = (ConstructPropIns)previous;
                                    propertyName = constructProp.PropertyName;
                                    break;
                                }

                                case OPCode.GetLocal_0:
                                {
                                    propertyName = instance.QName;
                                    break;
                                }
                            }

                            ASInstance innerInstance = abc.GetInstance(propertyName);
                            ASMethod innerMethod = innerInstance.GetMethod(callProperty.PropertyName.Name, null, callProperty.ArgCount);
                            if (innerMethod == null)
                            {
                                ASClass innerClass = abc.GetClass(propertyName);
                                innerMethod = innerClass.GetMethod(callProperty.PropertyName.Name, null, callProperty.ArgCount);
                            }

                            string innerStructure = GetIncomingStructure(innerInstance, innerMethod);
                            if (string.IsNullOrWhiteSpace(innerStructure)) return null;
                            structure += innerStructure;
                        }
                        else
                        {
                            if (!TryGetStructurePiece(false, callProperty.PropertyName, null, out char piece)) return null;
                            structure += piece;
                        }
                        break;
                    }

                    case OPCode.ConstructProp:
                    {
                        var constructProp = (ConstructPropIns)next;
                        ASInstance innerInstance = abc.GetInstance(constructProp.PropertyName);

                        string innerStructure = GetIncomingStructure(innerInstance, innerInstance.Constructor);
                        if (string.IsNullOrWhiteSpace(innerStructure)) return null;
                        structure += innerStructure;
                        break;
                    }

                    case OPCode.ConstructSuper:
                    {
                        ASInstance superInstance = abc.GetInstance(instance.Super);

                        string innerStructure = GetIncomingStructure(superInstance, superInstance.Constructor);
                        if (string.IsNullOrWhiteSpace(innerStructure)) return null;
                        structure += innerStructure;
                        break;
                    }

                    case OPCode.CallSuper:
                    {
                        var callSuper = (CallSuperIns)next;
                        ASInstance superInstance = abc.GetInstance(instance.Super);
                        ASMethod superMethod = superInstance.GetMethod(callSuper.MethodName.Name, null, callSuper.ArgCount);

                        string innerStructure = GetIncomingStructure(superInstance, superMethod);
                        if (string.IsNullOrWhiteSpace(innerStructure)) return null;
                        structure += innerStructure;
                        break;
                    }

                    case OPCode.CallPropVoid:
                    {
                        var callPropVoid = (CallPropVoidIns)next;
                        if (callPropVoid.ArgCount != 0) return null;

                        if (!TryGetStructurePiece(false, callPropVoid.PropertyName, null, out char piece)) return null;
                        structure += piece;
                        break;
                    }

                    default: return null;
                }
            }
            return structure;
        }

        private string GetOutgoingStructure(ASClass messageClass, ASClass @class)
        {
            ASMethod getArrayMethod = @class.Instance.GetMethod(null, "Array", 0);
            if (getArrayMethod == null)
            {
                ASClass superClass = @class.GetABC().GetClass(@class.Instance.Super);
                return GetOutgoingStructure(messageClass, superClass);
            }
            if (getArrayMethod.Body.Exceptions.Count > 0) return null;
            ASCode getArrayCode = getArrayMethod.Body.ParseCode();

            if (getArrayCode.JumpExits.Count > 0 || getArrayCode.SwitchExits.Count > 0)
            {
                // Unable to parse data structure that relies on user input that is not present,
                // since the structure may change based on the provided input.
                return null;
            }

            ASInstruction resultPusher = null;
            for (int i = getArrayCode.Count - 1; i >= 0; i--)
            {
                ASInstruction instruction = getArrayCode[i];
                if (instruction.OP == OPCode.ReturnValue)
                {
                    resultPusher = getArrayCode[i - 1];
                    break;
                }
            }

            int argCount = -1;
            if (resultPusher.OP == OPCode.ConstructProp)
            {
                argCount = ((ConstructPropIns)resultPusher).ArgCount;
            }
            else if (resultPusher.OP == OPCode.NewArray)
            {
                argCount = ((NewArrayIns)resultPusher).ArgCount;
            }

            if (argCount > 0)
            {
                return GetOutgoingStructure(messageClass, getArrayCode, resultPusher, argCount);
            }
            else if (argCount == 0 || resultPusher.OP == OPCode.PushNull)
            {
                return null;
            }

            if (resultPusher.OP == OPCode.GetProperty)
            {
                var getProperty = (GetPropertyIns)resultPusher;
                return GetOutgoingStructure(messageClass, getProperty.PropertyName);
            }
            else if (Local.IsGetLocal(resultPusher.OP))
            {
                return GetOutgoingStructure(messageClass, getArrayCode, (Local)resultPusher);
            }
            return null;
        }
        private string GetOutgoingStructure(ASClass messageClass, ASCode code, Local getLocal)
        {
            string structure = null;
            for (int i = 0; i < code.Count; i++)
            {
                ASInstruction instruction = code[i];
                if (instruction == getLocal) break;
                if (!Local.IsGetLocal(instruction.OP)) continue;

                var local = (Local)instruction;
                if (local.Register != getLocal.Register) continue;

                for (i += 1; i < code.Count; i++)
                {
                    ASInstruction next = code[i];
                    if (next.OP != OPCode.CallPropVoid) continue;

                    var callPropVoid = (CallPropVoidIns)next;
                    if (callPropVoid.PropertyName.Name != "push") continue;

                    ASInstruction previous = code[i - 1];
                    if (previous.OP != OPCode.GetProperty) continue;

                    ASClass classToCheck = messageClass;
                    var getProperty = (GetPropertyIns)previous;
                    ASMultiname propertyName = getProperty.PropertyName;

                    ASInstruction beforeGetProp = code[i - 2];
                    if (beforeGetProp.OP == OPCode.GetLex)
                    {
                        var getLex = (GetLexIns)beforeGetProp;
                        classToCheck = classToCheck.GetABC().GetClass(getLex.TypeName);
                    }

                    if (!TryGetStructurePiece(true, propertyName, classToCheck, out char piece)) return null;
                    structure += piece;
                }
            }
            return structure;
        }
        private string GetOutgoingStructure(ASClass @class, ASMultiname propertyName)
        {
            ASMethod constructor = @class.Instance.Constructor;
            if (constructor.Body.Exceptions.Count > 0) return null;

            ASCode code = constructor.Body.ParseCode();
            if (code.JumpExits.Count > 0 || code.SwitchExits.Count > 0) return null;

            string structure = null;
            for (int i = 0; i < code.Count; i++)
            {
                ASInstruction instruction = code[i];
                if (instruction.OP == OPCode.NewArray)
                {
                    var newArray = (NewArrayIns)instruction;
                    if (newArray.ArgCount > 0)
                    {
                        var structurePieces = new char[newArray.ArgCount];
                        for (int j = i - 1, length = newArray.ArgCount; j >= 0; j--)
                        {
                            ASInstruction previous = code[j];
                            if (Local.IsGetLocal(previous.OP) && previous.OP != OPCode.GetLocal_0)
                            {
                                var local = (Local)previous;
                                ASParameter parameter = constructor.Parameters[local.Register - 1];

                                if (!TryGetStructurePiece(true, parameter.Type, null, out char piece)) return null;
                                structurePieces[--length] = piece;
                            }
                            if (length == 0)
                            {
                                structure += new string(structurePieces);
                                break;
                            }
                        }
                    }
                }
                else if (instruction.OP == OPCode.ConstructSuper)
                {
                    var constructSuper = (ConstructSuperIns)instruction;
                    if (constructSuper.ArgCount > 0)
                    {
                        ASClass superClass = @class.GetABC().GetClass(@class.Instance.Super);
                        structure += GetOutgoingStructure(superClass, propertyName);
                    }
                }
                if (instruction.OP != OPCode.GetProperty) continue;

                var getProperty = (GetPropertyIns)instruction;
                if (getProperty.PropertyName != propertyName) continue;

                ASInstruction next = code[++i];
                ASClass classToCheck = @class;
                if (Local.IsGetLocal(next.OP))
                {
                    if (next.OP == OPCode.GetLocal_0) continue;

                    var local = (Local)next;
                    ASParameter parameter = constructor.Parameters[local.Register - 1];

                    if (!TryGetStructurePiece(true, parameter.Type, null, out char piece)) return null;
                    structure += piece;
                }
                else
                {
                    if (next.OP == OPCode.FindPropStrict)
                    {
                        classToCheck = null;
                    }
                    else if (next.OP == OPCode.GetLex)
                    {
                        var getLex = (GetLexIns)next;
                        classToCheck = classToCheck.GetABC().GetClass(getLex.TypeName);
                    }
                    do
                    {
                        next = code[++i];
                        propertyName = null;
                        if (next.OP == OPCode.GetProperty)
                        {
                            getProperty = (GetPropertyIns)next;
                            propertyName = getProperty.PropertyName;
                        }
                        else if (next.OP == OPCode.CallProperty)
                        {
                            var callProperty = (CallPropertyIns)next;
                            propertyName = callProperty.PropertyName;
                        }
                    }
                    while (next.OP != OPCode.GetProperty && next.OP != OPCode.CallProperty);

                    if (!TryGetStructurePiece(true, propertyName, classToCheck, out char piece)) return null;
                    structure += piece;
                }
            }
            return structure;
        }
        private string GetOutgoingStructure(ASClass messageClass, ASCode code, ASInstruction beforeReturn, int length)
        {
            var getLocalEndIndex = -1;
            int pushingEndIndex = code.IndexOf(beforeReturn);

            var structure = new char[length];
            var pushedLocals = new Dictionary<int, int>();
            for (int i = pushingEndIndex - 1; i >= 0; i--)
            {
                ASInstruction instruction = code[i];
                if (instruction.OP == OPCode.GetProperty)
                {
                    ASClass classToCheck = messageClass;
                    var getProperty = (GetPropertyIns)instruction;
                    ASMultiname propertyName = getProperty.PropertyName;

                    ASInstruction previous = code[i - 1];
                    if (previous.OP == OPCode.GetLex)
                    {
                        var getLex = (GetLexIns)previous;
                        classToCheck = classToCheck.GetABC().GetClass(getLex.TypeName);
                    }

                    if (!TryGetStructurePiece(true, propertyName, classToCheck, out char piece)) return null;
                    structure[--length] = piece;
                }
                else if (Local.IsGetLocal(instruction.OP) &&
                    instruction.OP != OPCode.GetLocal_0)
                {
                    var local = (Local)instruction;
                    pushedLocals.Add(local.Register, --length);
                    if (getLocalEndIndex == -1)
                    {
                        getLocalEndIndex = i;
                    }
                }
                if (length == 0) break;
            }
            for (int i = getLocalEndIndex - 1; i >= 0; i--)
            {
                ASInstruction instruction = code[i];
                if (!Local.IsSetLocal(instruction.OP)) continue;

                var local = (Local)instruction;
                if (pushedLocals.TryGetValue(local.Register, out int structIndex))
                {
                    ASInstruction beforeSet = code[i - 1];
                    pushedLocals.Remove(local.Register);
                    structure[structIndex] = beforeSet.OP switch
                    {
                        OPCode.PushTrue or OPCode.PushFalse => 'B',
                        OPCode.Coerce_s or OPCode.PushString => 's',
                        OPCode.PushInt or OPCode.PushByte or OPCode.Convert_i => 'i',
                        _ => throw new Exception($"Don't know what this value type is, tell someone about this please.\r\nOP: {beforeSet.OP}"),
                    };
                }
                if (pushedLocals.Count == 0) break;
            }
            return new string(structure);
        }

        private ASMultiname GetTraitType(ASContainer container, ASMultiname traitName)
        {
            if (container == null) return traitName;

            return container.GetTraits(TraitKind.Slot, TraitKind.Constant, TraitKind.Getter)
                .Where(t => t.QName == traitName)
                .FirstOrDefault()?.Type;
        }
        private bool TryGetStructurePiece(bool isOutgoing, ASMultiname multiname, ASClass @class, out char piece)
        {
            ASMultiname returnValueType = multiname;
            if (@class != null)
            {
                returnValueType = GetTraitType(@class, multiname) ?? GetTraitType(@class.Instance, multiname);
            }

            switch (returnValueType.Name.ToLower())
            {
                case "int":
                case "readint":
                case "gettimer": piece = 'i'; break;

                case "byte":
                case "readbyte": piece = 'b'; break;

                case "double":
                case "readdouble": piece = 'd'; break;

                case "string":
                case "readstring": piece = 's'; break;

                case "boolean":
                case "readboolean": piece = 'B'; break;

                case "array": piece = char.MinValue; break;
                default:
                {
                    if (!isOutgoing && !IsValidIdentifier(returnValueType.Name, true))
                    {
                        piece = 'i'; // This reference call is most likely towards 'readInt'
                    }
                    else piece = char.MinValue;
                    break;
                }
            }
            return piece != char.MinValue;
        }
        #endregion

        private static ASTrait AddPublicSlot(ASContainer container, string name, int typeIndex)
        {
            ABCFile abc = container.GetABC();
            var trait = new ASTrait(abc)
            {
                Kind = TraitKind.Slot,
                TypeIndex = typeIndex,
                QNameIndex = abc.Pool.AddConstant(new ASMultiname(abc.Pool)
                {
                    NamespaceIndex = 1,
                    Kind = MultinameKind.QName,
                    NameIndex = abc.Pool.AddConstant(name)
                })
            };

            container.Traits.Add(trait);
            return trait;
        }
        private static ASTrait AddPublicSlot(ASContainer container, string name, string typeName)
        {
            return AddPublicSlot(container, name, container.GetABC().Pool.GetMultinameIndex(typeName));
        }

        private static bool IsValidIdentifier(string value, bool invalidOnSanitized = false)
        {
            value = value.ToLower();
            if (invalidOnSanitized &&
                (value.StartsWith("class_") ||
                value.StartsWith("iinterface_") ||
                value.StartsWith("namespace_") ||
                value.StartsWith("method_") ||
                value.StartsWith("constant_") ||
                value.StartsWith("slot_") ||
                value.StartsWith("param")))
            {
                return false;
            }

            return !value.Contains("_-") && !_reservedNames.Contains(value.Trim());
        }

        private sealed class MessageHasher : BinaryWriter
        {
            private readonly SortedDictionary<int, int> _ints;
            private readonly SortedDictionary<bool, int> _bools;
            private readonly SortedDictionary<byte, int> _bytes;
            private readonly SortedDictionary<string, int> _strings;

            public bool IsSorting { get; set; }

            public MessageHasher(bool isSorting)
                : base(new MemoryStream())
            {
                _ints = new SortedDictionary<int, int>();
                _bools = new SortedDictionary<bool, int>();
                _bytes = new SortedDictionary<byte, int>();
                _strings = new SortedDictionary<string, int>();

                IsSorting = isSorting;
            }

            public override void Write(int value)
            {
                WriteOrSort(_ints, base.Write, value);
            }
            public override void Write(bool value)
            {
                WriteOrSort(_bools, base.Write, value);
            }
            public override void Write(byte value)
            {
                WriteOrSort(_bytes, base.Write, value);
            }
            public override void Write(string value)
            {
                WriteOrSort(_strings, base.Write, value);
            }

            public void Write(ASTrait trait)
            {
                Write(trait.Id);
                Write(trait.QName);
                Write(trait.IsStatic);
                Write((byte)trait.Kind);
                Write((byte)trait.Attributes);
                switch (trait.Kind)
                {
                    case TraitKind.Slot:
                    case TraitKind.Constant:
                    {
                        Write(trait.Type);
                        if (trait.Value != null)
                        {
                            Write(trait.ValueKind, trait.Value);
                        }
                        break;
                    }
                    case TraitKind.Method:
                    case TraitKind.Getter:
                    case TraitKind.Setter:
                    {
                        Write(trait.Method);
                        break;
                    }
                }
            }
            public void Write(ASMethod method)
            {
                Write(method.IsConstructor);
                if (!method.IsConstructor)
                {
                    Write(method.ReturnType);
                }
                Write(method.Parameters.Count);
                foreach (ASParameter parameter in method.Parameters)
                {
                    Write(parameter.Type);
                    if (!string.IsNullOrWhiteSpace(parameter.Name) && IsValidIdentifier(parameter.Name, true))
                    {
                        Write(parameter.Name);
                    }
                    Write(parameter.IsOptional);
                    if (parameter.IsOptional)
                    {
                        Write((byte)parameter.ValueKind);
                        Write(parameter.ValueKind, parameter.Value);
                    }
                }
                ASCode code = method.Body.ParseCode();
                foreach (OPCode op in code.GetOPGroups().Keys)
                {
                    if (op != OPCode.GetLex
                        && op != OPCode.GetProperty
                        && op != OPCode.CallProperty) continue;

                    Write((byte)op);
                }
            }
            public void Write(ASMultiname multiname)
            {
                if (multiname?.Kind == MultinameKind.TypeName)
                {
                    Write(multiname.QName);
                    Write(multiname.TypeIndices.Count);
                    foreach (ASMultiname type in multiname.GetTypes())
                    {
                        Write(type);
                    }
                }
                else if (multiname == null || IsValidIdentifier(multiname.Name, true))
                {
                    Write(multiname?.Name ?? "*");
                }
            }
            public void Write(ConstantKind kind, object value)
            {
                Write((byte)kind);
                switch (kind)
                {
                    case ConstantKind.Double:
                    Write((double)value);
                    break;
                    case ConstantKind.Integer:
                    Write((int)value);
                    break;
                    case ConstantKind.UInteger:
                    Write((uint)value);
                    break;
                    case ConstantKind.String:
                    Write((string)value);
                    break;
                    case ConstantKind.Null:
                    Write("null");
                    break;
                    case ConstantKind.True:
                    Write(true);
                    break;
                    case ConstantKind.False:
                    Write(false);
                    break;
                }
            }
            public void Write(ASContainer container, bool includeTraits)
            {
                Write(container.IsStatic);
                if (includeTraits)
                {
                    Write(container.Traits.Count);
                    container.Traits.ForEach(t => Write(t));
                }
            }

            public string Generate()
            {
                Flush();
                using (var md5 = MD5.Create())
                {
                    long curPos = BaseStream.Position;
                    BaseStream.Position = 0;

                    byte[] hashData = md5.ComputeHash(BaseStream);
                    string hashAsHex = BitConverter.ToString(hashData);

                    BaseStream.Position = curPos;
                    return hashAsHex.Replace("-", string.Empty).ToLower();
                }
            }
            public override void Flush()
            {
                WriteSorted(_ints, base.Write);
                WriteSorted(_bools, base.Write);
                WriteSorted(_bytes, base.Write);
                WriteSorted(_strings, base.Write);
            }

            private void WriteSorted<T>(IDictionary<T, int> storage, Action<T> writer)
            {
                foreach (KeyValuePair<T, int> storedPair in storage)
                {
                    writer(storedPair.Key);
                    base.Write(storedPair.Value);
                }
            }
            private void WriteOrSort<T>(IDictionary<T, int> storage, Action<T> writer, T value)
            {
                if (IsSorting)
                {
                    if (storage.ContainsKey(value))
                    {
                        storage[value]++;
                    }
                    else storage.Add(value, 1);
                }
                else writer(value);
            }
        }
        private sealed class MessageReference
        {
            public bool IsStatic { get; set; }
            public bool IsAnonymous { get; set; }

            public int ClassRank { get; set; }
            public int MethodRank { get; set; }
            public int InstructionRank { get; set; }

            public int GroupCount { get; set; }

            public ASClass FromClass { get; set; }
            public ASMethod FromMethod { get; set; }
        }
    }
}