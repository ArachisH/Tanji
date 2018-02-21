using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

using Flazzy;
using Flazzy.IO;
using Flazzy.ABC;
using Flazzy.Tags;
using Flazzy.Records;
using Flazzy.ABC.AVM2;
using Flazzy.ABC.AVM2.Instructions;

namespace Sulakore.Habbo
{
    [Flags]
    public enum Sanitizers
    {
        None = 0,
        Deobfuscate = 1,
        RegisterRename = 2,
        IdentifierRename = 4
    }

    public class HGame : ShockwaveFlash
    {
        private ASMethod _managerConnectMethod;

        private readonly Dictionary<DoABCTag, ABCFile> _abcFileTags;
        private readonly Dictionary<ASClass, MessageItem> _messages;

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

        public List<ABCFile> ABCFiles { get; }
        public bool IsPostShuffle { get; private set; } = true;

        public SortedDictionary<ushort, MessageItem> InMessages { get; }
        public SortedDictionary<ushort, MessageItem> OutMessages { get; }
        public SortedDictionary<string, List<MessageItem>> Messages { get; }

        private int _revisionIndex;
        public string Revision
        {
            get
            {
                if (ABCFiles.Count >= 3)
                {
                    return ABCFiles.Last().Pool.Strings[_revisionIndex];
                }
                else return "PRODUCTION-000000000000-000000000";
            }
            set
            {
                if (ABCFiles.Count >= 3)
                {
                    ABCFiles.Last().Pool.Strings[_revisionIndex] = value;
                }
            }
        }

        public string Location { get; set; }

        public HGame(string path)
            : this(File.OpenRead(path))
        {
            Location = path;
        }
        public HGame(byte[] data)
            : this(new MemoryStream(data))

        { }
        public HGame(Stream input)
            : this(input, false)
        { }
        public HGame(Stream input, bool leaveOpen)
            : this(new FlashReader(input, leaveOpen))
        { }
        protected HGame(FlashReader input)
            : base(input)
        {
            _abcFileTags = new Dictionary<DoABCTag, ABCFile>();
            _messages = new Dictionary<ASClass, MessageItem>();

            ABCFiles = new List<ABCFile>();
            InMessages = new SortedDictionary<ushort, MessageItem>();
            OutMessages = new SortedDictionary<ushort, MessageItem>();
            Messages = new SortedDictionary<string, List<MessageItem>>();
        }

        public void Sanitize(Sanitizers sanitizations)
        {
            if (sanitizations.HasFlag(Sanitizers.IdentifierRename))
            {
                RenameIdentifiers();
            }
            if (sanitizations.HasFlag(Sanitizers.Deobfuscate))
            {
                Deobfuscate();
            }
            if (sanitizations.HasFlag(Sanitizers.RegisterRename))
            {
                RenameRegisters();
            }
        }
        #region Sanitation
        protected void Deobfuscate()
        {
            foreach (ABCFile abc in ABCFiles)
            {
                foreach (ASMethodBody body in abc.MethodBodies)
                {
                    if (body.Exceptions.Count > 0) continue;
                    if (body.Code[0] == 0x27 && body.Code[1] == 0x26) // PushFalse, PushTrue
                    {
                        ASCode code = body.ParseCode();
                        code.Deobfuscate();

                        body.Code = code.ToArray();
                    }
                }
            }
        }
        protected void RenameRegisters()
        {
            foreach (ABCFile abc in ABCFiles)
            {
                var nameIndices = new Dictionary<string, int>();
                foreach (ASMethodBody body in abc.MethodBodies)
                {
                    if (body.Exceptions.Count > 0) continue;
                    if (body.Code.Length <= 50 && !body.Code.Contains((byte)OPCode.Debug)) continue;

                    ASCode code = body.ParseCode();
                    if (!code.Contains(OPCode.Debug)) continue;

                    bool wasModified = false;
                    List<ASParameter> parameters = body.Method.Parameters;
                    foreach (DebugIns debug in code.GetOPGroup(OPCode.Debug))
                    {
                        if (debug.Name != "k") continue;

                        string name = string.Empty;
                        ASParameter parameter = null;
                        int register = debug.RegisterIndex;
                        if (register < parameters.Count)
                        {
                            parameter = parameters[register];
                            if (!string.IsNullOrWhiteSpace(parameter.Name))
                            {
                                nameIndices.Add(parameter.Name, parameter.NameIndex);
                                name = parameter.Name;
                            }
                            else name = $"param{register + 1}";
                        }
                        else name = $"local{(register - parameters.Count) + 1}";

                        int nameIndex = -1;
                        if (!nameIndices.TryGetValue(name, out nameIndex))
                        {
                            nameIndex = abc.Pool.AddConstant(name, false);
                            nameIndices.Add(name, nameIndex);
                        }
                        if (parameter != null)
                        {
                            parameter.NameIndex = nameIndex;
                        }

                        debug.NameIndex = nameIndex;
                        wasModified = true;
                    }
                    if (wasModified)
                    {
                        body.Code = code.ToArray();
                    }
                }
            }
        }
        protected void RenameIdentifiers()
        {
            // [InvalidNamespaceName] - [ValidNamespaceName]
            var validNamespaces = new Dictionary<string, string>();
            // [ValidNamespaceName.InvalidClassName] - [ValidClassName]
            var validClasses = new SortedDictionary<string, string>();

            int classCount = 0;
            int interfaceCount = 0;
            int namespaceCount = 0;
            foreach (ABCFile abc in ABCFiles)
            {
                var nameIndices = new Dictionary<string, int>();
                foreach (KeyValuePair<string, string> fixedNames in validNamespaces.Concat(validClasses))
                {
                    int index = abc.Pool.AddConstant(fixedNames.Value, false);
                    nameIndices.Add(fixedNames.Key, index);
                }

                #region Namespace Renaming
                foreach (ASNamespace @namespace in abc.Pool.Namespaces)
                {
                    if (@namespace == null) continue;

                    namespaceCount++;
                    if (IsValidIdentifier(@namespace.Name)) continue;

                    int validNameIndex = -1;
                    if (!nameIndices.TryGetValue(@namespace.Name, out validNameIndex))
                    {
                        string validName = ("Namespace_" + namespaceCount.ToString("0000"));
                        validNameIndex = abc.Pool.AddConstant(validName, false);

                        nameIndices.Add(@namespace.Name, validNameIndex);
                        if (!validNamespaces.ContainsKey(@namespace.Name))
                        {
                            validNamespaces.Add(@namespace.Name, validName);
                        }
                    }
                    else namespaceCount--;
                    @namespace.NameIndex = validNameIndex;
                }
                #endregion
                #region Class Renaming
                foreach (ASClass @class in abc.Classes)
                {
                    var validName = string.Empty;
                    ASInstance instance = @class.Instance;
                    if (instance.IsInterface)
                    {
                        validName = ("IInterface_" + (++interfaceCount).ToString("0000"));
                    }
                    else
                    {
                        validName = ("Class_" + (++classCount).ToString("0000"));
                    }

                    ASMultiname qname = instance.QName;
                    if (IsValidIdentifier(qname.Name)) continue;

                    int validNameIndex = -1;
                    string key = ($"{qname.Namespace.Name}.{qname.Name}");
                    if (!nameIndices.TryGetValue(key, out validNameIndex))
                    {
                        validNameIndex = abc.Pool.AddConstant(validName, false);

                        nameIndices.Add(key, validNameIndex);
                        if (!validClasses.ContainsKey(key))
                        {
                            validClasses.Add(key, validName);
                        }
                    }
                    else if (instance.IsInterface)
                    {
                        interfaceCount--;
                    }
                    else
                    {
                        classCount--;
                    }
                    qname.NameIndex = validNameIndex;
                }
                #endregion
                #region Multiname Renaming
                foreach (ASMultiname multiname in abc.Pool.Multinames)
                {
                    if (string.IsNullOrWhiteSpace(multiname?.Name)) continue;
                    if (IsValidIdentifier(multiname.Name)) continue;

                    var validClassKeys = new List<string>();
                    switch (multiname.Kind)
                    {
                        default: continue;
                        case MultinameKind.QName:
                        {
                            validClassKeys.Add($"{multiname.Namespace.Name}.{multiname.Name}");
                            break;
                        }
                        case MultinameKind.Multiname:
                        {
                            foreach (ASNamespace @namespace in multiname.NamespaceSet.GetNamespaces())
                            {
                                validClassKeys.Add($"{@namespace.Name}.{multiname.Name}");
                            }
                            break;
                        }
                    }
                    foreach (string key in validClassKeys)
                    {
                        int validNameIndex = -1;
                        if (nameIndices.TryGetValue(key, out validNameIndex))
                        {
                            multiname.NameIndex = validNameIndex;
                        }
                    }
                }
                #endregion
            }
            #region Symbol Renaming
            foreach (SymbolClassTag symbolTag in Tags
                .Where(t => t.Kind == TagKind.SymbolClass)
                .Cast<SymbolClassTag>())
            {
                for (int i = 0; i < symbolTag.Names.Count; i++)
                {
                    string fullName = symbolTag.Names[i];

                    string className = fullName;
                    var namespaceName = string.Empty;
                    string[] names = fullName.Split('.');
                    if (names.Length == 2)
                    {
                        className = names[1];
                        namespaceName = names[0];

                        if (IsValidIdentifier(namespaceName) &&
                            IsValidIdentifier(className))
                        {
                            continue;
                        }
                    }

                    var fixedFullName = string.Empty;
                    var fixedNamespaceName = string.Empty;
                    if (validNamespaces.TryGetValue(namespaceName, out fixedNamespaceName))
                    {
                        fixedFullName += (fixedNamespaceName + ".");
                    }
                    else if (!string.IsNullOrWhiteSpace(namespaceName))
                    {
                        fixedFullName += (namespaceName + ".");
                    }

                    var fixedClassName = string.Empty;
                    if (validClasses.TryGetValue($"{fixedNamespaceName}.{className}", out fixedClassName))
                    {
                        fixedFullName += fixedClassName;
                    }
                    else fixedFullName += className;
                    symbolTag.Names[i] = fixedFullName;
                }
            }
            #endregion
        }
        #endregion

        public void GenerateMessageHashes()
        {
            FindMessagesReferences();
            foreach (MessageItem message in OutMessages.Values.Concat(InMessages.Values))
            {
                List<MessageItem> group = null;
                if (!Messages.TryGetValue(message.GenerateHash(), out group))
                {
                    group = new List<MessageItem>();
                    Messages.Add(message.Hash, group);
                }
                group.Add(message);
            }
        }
        #region Message Reference Searching
        private void FindMessagesReferences()
        {
            int classRank = 1;
            ABCFile abc = ABCFiles.Last();
            foreach (ASClass @class in abc.Classes)
            {
                ASInstance instance = @class.Instance;
                if (_messages.ContainsKey(@class)) continue;
                if (instance.Flags.HasFlag(ClassFlags.Interface)) continue;

                IEnumerable<ASMethod> methods = (new[] { @class.Constructor, instance.Constructor })
                    .Concat(instance.GetMethods())
                    .Concat(@class.GetMethods());

                int methodRank = 0;
                foreach (ASMethod fromMethod in methods)
                {
                    bool isStatic = (fromMethod.Trait?.IsStatic ?? @class.Constructor == fromMethod);
                    var fromContainer = (isStatic ? (ASContainer)@class : instance);

                    List<MessageReference> refernces = FindMessageReferences(@class, fromContainer, fromMethod);
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
            foreach (MessageItem incomingMsg in InMessages.Values)
            {
                foreach (MessageReference reference in incomingMsg.References)
                {
                    List<MessageReference> references = null;
                    if (!froms.TryGetValue(reference.FromMethod.Container, out references))
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
                List<MessageReference> references = null;
                if (froms.TryGetValue(@class, out references))
                {
                    container = @class;
                }
                else if (froms.TryGetValue(@class.Instance, out references))
                {
                    container = @class.Instance;
                }
                if (container != null)
                {
                    var methodReferenceGroups = new Dictionary<ASMethod, List<MessageReference>>();
                    foreach (MessageReference reference in references)
                    {
                        reference.FromClass = @class;
                        reference.InstructionRank = -1;
                        reference.ClassRank = classRank;
                        reference.IsStatic = container.IsStatic;
                        reference.GroupCount = references.Count;

                        List<MessageReference> methodReferences = null;
                        if (!methodReferenceGroups.TryGetValue(reference.FromMethod, out methodReferences))
                        {
                            methodReferences = new List<MessageReference>();
                            methodReferenceGroups.Add(reference.FromMethod, methodReferences);
                        }
                        methodReferences.Add(reference);
                    }

                    int methodRank = 1;
                    foreach (ASMethod method in container.GetMethods())
                    {
                        List<MessageReference> methodReferences = null;
                        if (methodReferenceGroups.TryGetValue(method, out methodReferences))
                        {
                            foreach (MessageReference reference in methodReferences)
                            {
                                reference.MethodRank = methodRank;
                            }
                            methodRank++;
                        }
                    }
                    classRank++;
                }
            }
        }
        private List<MessageReference> FindMessageReferences(ASClass fromClass, ASContainer fromContainer, ASMethod fromMethod)
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
                        references.AddRange(FindMessageReferences(fromClass, fromContainer, newFunction.Method));
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

                MessageItem message = null;
                if (!_messages.TryGetValue(messageClass, out message)) continue;
                if (message.HasMethodReference(fromMethod)) continue;

                var reference = new MessageReference();
                message.References.Add(reference);

                if (message.IsOutgoing)
                {
                    reference.FromClass = fromClass;
                    reference.FromMethod = fromMethod;
                    reference.InstructionRank = ++instructionRank;
                    reference.IsAnonymous = (!fromMethod.IsConstructor && fromMethod.Trait == null);

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

        public bool DisableHandshake()
        {
            if (!DisableEncryption()) return false;
            ABCFile abc = ABCFiles.Last();

            ASInstance habboCommDemoInstance = abc.GetInstance("HabboCommunicationDemo");
            if (habboCommDemoInstance == null) return false;

            int firstCoerceIndex = 0;
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

                    firstCoerceIndex = initCryptoCode.IndexOf(OPCode.Coerce);
                    asInterfaceQNameIndex = ((CoerceIns)initCryptoCode[firstCoerceIndex]).TypeNameIndex;
                }
                else if (parameter.TypeIndex == asInterfaceQNameIndex)
                {
                    int beforeExitIndex = (initCryptoCode.Count - 6);
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
            ASMethod localHostCheckMethod = ABCFiles[0].Classes[0].GetMethod(null, "Boolean", 1);
            if (localHostCheckMethod == null) return false;

            ASInstance habboInstance = ABCFiles[1].GetInstance("Habbo");
            if (habboInstance == null) return false;

            ASMethod remoteHostCheckMethod = habboInstance.GetMethod(null, "Boolean", new[] { "String", "Object" });
            if (remoteHostCheckMethod == null) return false;

            localHostCheckMethod.Body.Code[0] = remoteHostCheckMethod.Body.Code[0] = (byte)OPCode.PushTrue;
            localHostCheckMethod.Body.Code[1] = remoteHostCheckMethod.Body.Code[1] = (byte)OPCode.ReturnValue;
            return LockInfoHostProperty();
        }
        public bool EnableGameCenterIcon()
        {
            ABCFile abc = ABCFiles.Last();

            ASInstance bottomBarLeft = abc.GetInstance("BottomBarLeft");
            if (bottomBarLeft == null) return false;

            ASMethod visibleButtonsMethod = bottomBarLeft.GetMethod(null, "void", new[] { "String" });
            if (visibleButtonsMethod == null) return false;

            ASCode visibleButtonsCode = visibleButtonsMethod.Body.ParseCode();
            for (int i = 0; i < visibleButtonsCode.Count; i++)
            {
                ASInstruction instruction = visibleButtonsCode[i];
                if (instruction.OP != OPCode.GetLocal) continue;

                var getLocal1 = (GetLocalIns)instruction;
                if (getLocal1.Register != 4) continue;

                if (visibleButtonsCode[i + 1].OP != OPCode.GetProperty) continue;
                var getProperty1 = (GetPropertyIns)visibleButtonsCode[i + 1];

                if (getProperty1.PropertyName.Name != "name") continue;
                var pushString1 = (PushStringIns)visibleButtonsCode[i + 2];

                if (pushString1.Value != "GAMES") continue;

                ASTrait habboToolbarPropertySlot = bottomBarLeft.GetSlotTraits("HabboToolbar").FirstOrDefault();
                if (habboToolbarPropertySlot == null) return false;

                visibleButtonsCode.RemoveRange(i + 5, 3);
                visibleButtonsCode.InsertRange(i + 5, new ASInstruction[]
                {
                    // _local_4.visible = getBoolean("game.center.enabled");
                    new GetLocalIns(4),
                    new GetLocal0Ins(),
                    new GetPropertyIns(abc, habboToolbarPropertySlot.QNameIndex),
                    new PushStringIns(abc, "game.center.enabled"),
                    new CallPropertyIns(abc, abc.Pool.GetMultinameIndex("getBoolean"), 1),
                    new SetPropertyIns(abc, abc.Pool.GetMultinameIndex("visible"))
                });
                break;
            }

            visibleButtonsMethod.Body.Code = visibleButtonsCode.ToArray();
            return true;
        }

        public bool InjectRawCamera()
        {
            // TODO: Try to split this up.
            ABCFile abc = ABCFiles.Last();
            ASInstance renderRoomInstance = abc.GetInstance("RenderRoomMessageComposer");
            if (renderRoomInstance == null) return false;

            ASMethod isLessThan8100Method = renderRoomInstance.GetMethod(null, "Boolean", 0);
            if (isLessThan8100Method == null) return false;
            isLessThan8100Method.Body.Code[0] = (byte)OPCode.PushTrue;
            isLessThan8100Method.Body.Code[1] = (byte)OPCode.ReturnValue;

            ASMethod photoStringifierMethod = renderRoomInstance.GetMethod(null, "void", 0);
            if (photoStringifierMethod == null) return false;
            photoStringifierMethod.Body.Code[0] = (byte)OPCode.ReturnVoid;

            var assignDataMethod = new ASMethod(abc);
            assignDataMethod.ReturnTypeIndex = 2;
            int assignDataMethodIndex = abc.AddMethod(assignDataMethod);

            var bitmapParam = new ASParameter(abc.Pool, assignDataMethod);
            bitmapParam.TypeIndex = abc.Pool.GetMultinameIndex("BitmapData");
            assignDataMethod.Parameters.Add(bitmapParam);

            var assignDataBody = new ASMethodBody(abc);
            assignDataBody.MethodIndex = assignDataMethodIndex;
            assignDataBody.InitialScopeDepth = 4;
            assignDataBody.Code = new byte[0];
            assignDataBody.MaxScopeDepth = 5;
            assignDataBody.LocalCount = 3;
            assignDataBody.MaxStack = 4;
            abc.AddMethodBody(assignDataBody);

            ASTrait valuesArraySlot = renderRoomInstance.GetSlotTraits("Array").Last();
            var assignDataCode = new ASCode(abc, assignDataBody);
            assignDataCode.AddRange(new ASInstruction[]
            {
                new GetLocal0Ins(),
                new PushScopeIns(),

                new GetLexIns(abc, abc.Pool.GetMultinameIndex("PNGEncoder")),
                new GetLocal1Ins(),
                new CallPropertyIns(abc, abc.Pool.GetMultinameIndex("encode"), 1),
                new CoerceIns(abc, abc.Pool.GetMultinameIndex("ByteArray")),
                new SetLocal2Ins(),

                new GetLocal0Ins(),
                new GetLocal2Ins(),
                new NewArrayIns(1),
                new InitPropertyIns(abc, valuesArraySlot.QNameIndex),

                new ReturnVoidIns()
            });
            assignDataBody.Code = assignDataCode.ToArray();
            renderRoomInstance.AddMethod(assignDataMethod, "assignBitmap");

            ASInstance smallCameraInstance = abc.GetInstance("RoomThumbnailCameraWidget");
            if (smallCameraInstance == null) return false;

            ASTrait smallBitmapWindowSlot = smallCameraInstance.GetSlotTraits("IBitmapWrapperWindow").FirstOrDefault();
            if (smallBitmapWindowSlot == null) return false;

            ASMethod smallCaptureMethod = smallCameraInstance.GetMethod(null, "void", new string[] { "WindowEvent", "IWindow" });
            if (smallCaptureMethod == null) return false;

            ASCode captureClickEventCode = smallCaptureMethod.Body.ParseCode();
            for (int i = 0; i < captureClickEventCode.Count; i++)
            {
                ASInstruction instruction = captureClickEventCode[i];
                if (instruction.OP != OPCode.GetLocal) continue;

                var getLocal = (GetLocalIns)instruction;
                if (getLocal.Register != 4) continue;

                instruction = captureClickEventCode[i - 1];
                if (instruction.OP != OPCode.GetProperty) continue;

                var getProperty = (GetPropertyIns)instruction;
                if (getProperty.PropertyName.Name != "handler") continue;

                captureClickEventCode.InsertRange(i - 2, new ASInstruction[]
                {
                    // local4.assignBitmap(this.smallBitmapWindowSlot.bitmap);
                    new GetLocalIns(4),
                    new GetLocal0Ins(),
                    new GetPropertyIns(abc, smallBitmapWindowSlot.QNameIndex),
                    new GetPropertyIns(abc, abc.Pool.GetMultinameIndex("bitmap")),
                    new CallPropVoidIns(abc, abc.Pool.GetMultinameIndex("assignBitmap"), 1)
                });
                break;
            }
            smallCaptureMethod.Body.Code = captureClickEventCode.ToArray();

            ASInstance photoLabInstance = abc.GetInstance("CameraPhotoLab");
            if (photoLabInstance == null) return false;

            ASTrait bigBitmapWindowSlot = photoLabInstance.GetSlotTraits("IBitmapWrapperWindow").FirstOrDefault();
            if (bigBitmapWindowSlot == null) return false;

            ASMethod bigPurchaseMethod = photoLabInstance.GetMethods(null, "void", 1)
                .First(m => m.Parameters[0].Type.Name == "MouseEvent");

            string dataSendTraitName = null;
            int cameraHandlerSlotNameIndex = -1;
            ASCode bigPurchaseCode = bigPurchaseMethod.Body.ParseCode();
            for (int i = 0; i < bigPurchaseCode.Count; i++)
            {
                ASInstruction instruction = bigPurchaseCode[i];
                if (instruction.OP != OPCode.ConstructProp) continue;

                var constructProp = (ConstructPropIns)instruction;
                if (constructProp.ArgCount != 2) continue;

                var getProperty = (bigPurchaseCode[i + 4] as GetPropertyIns);
                if (getProperty == null) return false;
                cameraHandlerSlotNameIndex = getProperty.PropertyNameIndex;

                bigPurchaseCode.InsertRange(i + 5, new ASInstruction[]
                {
                    // this.bigBitmapWindowSlot.bitmap
                    new GetLocal0Ins(),
                    new GetPropertyIns(abc, bigBitmapWindowSlot.QNameIndex),
                    new GetPropertyIns(abc, abc.Pool.GetMultinameIndex("bitmap"))
                });

                var callProperty = (bigPurchaseCode[i + 8] as CallPropertyIns);
                if (callProperty == null) return false;
                dataSendTraitName = callProperty.PropertyName.Name;
                callProperty.ArgCount = 1;
                break;
            }
            bigPurchaseMethod.Body.Code = bigPurchaseCode.ToArray();

            ASInstance bigCameraHandlerInstance = photoLabInstance.GetTraits(TraitKind.Slot)
                .Select(t => abc.GetInstance(t.Type))
                .FirstOrDefault();
            if (bigCameraHandlerInstance == null) return false;

            ASMethod dataSendMethod = bigCameraHandlerInstance.GetMethod(dataSendTraitName, "Boolean", 0);
            if (dataSendMethod == null) return false;

            var bitmapDataParam = new ASParameter(abc.Pool, dataSendMethod);
            bitmapDataParam.TypeIndex = abc.Pool.GetMultinameIndex("BitmapData");
            dataSendMethod.Parameters.Add(bitmapDataParam);

            ASCode dataSendCode = dataSendMethod.Body.ParseCode();
            ShiftRegistersBy(dataSendCode, 1);

            for (int i = 0; i < dataSendCode.Count; i++)
            {
                ASInstruction instruction = dataSendCode[i];
                if (instruction.OP != OPCode.SetLocal_2) continue;

                dataSendCode.InsertRange(i + 1, new ASInstruction[]
                {
                    new GetLocal2Ins(),
                    new GetLocal1Ins(),
                    new CallPropVoidIns(abc, abc.Pool.GetMultinameIndex("assignBitmap"), 1)
                });
                break;
            }

            dataSendMethod.Body.MaxStack += 1;
            dataSendMethod.Body.LocalCount += 1;
            dataSendMethod.Body.Code = dataSendCode.ToArray();
            return true;
        }
        public bool InjectKeyShouter(int messageId)
        {
            ABCFile abc = ABCFiles.Last();
            ASInstance socketConnInstance = abc.GetInstance("SocketConnection");
            if (socketConnInstance == null) return false;

            ASTrait sendFunction = InjectUniversalSendFunction(true);
            if (sendFunction == null) return false;

            ASInstance habboCommDemoInstance = abc.GetInstance("HabboCommunicationDemo");
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
        public bool InjectEndPointShouter(int messageId)
        {
            ABCFile abc = ABCFiles.Last();
            ASInstance socketConnInstance = abc.GetInstance("SocketConnection");
            if (socketConnInstance == null) return false;

            ASTrait sendFunction = InjectUniversalSendFunction(true);
            if (sendFunction == null) return false;

            ASInstance habboCommDemoInstance = abc.GetInstance("HabboCommunicationDemo");
            if (habboCommDemoInstance == null) return false;

            if (!InjectEndPointSaver(out ASTrait hostTrait, out ASTrait portTrait)) return false;
            foreach (ASMethod method in habboCommDemoInstance.GetMethods(null, "void", 1))
            {
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
            ABCFile abc = ABCFiles.Last();

            ASInstance socketConnInstance = abc.GetInstance("SocketConnection");
            if (socketConnInstance == null) return false;

            ASMethod initMethod = socketConnInstance.GetMethod("init", "Boolean", 2);
            if (initMethod == null) return false;

            ASTrait remoteHostTrait, remotePortTrait;
            if (!InjectEndPointSaver(out remoteHostTrait, out remotePortTrait)) return false;

            ASCode code = initMethod.Body.ParseCode();
            for (int i = 0; i < code.Count; i++)
            {
                ASInstruction instruction = code[i];
                if (instruction.OP != OPCode.CallPropVoid) continue;

                var callPropVoid = (CallPropVoidIns)instruction;
                if (callPropVoid.PropertyName.Name == "connect" && callPropVoid.ArgCount == 2)
                {
                    code[i - 2] = new PushStringIns(abc, host);
                    code[i - 1] = new PushIntIns(abc, port);
                    break;
                }
            }
            initMethod.Body.Code = code.ToArray();
            return LockEndPointPing(host, port);
        }
        public bool InjectDebugLogger(string functionName)
        {
            ABCFile abc = ABCFiles[1];

            ASMethod logMethod = null;
            foreach (ASClass @class in abc.Classes)
            {
                if (@class.Traits.Count != 2) continue;

                logMethod = @class.GetMethod("log", "void", 0);
                if (logMethod != null) break;
            }
            if (logMethod == null) return false;

            ASCode code = logMethod.Body.ParseCode();
            int startIndex = (code.IndexOf(OPCode.PushScope) + 1);

            var ifNotAvailable = new IfFalseIns() { Offset = 1 };
            ASInstruction jumpExit = code[code.IndexOf(OPCode.ReturnVoid)];

            code.InsertRange(startIndex, new ASInstruction[]
            {
                /*
                 * if(ExternalInterface.available)
                 * {
                 *     ExternalInterface.call({functionName});
                 * }
                 */
                new GetLexIns(abc, abc.Pool.GetMultinameIndex("ExternalInterface")),
                new GetPropertyIns(abc, abc.Pool.GetMultinameIndex("available")),
                ifNotAvailable,
                new GetLexIns(abc, abc.Pool.GetMultinameIndex("ExternalInterface")),
                new PushStringIns(abc, functionName),
                new GetLocal1Ins(),
                new CallPropVoidIns(abc, abc.Pool.GetMultinameIndex("call"), 2)
            });

            code.JumpExits[ifNotAvailable] = jumpExit;
            logMethod.Body.Code = code.ToArray();
            logMethod.Body.MaxStack += 3;
            return true;
        }
        public bool InjectMessageLogger(string functionName)
        {
            ABCFile abc = ABCFiles.Last();
            ASInstance decoderInstance = null;
            foreach (ASInstance instance in abc.Instances)
            {
                if (instance.IsInterface) continue;
                if (instance.Traits.Count != 12) continue;
                if (instance.Constructor.Parameters.Count != 2) continue;
                if (instance.Constructor.Parameters[0].Type.Name != "int") continue;
                if (instance.Constructor.Parameters[1].Type.Name != "ByteArray") continue;

                decoderInstance = instance;
                break;
            }

            var structureQName = new ASMultiname(abc.Pool);
            structureQName.NameIndex = abc.Pool.AddConstant("values");
            structureQName.Kind = MultinameKind.QName;
            structureQName.NamespaceIndex = 1; // Public

            var valuesSlot = new ASTrait(abc);
            valuesSlot.Kind = TraitKind.Slot;
            valuesSlot.QNameIndex = abc.Pool.AddConstant(structureQName);
            valuesSlot.TypeIndex = abc.Pool.GetMultinameIndex("Array");
            decoderInstance.Traits.Add(valuesSlot);

            ASCode ctorCode = decoderInstance.Constructor.Body.ParseCode();
            ctorCode.InsertRange(ctorCode.Count - 2, new ASInstruction[]
            {
                new GetLocal0Ins(),
                new GetLocal1Ins(),
                new NewArrayIns(1),
                new SetPropertyIns(abc, valuesSlot.QNameIndex)
            });
            decoderInstance.Constructor.Body.Code = ctorCode.ToArray();

            var addValueMethod = new ASMethod(abc);
            addValueMethod.ReturnTypeIndex = 2; // void
            int addTypeMethodIndex = abc.AddMethod(addValueMethod);

            var valueParam = new ASParameter(abc.Pool, addValueMethod);
            valueParam.NameIndex = abc.Pool.AddConstant("value");
            valueParam.TypeIndex = abc.Pool.GetMultinameIndex("Object");
            addValueMethod.Parameters.Add(valueParam);

            var addValueBody = new ASMethodBody(abc);
            addValueBody.MethodIndex = addTypeMethodIndex;
            addValueBody.InitialScopeDepth = 5;
            addValueBody.Code = new byte[0];
            addValueBody.MaxScopeDepth = 6;
            addValueBody.LocalCount = 2;
            addValueBody.MaxStack = 3;
            abc.AddMethodBody(addValueBody);

            var addTypeCode = new ASCode(abc, addValueBody);
            addTypeCode.AddRange(new ASInstruction[]
            {
                new GetLocal0Ins(),
                new PushScopeIns(),

                new GetLocal0Ins(),
                new GetPropertyIns(abc, valuesSlot.QNameIndex),
                new GetLocal1Ins(),
                new CallPropVoidIns(abc, abc.Pool.GetMultinameIndex("push"), 1),

                new ReturnVoidIns()
            });
            addValueBody.Code = addTypeCode.ToArray();

            decoderInstance.AddMethod(addValueMethod, "addValue");
            return true;
        }
        public bool InjectRSAKeys(string exponent, string modulus)
        {
            ABCFile abc = ABCFiles.Last();
            ASClass keyObfuscatorClass = abc.GetClass("KeyObfuscator");
            if (keyObfuscatorClass == null) return false;

            int modifyCount = 0;
            foreach (ASMethod method in keyObfuscatorClass.GetMethods(null, "String", 0))
            {
                int keyIndex = 0;
                switch (method.Trait.Id)
                {
                    // Get Modulus Method
                    case 6:
                    {
                        modifyCount++;
                        keyIndex = abc.Pool.AddConstant(modulus);
                        break;
                    }
                    // Get Exponent Method
                    case 7:
                    {
                        modifyCount++;
                        keyIndex = abc.Pool.AddConstant(exponent);
                        break;
                    }

                    // This is not a method we want to modify, continue enumerating.
                    default: continue;
                }

                ASCode code = method.Body.ParseCode();
                code.InsertRange(0, new ASInstruction[]
                {
                    new PushStringIns(abc, keyIndex),
                    new ReturnValueIns()
                });
                method.Body.Code = code.ToArray();
            }
            return (modifyCount == 2);
        }

        public ushort[] GetMessageIds(string hash)
        {
            List<MessageItem> messages = null;
            if (Messages.TryGetValue(hash, out messages))
            {
                return messages.Select(m => m.Id).ToArray();
            }
            return null;
        }
        [Obsolete]
        public IReadOnlyList<ushort> GetMessageHeaders(string hash) => GetMessageIds(hash).ToList().AsReadOnly();

        private void LoadMessages()
        {
            ABCFile abc = ABCFiles.Last();
            ASClass habboMessagesClass = abc.GetClass("HabboMessages");
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
                if (habboMessagesClass == null) return;
            }

            ASCode code = habboMessagesClass.Constructor.Body.ParseCode();
            int inMapTypeIndex = habboMessagesClass.Traits[0].QNameIndex;
            int outMapTypeIndex = habboMessagesClass.Traits[1].QNameIndex;

            ASInstruction[] instructions = code
                .Where(i => i.OP == OPCode.GetLex ||
                            i.OP == OPCode.PushShort ||
                            i.OP == OPCode.PushByte)
                .ToArray();

            for (int i = 0; i < instructions.Length; i += 3)
            {
                var getLexInst = (instructions[i + 0] as GetLexIns);
                bool isOutgoing = (getLexInst.TypeNameIndex == outMapTypeIndex);

                var primitive = (instructions[i + 1] as Primitive);
                ushort id = Convert.ToUInt16(primitive.Value);

                getLexInst = (instructions[i + 2] as GetLexIns);
                ASClass messageClass = abc.GetClass(getLexInst.TypeName);

                var message = new MessageItem(messageClass, isOutgoing, id);
                (isOutgoing ? OutMessages : InMessages).Add(id, message);

                if (_messages.ContainsKey(messageClass))
                {
                    _messages[messageClass].SharedIds.Add(id);
                }
                else _messages.Add(messageClass, message);

                if (id == 4000 && isOutgoing)
                {
                    ASInstance messageInstance = messageClass.Instance;
                    ASMethod toArrayMethod = messageInstance.GetMethod(null, "Array", 0);

                    ASCode toArrayCode = toArrayMethod.Body.ParseCode();
                    int index = toArrayCode.IndexOf(OPCode.PushString);

                    if (index != -1)
                    {
                        var pushStringIns = (PushStringIns)toArrayCode[index];
                        _revisionIndex = pushStringIns.ValueIndex;
                    }
                }
            }
        }
        private void SimplifySendCode(ABCFile abc, ASCode sendCode)
        {
            bool isTrimming = true;
            for (int i = 0; i < sendCode.Count; i++)
            {
                ASInstruction instruction = sendCode[i];
                if (!isTrimming && Local.IsValid(instruction.OP))
                {
                    var local = (Local)instruction;
                    int newRegister = (local.Register - 1);
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
                    if (instruction.OP != OPCode.CallProperty) continue;

                    var callProperty = (CallPropertyIns)instruction;
                    if (callProperty.PropertyName.Name != "encode") continue;

                    sendCode.RemoveRange(0, i - 4);
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
            ABCFile abc = ABCFiles.Last();
            if (disableCrypto && !DisableEncryption()) return null;

            ASInstance socketConnInstance = abc.GetInstance("SocketConnection");
            if (socketConnInstance == null) return null;

            ASMethod sendMethod = socketConnInstance.GetMethod("send", "Boolean", 1);
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
            var idParam = new ASParameter(abc.Pool, sendMessageMethod);
            idParam.NameIndex = abc.Pool.AddConstant("id");
            idParam.TypeIndex = abc.Pool.GetMultinameIndex("int");
            sendMessageMethod.Parameters.Add(idParam);

            // The method body that houses the instructions.
            var sendMessageBody = new ASMethodBody(abc);
            sendMessageBody.MethodIndex = sendMessageMethodIndex;
            sendMessageBody.Code = sendCode.ToArray();
            sendMessageBody.InitialScopeDepth = 5;
            sendMessageBody.MaxScopeDepth = 6;
            sendMessageBody.LocalCount = 10;
            sendMessageBody.MaxStack = 5;
            abc.AddMethodBody(sendMessageBody);

            socketConnInstance.AddMethod(sendMessageMethod, "sendMessage");
            return sendMessageMethod.Trait;
        }
        private ASTrait AddPublicSlot(ASContainer container, string name, int typeIndex)
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
        private ASTrait AddPublicSlot(ASContainer container, string name, string typeName)
        {
            return AddPublicSlot(container, name, container.GetABC().Pool.GetMultinameIndex(typeName));
        }

        private bool DisableEncryption()
        {
            ABCFile abc = ABCFiles.Last();

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
        private bool LockInfoHostProperty()
        {
            ABCFile abc = ABCFiles.Last();

            ASMethod connectMethod = GetManagerConnectMethod();
            if (connectMethod == null) return false;

            ASCode connectCode = connectMethod.Body.ParseCode();
            int pushByteIndex = connectCode.IndexOf(OPCode.PushByte);

            ASInstance habboCommunicationManager = abc.GetInstance("HabboCommunicationManager");
            if (habboCommunicationManager == null) return false;

            ASTrait infoHostSlot = habboCommunicationManager.GetSlotTraits("String").FirstOrDefault();
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
        private ASMethod GetManagerConnectMethod()
        {
            if (_managerConnectMethod != null) return _managerConnectMethod;
            ABCFile abc = ABCFiles.Last();

            ASInstance habboCommunicationManager = abc.GetInstance("HabboCommunicationManager");
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
            return (_managerConnectMethod = habboCommunicationManager.GetMethod(connectMethodName, "void", 0));
        }
        private bool LockEndPointPing(string host, int port)
        {
            ABCFile abc = ABCFiles.Last();
            if (!LockInfoHostProperty()) return false;

            ASMethod connectMethod = GetManagerConnectMethod();
            if (connectMethod == null) return false;

            ASCode connectCode = connectMethod.Body.ParseCode();
            for (int i = 0, findPropStrictCount = 0; i < connectCode.Count; i++)
            {
                ASInstruction instruction = connectCode[i];
                if (instruction.OP != OPCode.FindPropStrict || ++findPropStrictCount != 3) continue;

                i++;
                var thirdFindPropStrictIns = (FindPropStrictIns)instruction;
                for (int j = 0; i < connectCode.Count; i++, j++)
                {
                    instruction = connectCode[i];
                    if (instruction.OP != OPCode.ConstructProp) continue;
                    if (((ConstructPropIns)instruction).PropertyNameIndex != thirdFindPropStrictIns.PropertyNameIndex) continue;

                    connectCode.RemoveRange(i - j, j);
                    connectCode.InsertRange(i - j, new ASInstruction[]
                    {
                        new PushStringIns(abc, host),
                        new PushIntIns(abc, port)
                    });

                    connectMethod.Body.Code = connectCode.ToArray();
                    return true;
                }
            }
            return false;
        }
        private void ShiftRegistersBy(ASCode code, int offset)
        {
            for (int i = 0; i < code.Count; i++)
            {
                ASInstruction instruction = code[i];
                if (!Local.IsValid(instruction.OP)) continue;
                if (instruction.OP == OPCode.GetLocal_0) continue;

                var local = (Local)instruction;
                if (Local.IsGetLocal(instruction.OP))
                {
                    code[i] = Local.CreateGet(local.Register + offset);
                }
                else if (Local.IsSetLocal(instruction.OP))
                {
                    code[i] = Local.CreateSet(local.Register + offset);
                }
            }
        }
        private bool InjectEndPointSaver(out ASTrait hostTrait, out ASTrait portTrait)
        {
            hostTrait = portTrait = null;
            ABCFile abc = ABCFiles.Last();

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
                            code[i - 2].Clone(),
                            new SetPropertyIns(abc, hostTrait.QNameIndex),

                            new GetLocal0Ins(),
                            code[i - 1].Clone(),
                            new SetPropertyIns(abc, portTrait.QNameIndex)
                        });
                        init.Body.Code = code.ToArray();
                        return true;
                    }
                }
            }
            return false;
        }

        public override void Disassemble(Action<TagItem> callback)
        {
            base.Disassemble(callback);
            LoadMessages();
        }
        protected override void WriteTag(TagItem tag, FlashWriter output)
        {
            if (tag.Kind == TagKind.DoABC)
            {
                var doABCTag = (DoABCTag)tag;
                doABCTag.ABCData = _abcFileTags[doABCTag].ToArray();
            }
            base.WriteTag(tag, output);
        }
        protected override TagItem ReadTag(HeaderRecord header, FlashReader input)
        {
            TagItem tag = base.ReadTag(header, input);
            if (tag.Kind == TagKind.DoABC)
            {
                var doABCTag = (DoABCTag)tag;
                var abcFile = new ABCFile(doABCTag.ABCData);

                _abcFileTags[doABCTag] = abcFile;
                ABCFiles.Add(abcFile);
            }
            return tag;
        }

        public static bool IsValidIdentifier(string value, bool invalidOnSanitized = false)
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

            return (!value.Contains("_-") &&
                !_reservedNames.Contains(value.Trim()));
        }
    }

    public class HashWriter : BinaryWriter
    {
        private readonly SortedDictionary<int, int> _ints;
        private readonly SortedDictionary<bool, int> _bools;
        private readonly SortedDictionary<byte, int> _bytes;
        private readonly SortedDictionary<string, int> _strings;

        public bool IsSorting { get; set; }

        public HashWriter(bool isSorting)
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
                if (!string.IsNullOrWhiteSpace(parameter.Name) &&
                    HGame.IsValidIdentifier(parameter.Name, true))
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
            else if (multiname == null ||
                HGame.IsValidIdentifier(multiname.Name, true))
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

        public override void Flush()
        {
            WriteSorted(_ints, base.Write);
            WriteSorted(_bools, base.Write);
            WriteSorted(_bytes, base.Write);
            WriteSorted(_strings, base.Write);
        }
        public string GenerateMD5Hash()
        {
            Flush();
            using (var md5 = MD5.Create())
            {
                long curPos = BaseStream.Position;
                BaseStream.Position = 0;

                byte[] hashData = md5.ComputeHash(BaseStream);
                string hashAsHex = (BitConverter.ToString(hashData));

                BaseStream.Position = curPos;
                return hashAsHex.Replace("-", string.Empty).ToLower();
            }
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
    public class MessageItem
    {
        public ushort Id { get; set; }
        public string Hash { get; set; }
        public bool IsOutgoing { get; set; }

        public ASClass Class { get; }
        public ASClass Parser { get; }
        public string[] Structure { get; }
        public List<ushort> SharedIds { get; }
        public List<MessageReference> References { get; }

        public MessageItem(ASClass messageClass, bool isOutgoing, ushort id)
        {
            Id = id;
            Class = messageClass;
            IsOutgoing = isOutgoing;

            SharedIds = new List<ushort>();
            References = new List<MessageReference>();

            if (!IsOutgoing)
            {
                Parser = GetMessageParser();
                if (Parser != null)
                {
                    Structure = GetIncomingStructure(Parser);
                }
            }
            else
            {
                Structure = GetOutgoingStructure(Class);
            }
        }

        public string GenerateHash()
        {
            if (!string.IsNullOrWhiteSpace(Hash))
            {
                return Hash;
            }
            using (var output = new HashWriter(false))
            {
                output.Write(IsOutgoing);
                if (!HGame.IsValidIdentifier(Class.QName.Name, true))
                {
                    output.Write(Class.Instance, true);
                    output.Write(Class.Instance.Constructor);

                    output.Write(References.Count);
                    foreach (MessageReference reference in References)
                    {
                        output.Write(reference.IsStatic);
                        output.Write(reference.IsAnonymous);

                        output.Write(reference.MethodRank);
                        output.Write(reference.InstructionRank);

                        output.Write(reference.FromMethod);

                        output.Write(reference.FromClass.Constructor);
                        output.Write(reference.FromClass.Instance.Constructor);
                    }
                    if (!IsOutgoing && Parser != null)
                    {
                        output.Write(Parser.Instance, true);
                    }
                }
                else output.Write(Class.QName.Name);
                return (Hash = output.GenerateMD5Hash());
            }
        }
        public bool HasMethodReference(ASMethod method)
        {
            return References.Any(r => r.FromMethod == method);
        }

        public int GetMatchDeviation(MessageItem message)
        {
            if (Class.QName.Name == message.Class.QName.Name) return 0;

            int cClassRankTotal = 0;
            foreach (MessageReference reference in References)
            {
                cClassRankTotal += reference.ClassRank;
            }

            int pClassRankTotal = 0;
            foreach (MessageReference reference in message.References)
            {
                pClassRankTotal += reference.ClassRank;
            }

            return Math.Abs(cClassRankTotal - pClassRankTotal);
        }
        public MessageItem GetClosestMatch(IEnumerable<MessageItem> messages)
        {
            MessageItem closestMatch = null;
            int lowestDeviation = int.MaxValue;
            foreach (MessageItem message in messages)
            {
                int deviation = GetMatchDeviation(message);
                if (deviation == 0) return message;

                if (deviation < lowestDeviation)
                {
                    closestMatch = message;
                    lowestDeviation = deviation;
                }
            }
            return closestMatch;
        }

        private ASClass GetMessageParser()
        {
            ABCFile abc = Class.GetABC();
            ASInstance instance = Class.Instance;

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

        #region Structure Extraction
        private string[] GetIncomingStructure(ASClass @class)
        {
            ASMethod parseMethod = @class.Instance.GetMethod("parse", "Boolean", 1);
            return GetIncomingStructure(@class.Instance, parseMethod);
        }
        private string[] GetIncomingStructure(ASInstance instance, ASMethod method)
        {
            if (method.Body.Exceptions.Count > 0) return null;

            ASCode code = method.Body.ParseCode();
            if (code.JumpExits.Count > 0 || code.SwitchExits.Count > 0) return null;

            ABCFile abc = method.GetABC();
            var structure = new List<string>();
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

                            string[] innerStructure = GetIncomingStructure(innerInstance, innerMethod);
                            if (innerStructure != null)
                            {
                                structure.AddRange(innerStructure);
                            }
                            else return null;
                        }
                        else
                        {
                            structure.Add(GetReadReturnTypeName(callProperty.PropertyName));
                        }
                        break;
                    }

                    case OPCode.ConstructProp:
                    {
                        var constructProp = (ConstructPropIns)next;
                        ASInstance innerInstance = abc.GetInstance(constructProp.PropertyName);

                        string[] innerStructure = GetIncomingStructure(innerInstance, innerInstance.Constructor);
                        if (innerStructure != null)
                        {
                            structure.AddRange(innerStructure);
                        }
                        else return null;
                        break;
                    }

                    case OPCode.ConstructSuper:
                    {
                        var constructSuper = (ConstructSuperIns)next;
                        ASInstance superInstance = abc.GetInstance(instance.Super);

                        string[] innerStructure = GetIncomingStructure(superInstance, superInstance.Constructor);
                        if (innerStructure != null)
                        {
                            structure.AddRange(innerStructure);
                        }
                        else return null;
                        break;
                    }

                    case OPCode.CallSuper:
                    {
                        var callSuper = (CallSuperIns)next;
                        ASInstance superInstance = abc.GetInstance(instance.Super);

                        ASMethod superMethod = superInstance.GetMethod(callSuper.MethodName.Name, null, callSuper.ArgCount);
                        string[] innerStructure = GetIncomingStructure(superInstance, superMethod);
                        if (innerStructure != null)
                        {
                            structure.AddRange(innerStructure);
                        }
                        else return null;
                        break;
                    }

                    case OPCode.CallPropVoid:
                    {
                        var callPropVoid = (CallPropVoidIns)next;
                        if (callPropVoid.ArgCount == 0)
                        {
                            structure.Add(GetReadReturnTypeName(callPropVoid.PropertyName));
                        }
                        else return null;
                        break;
                    }

                    default: return null;
                }
            }
            return structure.ToArray();
        }

        private string[] GetOutgoingStructure(ASClass @class)
        {
            ASMethod getArrayMethod = @class.Instance.GetMethod(null, "Array", 0);
            if (getArrayMethod == null)
            {
                ASClass superClass = @class.GetABC().GetClass(@class.Instance.Super);
                return GetOutgoingStructure(superClass);
            }
            if (getArrayMethod.Body.Exceptions.Count > 0) return null;
            ASCode getArrayCode = getArrayMethod.Body.ParseCode();

            if (getArrayCode.JumpExits.Count > 0 ||
                getArrayCode.SwitchExits.Count > 0)
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
                return GetOutgoingStructure(getArrayCode, resultPusher, argCount);
            }
            else if (argCount == 0 ||
                resultPusher.OP == OPCode.PushNull)
            {
                return null;
            }

            if (resultPusher.OP == OPCode.GetProperty)
            {
                var getProperty = (GetPropertyIns)resultPusher;
                return GetOutgoingStructure(Class, getProperty.PropertyName);
            }
            else if (Local.IsGetLocal(resultPusher.OP))
            {
                return GetOutgoingStructure(getArrayCode, (Local)resultPusher);
            }
            return null;
        }
        private string[] GetOutgoingStructure(ASCode code, Local getLocal)
        {
            var structure = new List<string>();
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
                    if (previous.OP == OPCode.GetProperty)
                    {
                        ASClass classToCheck = Class;
                        var getProperty = (GetPropertyIns)previous;
                        ASMultiname propertyName = getProperty.PropertyName;

                        ASInstruction beforeGetProp = code[i - 2];
                        if (beforeGetProp.OP == OPCode.GetLex)
                        {
                            var getLex = (GetLexIns)beforeGetProp;
                            classToCheck = classToCheck.GetABC().GetClass(getLex.TypeName);
                        }

                        string propertyTypeName = null;
                        if (TryGetTraitTypeName(classToCheck, propertyName, out propertyTypeName) ||
                            TryGetTraitTypeName(classToCheck.Instance, propertyName, out propertyTypeName))
                        {
                            structure.Add(propertyTypeName);
                        }
                    }
                }
            }
            return structure.ToArray();
        }
        private string[] GetOutgoingStructure(ASClass @class, ASMultiname propertyName)
        {
            ASMethod constructor = @class.Instance.Constructor;
            if (constructor.Body.Exceptions.Count > 0) return null;
            ASCode code = constructor.Body.ParseCode();

            if (code.JumpExits.Count > 0 ||
                code.SwitchExits.Count > 0)
            {
                return null;
            }

            var structure = new List<string>();
            var pushedLocals = new Dictionary<int, int>();
            for (int i = 0; i < code.Count; i++)
            {
                ASInstruction next = null;
                ASInstruction instruction = code[i];
                if (instruction.OP == OPCode.NewArray)
                {
                    var newArray = (NewArrayIns)instruction;
                    if (newArray.ArgCount > 0)
                    {
                        var structArray = new string[newArray.ArgCount];
                        for (int j = i - 1, length = newArray.ArgCount; j >= 0; j--)
                        {
                            ASInstruction previous = code[j];
                            if (Local.IsGetLocal(previous.OP) &&
                                previous.OP != OPCode.GetLocal_0)
                            {
                                var local = (Local)previous;
                                ASParameter parameter = constructor.Parameters[local.Register - 1];
                                structArray[--length] = parameter.Type.Name;
                            }
                            if (length == 0)
                            {
                                structure.AddRange(structArray);
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
                        structure.AddRange(GetOutgoingStructure(superClass, propertyName));
                    }
                }
                if (instruction.OP != OPCode.GetProperty) continue;

                var getProperty = (GetPropertyIns)instruction;
                if (getProperty.PropertyName != propertyName) continue;

                next = code[++i];
                ASClass classToCheck = @class;
                if (Local.IsGetLocal(next.OP))
                {
                    if (next.OP == OPCode.GetLocal_0)
                    {
                        classToCheck = @class;
                        continue;
                    }

                    var local = (Local)next;
                    ASParameter parameter = constructor.Parameters[local.Register - 1];
                    structure.Add(parameter.Type.Name);
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

                    string propertyTypeName = null;
                    if (TryGetTraitTypeName(classToCheck, propertyName, out propertyTypeName) ||
                        TryGetTraitTypeName(classToCheck?.Instance, propertyName, out propertyTypeName))
                    {
                        structure.Add(propertyTypeName);
                    }
                }
            }
            if (structure.Contains("Array"))
            {
                // External array... impossible to check what value types are contained in this.
                return null;
            }
            return structure.ToArray();
        }
        private string[] GetOutgoingStructure(ASCode code, ASInstruction beforeReturn, int length)
        {
            var getLocalEndIndex = -1;
            int pushingEndIndex = code.IndexOf(beforeReturn);

            var structure = new string[length];
            ASInstance instance = Class.Instance;
            var pushedLocals = new Dictionary<int, int>();
            for (int i = pushingEndIndex - 1; i >= 0; i--)
            {
                ASInstruction instruction = code[i];
                if (instruction.OP == OPCode.GetProperty)
                {
                    ASClass classToCheck = Class;
                    var getProperty = (GetPropertyIns)instruction;
                    ASMultiname propertyName = getProperty.PropertyName;

                    ASInstruction previous = code[i - 1];
                    if (previous.OP == OPCode.GetLex)
                    {
                        var getLex = (GetLexIns)previous;
                        classToCheck = classToCheck.GetABC().GetClass(getLex.TypeName);
                    }

                    string propertyTypeName = null;
                    if (TryGetTraitTypeName(classToCheck, propertyName, out propertyTypeName) ||
                        TryGetTraitTypeName(classToCheck.Instance, propertyName, out propertyTypeName))
                    {
                        structure[--length] = propertyTypeName;
                    }
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
            for (int i = (getLocalEndIndex - 1); i >= 0; i--)
            {
                ASInstruction instruction = code[i];
                if (!Local.IsSetLocal(instruction.OP)) continue;

                int structIndex = -1;
                var local = (Local)instruction;
                if (pushedLocals.TryGetValue(local.Register, out structIndex))
                {
                    ASInstruction beforeSet = code[i - 1];
                    pushedLocals.Remove(local.Register);
                    switch (beforeSet.OP)
                    {
                        case OPCode.PushInt:
                        case OPCode.PushByte:
                        case OPCode.Convert_i:
                        structure[structIndex] = "int";
                        break;

                        case OPCode.Coerce_s:
                        case OPCode.PushString:
                        structure[structIndex] = "String";
                        break;

                        case OPCode.PushTrue:
                        case OPCode.PushFalse:
                        structure[structIndex] = "Boolean";
                        break;

                        default:
                        throw new Exception($"Don't know what this value type is, tell someone about this please.\r\nOP: {beforeSet.OP}");
                    }
                }
                if (pushedLocals.Count == 0) break;
            }
            return structure;
        }

        private string GetReadReturnTypeName(ASMultiname propertyName)
        {
            switch (propertyName.Name)
            {
                case "readString":
                return "String";

                case "readBoolean":
                return "Boolean";

                case "readByte":
                return "Byte";

                case "readDouble":
                return "Double";

                default:
                {
                    if (!HGame.IsValidIdentifier(propertyName.Name, true))
                    {
                        // Most likely: readInt
                        return "int";
                    }
                    return null;
                }
            }
        }
        private string GetStrictReturnTypeName(ASMultiname propertyName)
        {
            switch (propertyName.Name)
            {
                case "int":
                case "getTimer": return "int";
            }
            return null;
        }
        private bool TryGetTraitTypeName(ASContainer container, ASMultiname propertyName, out string propertyTypeName)
        {
            if (container == null)
            {
                propertyTypeName =
                    GetStrictReturnTypeName(propertyName);
            }
            else
            {
                ASTrait propertyTrait = container.GetTraits(
                    TraitKind.Slot, TraitKind.Constant, TraitKind.Getter)
                    .Where(t => t.QName == propertyName)
                    .FirstOrDefault();

                propertyTypeName = propertyTrait?.Type.Name;
            }
            return (propertyTypeName != null);
        }
        #endregion
    }
    public class MessageReference
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