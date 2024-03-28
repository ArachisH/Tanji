using System.Net;
using System.Text;
using System.Buffers;

using Tanji.Core.Net.Formats;
using Tanji.Core.Net.Messages;

using Flazzy;
using Flazzy.IO;
using Flazzy.ABC;
using Flazzy.Tags;
using Flazzy.Tools;
using Flazzy.ABC.AVM2;
using Flazzy.ABC.AVM2.Instructions;

namespace Tanji.Core.Canvas.Flash;

public sealed class FlashGame : IGame
{
    #region Reserved Flash/AS3 Keywords
    private static readonly string[] ReservedKeywords = new[]
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

    private readonly ShockwaveFlash _flash;
    private readonly Dictionary<DoABCTag, ABCFile> _abcFileTags;
    private readonly Dictionary<uint, FlashMessage> _flashMessagesByHash;
    private readonly Dictionary<string, FlashMessage> _flashMessagesByClassName;

    private ASCode? _scSendCode, _scSendUnencryptedCode;
    private ASMethod? _scInitMethod, _scSendMethod, _scSendUnencryptedMethod;

    private ASMethod? _hcmNextPortMethod, _hcmUpdateHostParametersMethod;
    private ASInstance? _socketConnectionInstance, _habboCommunicationDemoInstance, _habboCommunicationManagerInstance;

    public HPlatform Platform => HPlatform.Flash;
    public bool IsPostShuffle { get; private set; } = true;

    public IHFormat SendPacketFormat => IsPostShuffle ? IHFormat.EvaWire : IHFormat.WedgieOut;
    public IHFormat ReceivePacketFormat => IsPostShuffle ? IHFormat.EvaWire : IHFormat.WedgieIn;

    public string? Revision { get; private set; }
    public int MinimumConnectionAttempts { get; private set; }

    public string? Path { get; private set; }
    public GamePatchingOptions AppliedPatchingOptions { get; private set; }

    public bool IsDisposed { get; private set; }

    public FlashGame(string path)
        : this(new ShockwaveFlash(path))
    {
        Path = path;
    }
    public FlashGame(byte[] data)
        : this(new ShockwaveFlash(data))
    { }
    public FlashGame(Stream stream)
        : this(new ShockwaveFlash(stream))
    { }
    private FlashGame(ShockwaveFlash flash)
    {
        _flash = flash;
        _abcFileTags = new Dictionary<DoABCTag, ABCFile>();
        _flashMessagesByHash = new Dictionary<uint, FlashMessage>(1000);
        _flashMessagesByClassName = new Dictionary<string, FlashMessage>(1000);
    }

    public void GenerateMessageHashes()
    {
        // Hashes have already been generated.
        if (_flashMessagesByHash.Count > 0) return;

        // Find references to all known message types throughout every ABCFile instance.
        var methodsReferencingMessages = new Dictionary<string, (int, FlashMessage[])>();
        FlashMessage[] referencedMessagesBuffer = ArrayPool<FlashMessage>.Shared.Rent(64);
        foreach (ASMethod method in _abcFileTags.Values.SelectMany(abc => abc.Methods))
        {
            if (method.IsAnonymous) continue;
            if (method.Body == null) continue;

            (int OutCount, int InCount) = FindMessageReferences(method, referencedMessagesBuffer);
            if (OutCount == 0 || method.IsConstructor) continue;

            FlashMessage[] referencedMessages = ArrayPool<FlashMessage>.Shared.Rent(OutCount);
            referencedMessagesBuffer.AsSpan(referencedMessagesBuffer.Length - OutCount, OutCount).CopyTo(referencedMessages);

            string fullName = $"{method.Container.QName.Namespace.Name}:{method.Container.QName.Name}.{method.Trait.QName.Name}";
            methodsReferencingMessages.Add(fullName, (OutCount, referencedMessages));
            if (method.Container.IsStatic) continue;

            var instance = (ASInstance)method.Container;
            foreach (ASMultiname interfaceName in instance.GetInterfaces())
            {
                ASInstance interfaceInstance = method.ABC.GetInstance(interfaceName.Name);
                if (interfaceInstance == null) continue;

                if (interfaceInstance.GetMethod(method.Trait.QName.Name, method.ReturnType.Name, method.Parameters.Count) == null) continue;

                fullName = $"{interfaceInstance.QName.Namespace.Name}:{interfaceName.Name}.{method.Trait.QName.Name}";
                if (methodsReferencingMessages.TryGetValue(fullName, out (int Count, FlashMessage[] InterfaceReferencedMessages) messages))
                {
                    methodsReferencingMessages.Remove(fullName);
                }

                FlashMessage[] interfaceReferencedMessages = ArrayPool<FlashMessage>.Shared.Rent(OutCount + messages.Count);
                Array.Copy(referencedMessages, 0, interfaceReferencedMessages, 0, OutCount);

                if (messages.InterfaceReferencedMessages != null)
                {
                    Array.Copy(messages.InterfaceReferencedMessages, 0, interfaceReferencedMessages, OutCount, messages.Count);
                    ArrayPool<FlashMessage>.Shared.Return(messages.InterfaceReferencedMessages);
                }
                methodsReferencingMessages.Add(fullName, (OutCount + messages.Count, interfaceReferencedMessages));
            }
        }
        methodsReferencingMessages.TrimExcess();
        ArrayPool<FlashMessage>.Shared.Return(referencedMessagesBuffer);

        // Conducts a deeper reference scan where every method that constructs an outgoing message is checked for useage in other methods.
        FindMessageReferences(methodsReferencingMessages);
        foreach ((int Count, FlashMessage[] ReferencedMessages) in methodsReferencingMessages.Values)
        {
            ArrayPool<FlashMessage>.Shared.Return(ReferencedMessages);
        }

        var hashBuffer = new FlashMessageHasher(256);
        foreach (FlashMessage flashMessage in _flashMessagesByClassName.Values)
        {
            if (flashMessage.References.Count == 0) continue;

            int collisions = 0;
            uint hash = FlashMessageHasher.Generate(hashBuffer, flashMessage);
            while (_flashMessagesByHash.ContainsKey(hash))
            {
                hash = FlashMessageHasher.Generate(hashBuffer, flashMessage, ++collisions);
            }

            flashMessage.Hash = hash;
            _flashMessagesByHash.Add(hash, flashMessage);
        }
    }
    public void Patch(GamePatchingOptions options)
    {
        AppliedPatchingOptions = options;
        foreach (HPatches patch in Enum.GetValues(typeof(HPatches)))
        {
            if ((options.Patches & patch) != patch || patch == HPatches.None || patch == HPatches.FlashDefaults) continue;
            if (!TryPatch(options, patch))
            {
                throw new Exception($"Failed to apply patch to the game client: {patch}");
            }
        }
    }
    public bool TryResolveMessage(uint hash, out HMessage message)
    {
        message = default;
        if (_flashMessagesByHash.TryGetValue(hash, out FlashMessage? flashMessage))
        {
            message = new HMessage(flashMessage.Id, flashMessage.IsOutgoing)
            {
                Hash = flashMessage.Hash
            };
        }
        return message != default;
    }
    public bool TryResolveMessage(string name, out HMessage message)
    {
        message = default;
        if (_flashMessagesByClassName.TryGetValue(name, out FlashMessage? flashMessage))
        {
            message = new HMessage(flashMessage.Id, flashMessage.IsOutgoing)
            {
                Hash = flashMessage.Hash
            };
        }
        return message != default;
    }

    private bool PrepareFlashMessages(ABCFile abc)
    {
        ASClass? habboMessagesClass = null;
        foreach (ASClass @class in abc.Classes)
        {
            if (@class.Traits.Count != 2 || @class.Instance.Traits.Count != 2) continue;

            if (@class.Traits[0].QName.Namespace.Kind != NamespaceKind.Private) continue;
            if (@class.Instance.Traits[0].Kind != TraitKind.Getter) continue;

            if (@class.Traits[1].QName.Namespace.Kind != NamespaceKind.Private) continue;
            if (@class.Instance.Traits[1].Kind != TraitKind.Getter) continue;

            habboMessagesClass = @class;
            break;
        }

        if (habboMessagesClass == null)
        {
            foreach (ASClass @class in abc.Classes)
            {
                if (@class.Traits.Count != 2) continue;
                if (@class.Instance.Traits.Count != 3) continue;

                habboMessagesClass = @class;
                break;
            }
            IsPostShuffle = habboMessagesClass == null;
        }
        if (habboMessagesClass == null) return false; // Neither pre/post shuffle client.

        ASCode code = habboMessagesClass.Constructor.Body.ParseCode();
        int inMapTypeIndex = habboMessagesClass.Traits[0].QNameIndex;
        int outMapTypeIndex = habboMessagesClass.Traits[1].QNameIndex;

        ASInstruction[] instructions = code.Where(i => i.OP
            is OPCode.GetLex
            or OPCode.PushShort
            or OPCode.PushByte).ToArray();

        for (int i = 0; i < instructions.Length; i += 3)
        {
            if (instructions[i] is not GetLexIns getLexMapIns) continue;
            bool isOutgoing = getLexMapIns.TypeNameIndex == outMapTypeIndex;

            if (instructions[i + 1] is not Primitive primitive) continue;
            short id = Convert.ToInt16(primitive.Value);

            if (instructions[i + 2] is not GetLexIns getLexTypeIns) continue;
            ASClass messageClass = abc.GetClass(getLexTypeIns.TypeName);

            string? structure = null;
            ASClass? parserClass = null;
            if (!isOutgoing)
            {
                parserClass = GetMessageParser(messageClass);
                if (parserClass != null)
                {
                    structure = GetIncomingStructure(parserClass);
                }
            }
            else structure = GetOutgoingStructure(messageClass, messageClass);

            if (!_flashMessagesByClassName.ContainsKey(messageClass.QName.Name))
            {
                var message = new FlashMessage(id, isOutgoing, messageClass)
                {
                    Structure = structure,
                    ParserClass = parserClass,
                };
                _flashMessagesByClassName.Add(messageClass.QName.Name, message);
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

        _flashMessagesByClassName.TrimExcess();
        _flashMessagesByHash.EnsureCapacity(_flashMessagesByClassName.Count);
        return habboMessagesClass != null;
    }
    private bool PrepareSocketConnection(ABCFile abc)
    {
        if (_socketConnectionInstance == null)
        {
            _socketConnectionInstance =
                abc.GetInstance("SocketConnection") ??
                abc.GetInstanceByConstructor("com.sulake.core.communication.connection.SocketConnection");

            if (_socketConnectionInstance != null)
            {
                foreach (ASMethod method in _socketConnectionInstance.GetMethods(null, "Boolean", 1))
                {
                    if (method.Trait.QName.NamespaceIndex != 1) continue; // Must be a public method.
                    if (_scSendMethod == null)
                    {
                        _scSendMethod = method;
                    }
                    else if (_scSendUnencryptedMethod == null)
                    {
                        _scSendUnencryptedMethod = method;
                        break; // This should be the last method, so exit the loop.
                    }
                }

                if (_scSendMethod == null && _scSendUnencryptedMethod == null) return false;

                _scInitMethod = _socketConnectionInstance.GetMethod("init", "Boolean", 2) ??
                    _socketConnectionInstance.GetMethod("init", "Boolean", 3); // 'TCP_NODELAY' Flag
                if (_scInitMethod == null) return false;

                // Deobfuscation on this method on older clients is tricky, avoid for now.
                //ASMethod initMethod = _socketConnectionInstance.GetMethod("init", "Boolean", 2);
                //if (initMethod != null)
                //{
                //    ASCode initCode = initMethod.Body.ParseCode();
                //    initCode.Deobfuscate();
                //    initMethod.Body.Code = initCode.ToArray();
                //}

                // send Deobfuscation
                if (_scSendMethod != null)
                {
                    _scSendCode = _scSendMethod.Body.ParseCode();
                    _scSendCode.Deobfuscate();
                    _scSendMethod.Body.Code = _scSendCode.ToArray();
                }

                // sendUnencrypted Deobfuscation
                if (_scSendUnencryptedMethod != null)
                {
                    _scSendUnencryptedCode = _scSendUnencryptedMethod.Body.ParseCode();
                    _scSendUnencryptedCode.Deobfuscate();
                    _scSendUnencryptedMethod.Body.Code = _scSendUnencryptedCode.ToArray();
                }
            }
        }
        return _socketConnectionInstance != null;
    }
    private bool PrepareHabboCommunicationDemo(ABCFile abc)
    {
        if (_habboCommunicationDemoInstance == null)
        {
            _habboCommunicationDemoInstance =
                abc.GetInstance("HabboCommunicationDemo") ??
                abc.GetInstanceByConstructor("com.sulake.habbo.communication.demo.IncomingMessages");

            if (_habboCommunicationDemoInstance == null)
            {
                foreach (ASInstance instance in abc.Instances)
                {
                    if (!instance.Flags.HasFlag(ClassFlags.Sealed)) continue;

                    if (instance.IsStatic) continue;
                    if (instance.InterfaceIndices.Count > 0) continue;
                    if (instance.Constructor.Parameters.Count < 2) continue;

                    if (instance.Constructor.Body.MaxStack < 3) continue;
                    if (instance.Constructor.Body.LocalCount < 4) continue;

                    if (instance.Constructor.Body.Code.Length < 300) continue;
                    if (instance.Constructor.Body.Code.Length > 500) continue;

                    ASCode code = instance.Constructor.Body.ParseCode();
                    foreach (PushStringIns pushStringIns in code.GetOPGroup(OPCode.PushString))
                    {
                        if (pushStringIns.Value == "Connection is required to initialize!")
                        {
                            _habboCommunicationDemoInstance = instance;
                        }
                        break; // The first PushString instruction should be what we are looking for.
                    }
                    if (_habboCommunicationDemoInstance != null) break;
                }
            }
        }
        return _habboCommunicationDemoInstance != null;
    }
    private bool PrepareHabboCommunicationManager(ABCFile abc)
    {
        if (_habboCommunicationManagerInstance == null)
        {
            _habboCommunicationManagerInstance =
                abc.GetInstance("HabboCommunicationManager") ??
                abc.GetInstanceByConstructor("com.sulake.habbo.communication.HabboCommunicationManager");

            // If the desired ASInstance object was not found above, then attempt to find a matching ASInstance element using its' traits.
            if (_habboCommunicationManagerInstance == null)
            {
                foreach (ASInstance instance in abc.Instances)
                {
                    if (instance.Super == null) continue;
                    if (!instance.Super.Name.ToLower().Equals("element") && !instance.Super.Name.ToLower().Equals("component")) continue;

                    if (instance.InterfaceIndices.Count != 2) continue;
                    if (instance.Constructor.Parameters.Count != 3) continue;
                    if (instance.Traits.Count is < 35 or >= 50) continue;

                    _habboCommunicationManagerInstance = instance;
                    break;
                }
            }

            //Attempts to find this object failed.
            if (_habboCommunicationManagerInstance == null) return false;

            ASTrait? hostTrait = _habboCommunicationManagerInstance.GetSlotTraits("String").FirstOrDefault();
            if (hostTrait == null) return false;

            ASMethod? initComponentMethod = _habboCommunicationManagerInstance.GetMethod("initComponent", "void", 0);
            if (initComponentMethod == null) return false;

            int callPropVoidCount = 0;
            ASCode initComponentCode = initComponentMethod.Body.ParseCode();

            for (int i = initComponentCode.Count - 1; i >= 0; i--)
            {
                ASInstruction instruction = initComponentCode[i];
                if (instruction.OP != OPCode.CallPropVoid) continue;

                var callPropVoidIns = (CallPropVoidIns)instruction;

                switch (callPropVoidCount)
                {
                    case 0: _hcmNextPortMethod = _habboCommunicationManagerInstance.GetMethod(callPropVoidIns.PropertyName.Name, "void", callPropVoidIns.ArgCount); break;
                    case 1:
                    {
                        _hcmUpdateHostParametersMethod = _habboCommunicationManagerInstance.GetMethod(callPropVoidIns.PropertyName.Name, "void", callPropVoidIns.ArgCount);
                        ASCode code = _hcmUpdateHostParametersMethod.Body.ParseCode();

                        code.Deobfuscate();
                        _hcmUpdateHostParametersMethod.Body.Code = code.ToArray();
                        break;
                    }
                }
                if (callPropVoidCount++ == 1) break;
            }
        }
        return _habboCommunicationManagerInstance != null;
    }

    public byte[] ToArray()
    {
        using var assembleStream = new MemoryStream((int)_flash.FileLength);
        using var output = new FlashWriter(assembleStream);
        Assemble(output, CompressionKind.ZLib);
        return assembleStream.ToArray();
    }
    public void Disassemble()
    {
        var upgrader = new AS3MultinameUpgrader(true, true);
        _flash.Disassemble(tag =>
        {
            if (tag.Kind != TagKind.DoABC) return;

            var doABCTag = (DoABCTag)tag;
            var abcFile = new ABCFile(doABCTag.ABCData);

            upgrader.Search(abcFile);
            abcFile.ResetCache();

            _abcFileTags[doABCTag] = abcFile;
        });
        _abcFileTags.TrimExcess();

        foreach (ABCFile abc in _abcFileTags.Values)
        {
            if (_flashMessagesByClassName.Count == 0)
            {
                PrepareFlashMessages(abc);
            }
            PrepareSocketConnection(abc);
            PrepareHabboCommunicationDemo(abc);
            PrepareHabboCommunicationManager(abc);
        }
    }
    public void Assemble(string path)
    {
        using FileStream assembleStream = File.Open(path, FileMode.Create);
        using var output = new FlashWriter(assembleStream);
        Assemble(output, CompressionKind.ZLib);
    }
    private void Assemble(FlashWriter output, CompressionKind compression)
    {
        _flash.Assemble(output, compression, t =>
        {
            if (t.Kind == TagKind.DoABC)
            {
                var doABCTag = (DoABCTag)t;
                doABCTag.ABCData = _abcFileTags[doABCTag].ToArray();
            }
        });
        output.Flush();
    }

    private static bool IsValidIdentifier(string value)
    {
        return !string.IsNullOrWhiteSpace(value) &&
            !value.StartsWith("_-") &&
            !ReservedKeywords.Contains(value.ToLower());
    }

    #region Patching Methods
    private bool TryPatch(GamePatchingOptions options, HPatches patch) => patch switch
    {
        HPatches.InjectAddress => TryInjectAddress(options.InjectedAddress),
        HPatches.InjectKeyShouter => TryInjectKeyShouter(options.KeyShoutingId),
        HPatches.DisableHostChecks => TryDisableHostChecks(),
        HPatches.DisableEncryption => TryDisableEncryption(),
        HPatches.InjectAddressShouter => TryInjectAddressShouter(options.AddressShoutingId),
        HPatches.InjectRSAKeys => TryInjectRSAKeys(options.RSAExponent, options.RSAModulus),
        _ => false
    };

    private bool TryDisableHostChecks()
    {
        ASMethod? localHostCheckMethod = null;
        foreach (ABCFile abc in _abcFileTags.Values)
        {
            foreach (ASMethod method in abc.Methods)
            {
                if (method.ReturnType?.Name != "Boolean") continue;
                if (method.Parameters.Count != 1) continue;
                if (method.Parameters[0].Type.Name != "Sprite") continue;
                localHostCheckMethod = method;
                break;
            }
            if (localHostCheckMethod != null) break;
        }
        if (localHostCheckMethod == null) return false;

        ASInstance habboInstance = _abcFileTags.Values.First().GetInstance("Habbo");
        if (habboInstance != null)
        {
            ASMethod remoteHostCheckMethod = habboInstance.GetMethod(null, "Boolean", ["String", "Object"]);
            if (remoteHostCheckMethod != null)
            {
                remoteHostCheckMethod.Body.Code[0] = (byte)OPCode.PushTrue;
                remoteHostCheckMethod.Body.Code[1] = (byte)OPCode.ReturnValue;
            }
        }

        localHostCheckMethod.Body.Code[0] = (byte)OPCode.PushTrue;
        localHostCheckMethod.Body.Code[1] = (byte)OPCode.ReturnValue;
        return LockInfoHostProperty(out _);
    }
    private bool TryDisableEncryption(ASCode? sendCode = null)
    {
        sendCode ??= _scSendCode;
        if (sendCode == null) return false;

        int localCount = 0;
        for (int i = sendCode.Count - 1; i >= 0; i--)
        {
            ASInstruction instruction = sendCode[i];
            if (instruction.OP == OPCode.PushNull)
            {
                /* 
                 * Remove the encryption cipher null check.
                 * if (_clientToServerEncryption == null)
                 * {
                 *     return false;
                 * }
                 */
                sendCode.RemoveRange(i - 1, 5);
                break;
            }
            else if (Local.IsValid(instruction.OP) && ++localCount == 2)
            {
                /* 
                 * Remove the instructions that attempt to encrypt a local block of data.
                 * _clientToServerEncryption.cipher(local);
                 */
                sendCode.RemoveRange(--i, 3);
            }
        }
        if (sendCode == _scSendCode && _scSendMethod != null)
        {
            _scSendMethod.Body.Code = _scSendCode.ToArray();
        }
        return true;
    }

    private bool TryInjectAddress(IPEndPoint? remoteAddress)
    {
        if (_socketConnectionInstance == null || _scInitMethod == null || remoteAddress == null) return false;
        if (!TryInjectAddressSaver(out _, out _)) return false;

        ASCode initCode = _scInitMethod.Body.ParseCode();
        for (int i = 0; i < initCode.Count; i++)
        {
            ASInstruction instruction = initCode[i];
            if (Jumper.IsValid(instruction.OP))
            {
                initCode.InsertRange(i - 2,
                    [
                        new PushStringIns(_socketConnectionInstance.ABC, remoteAddress.Address.ToString()),
                        new SetLocal1Ins(),

                        new PushIntIns(_socketConnectionInstance.ABC, remoteAddress.Port),
                        new SetLocal2Ins()
                    ]);
                break;
            }
        }

        _scInitMethod.Body.Code = initCode.ToArray();
        MinimumConnectionAttempts = CountConnectionAttempts();
        return true;
    }
    private bool TryInjectAddressSaver(out ASTrait? hostTrait, out ASTrait? portTrait)
    {
        if (_socketConnectionInstance == null || _scInitMethod == null)
        {
            hostTrait = portTrait = null;
            return false;
        }

        hostTrait = _socketConnectionInstance.GetSlot("remoteHost");
        portTrait = _socketConnectionInstance.GetSlot("remotePort");
        if (hostTrait != null && portTrait != null) return true;

        portTrait = _socketConnectionInstance.AddSlot("remotePort", "int");
        hostTrait = _socketConnectionInstance.AddSlot("remoteHost", "String");

        ASCode initCode = _scInitMethod.Body.ParseCode();
        initCode.InsertRange(2,
        [
            new GetLocal0Ins(),
            new GetLocal1Ins(),
            new SetPropertyIns(_socketConnectionInstance.ABC, hostTrait.QNameIndex),

            new GetLocal0Ins(),
            new GetLocal2Ins(),
            new SetPropertyIns(_socketConnectionInstance.ABC, portTrait.QNameIndex)
        ]);
        _scInitMethod.Body.MaxStack += 2;
        _scInitMethod.Body.Code = initCode.ToArray();
        return true;
    }

    private bool TryInjectKeyShouter(int keyShoutingId)
    {
        if (_habboCommunicationDemoInstance == null) return false;

        ASTrait? sendFunction = InjectUniversalSendFunction();
        if (sendFunction == null) return false;

        ASMethod onCompleteDiffieHandshakeMethod = _habboCommunicationDemoInstance.GetMethod("onCompleteDiffieHandshake", "void", 1);
        if (onCompleteDiffieHandshakeMethod != null)
        {
            foreach (ASMethod method in _habboCommunicationDemoInstance.GetMethods(null, "void", 1))
            {
                if (method.Flags != MethodFlags.None) continue;
                if (method.Body.MaxStack != 4) continue;
                if (method.Body.LocalCount < 10) continue;

                onCompleteDiffieHandshakeMethod = method;
                break;
            }
        }
        if (onCompleteDiffieHandshakeMethod == null) return false;

        int connectionInstanceRegister = 2;
        ASCode onCompleteDiffieHandshakeCode = onCompleteDiffieHandshakeMethod.Body.ParseCode();
        for (int i = 0; i < onCompleteDiffieHandshakeCode.Count; i++)
        {
            ASInstruction instruction = onCompleteDiffieHandshakeCode[i];
            if (instruction.OP == OPCode.Coerce)
            {
                if (Local.IsSetLocal(onCompleteDiffieHandshakeCode[i + 1].OP))
                {
                    var local = (Local)onCompleteDiffieHandshakeCode[i + 1];
                    connectionInstanceRegister = local.Register;
                }
                break;
            }
        }

        // {SocketConnection}.sendMessage({keyShoutingId}, {sharedKey});
        onCompleteDiffieHandshakeCode.InsertRange(onCompleteDiffieHandshakeCode.Count - 5, new ASInstruction[]
        {
            new GetLocalIns(connectionInstanceRegister),
            new PushIntIns(_habboCommunicationDemoInstance.ABC, keyShoutingId),
            new GetLocalIns(2),
            new CallPropVoidIns(_habboCommunicationDemoInstance.ABC, sendFunction.QNameIndex, 2)
        });

        onCompleteDiffieHandshakeMethod.Body.MaxStack += 4;
        onCompleteDiffieHandshakeMethod.Body.Code = onCompleteDiffieHandshakeCode.ToArray();
        return true;
    }
    private bool TryInjectAddressShouter(int addressShoutingId)
    {
        if (_habboCommunicationDemoInstance == null) return false;

        ASTrait? sendFunction = InjectUniversalSendFunction();
        if (sendFunction == null) return false;

        if (!TryInjectAddressSaver(out ASTrait? hostTrait, out ASTrait? portTrait)) return false;
        if (hostTrait == null || portTrait == null) return false;

        ASCode? onConnectionEstablishedCode = null;
        ASMethod onConnectionEstablishedMethod = _habboCommunicationDemoInstance.GetMethod("onConnectionEstablished");
        if (onConnectionEstablishedMethod == null)
        {
            foreach (ASMethod method in _habboCommunicationDemoInstance.GetMethods(null, "void", ["Event"]))
            {
                if (!IsPostShuffle && !method.Name.EndsWith("onConnectionEstablished")) continue;
                if (method.Flags != MethodFlags.HasOptional) continue;

                onConnectionEstablishedCode = method.Body.ParseCode();
                for (int i = 0; i < onConnectionEstablishedCode.Count; i++)
                {
                    ASInstruction instruction = onConnectionEstablishedCode[i];
                    if (instruction.OP != OPCode.GetLocal_2) continue;
                    if (onConnectionEstablishedCode[i + 1].OP == OPCode.PushNull) continue;

                    onConnectionEstablishedMethod = method;
                    break;
                }
            }
        }

        if (onConnectionEstablishedMethod == null) return false;
        onConnectionEstablishedCode ??= onConnectionEstablishedMethod.Body.ParseCode();

        // local2.sendMessage(messageId, local2.remoteHost, int(local2.remotePort))
        ABCFile abc = _habboCommunicationDemoInstance.ABC;
        onConnectionEstablishedCode.InsertRange(onConnectionEstablishedCode.IndexOf(OPCode.IfEq) + 1,
        [
            new GetLocal2Ins(),

            new PushIntIns(abc, addressShoutingId),

            new GetLocal2Ins(),
            new GetPropertyIns(abc, hostTrait.QNameIndex),

            new GetLocal2Ins(),
            new GetPropertyIns(abc, portTrait.QNameIndex),
            new ConvertIIns(),

            new CallPropVoidIns(abc, sendFunction.QNameIndex, 3)
        ]);

        onConnectionEstablishedMethod.Body.MaxStack += 5;
        onConnectionEstablishedMethod.Body.Code = onConnectionEstablishedCode.ToArray();

        return onConnectionEstablishedMethod != null;
    }
    private bool TryInjectRSAKeys(string? exponent, string? modulus)
    {
        if (_habboCommunicationDemoInstance == null) return false;
        if (string.IsNullOrWhiteSpace(exponent))
        {
            ThrowHelper.ThrowArgumentException("The specified public key must not be empty or null.", nameof(exponent));
        }
        if (string.IsNullOrWhiteSpace(modulus))
        {
            ThrowHelper.ThrowArgumentException("The specified public key must not be empty or null.", nameof(modulus));
        }

        foreach (ASMethod method in _habboCommunicationDemoInstance.GetMethods(null, "void", 1))
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
                    new PushStringIns(_habboCommunicationDemoInstance.ABC, modulus),
                    new PushStringIns(_habboCommunicationDemoInstance.ABC, exponent),
                });

                method.Body.Code = code.ToArray();
                return true;
            }
        }
        return false;
    }

    private int CountConnectionAttempts()
    {
        if (_hcmNextPortMethod == null) return 0;

        if (!LockInfoHostProperty(out ASTrait? infoHostSlot)) return 0;
        if (infoHostSlot == null) return 0;

        int connectionAttempts = 0;
        ASCode connectCode = _hcmNextPortMethod.Body.ParseCode();
        for (int i = 0; i < connectCode.Count; i++)
        {
            ASMultiname propertyName;
            ASInstruction instruction = connectCode[i];
            if (instruction.OP == OPCode.GetLex)
            {
                propertyName = ((GetLexIns)instruction).TypeName;
            }
            else if (instruction.OP == OPCode.GetProperty)
            {
                propertyName = ((GetPropertyIns)instruction).PropertyName;
            }
            else continue;

            if (propertyName != infoHostSlot.QName) continue;
            connectionAttempts++;
        }
        return connectionAttempts;
    }
    private ASTrait? InjectUniversalSendFunction()
    {
        if (_socketConnectionInstance == null || _scSendMethod == null) return null;
        ABCFile abc = _socketConnectionInstance.ABC;

        ASTrait? sendFunctionTrait = _socketConnectionInstance.GetMethod("sendMessage", "Boolean", 1)?.Trait;
        if (sendFunctionTrait != null) return sendFunctionTrait;

        ASCode trimmedSendCode = _scSendMethod.Body.ParseCode();
        TryDisableEncryption(trimmedSendCode);

        for (int i = 6; i < trimmedSendCode.Count; i++)
        {
            ASInstruction instruction = trimmedSendCode[i];
            if (instruction.OP == OPCode.Coerce)
            {
                var coerceIns = (CoerceIns)instruction;
                if (coerceIns.TypeName.Name != "ByteArray") continue;

                trimmedSendCode.RemoveRange(6, i - 10);
                break;
            }
        }
        trimmedSendCode.InsertRange(2, new ASInstruction[2]
        {
            new GetLocal1Ins(),
            new SetLocalIns(4)
        });

        var sendMessageMethod = new ASMethod(abc);
        sendMessageMethod.Flags |= MethodFlags.NeedRest;
        sendMessageMethod.ReturnTypeIndex = _scSendMethod.ReturnTypeIndex;
        int sendMessageMethodIndex = abc.AddMethod(sendMessageMethod);

        // The parameters for the instructions to expect / use.
        var idParam = new ASParameter(sendMessageMethod)
        {
            NameIndex = abc.Pool.AddConstant("id"),
            TypeIndex = abc.Pool.GetMultinameIndex("int")
        };
        sendMessageMethod.Parameters.Add(idParam);

        // The method body that houses the instructions.
        var sendMessageBody = new ASMethodBody(abc)
        {
            MethodIndex = sendMessageMethodIndex,
            Code = trimmedSendCode.ToArray(),
            InitialScopeDepth = 0,
            MaxScopeDepth = 1,
            LocalCount = 7,
            MaxStack = 7
        };
        abc.AddMethodBody(sendMessageBody);

        _socketConnectionInstance.AddMethod(sendMessageMethod, "sendMessage");
        return sendMessageMethod.Trait;
    }
    private bool LockInfoHostProperty(out ASTrait? infoHostSlot)
    {
        if (_habboCommunicationManagerInstance == null || _hcmNextPortMethod == null)
        {
            infoHostSlot = null;
            return false;
        }

        ABCFile abc = _habboCommunicationManagerInstance.ABC;

        ASCode connectCode = _hcmNextPortMethod.Body.ParseCode();
        int pushByteIndex = connectCode.IndexOf(OPCode.PushByte);

        infoHostSlot = _habboCommunicationManagerInstance.GetSlotTraits("String").FirstOrDefault();
        if (infoHostSlot == null) return false;

        int getPropertyIndex = abc.Pool.GetMultinameIndex("getProperty");
        connectCode.InsertRange(pushByteIndex,
        [
            new GetLocal0Ins(),
            new FindPropStrictIns(abc, getPropertyIndex),
            new PushStringIns(abc, "connection.info.host"),
            new CallPropertyIns(abc, getPropertyIndex, 1),
            new InitPropertyIns(abc, infoHostSlot.QNameIndex)
        ]);

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

        _hcmNextPortMethod.Body.MaxStack += 4;
        _hcmNextPortMethod.Body.Code = connectCode.ToArray();
        return true;
    }
    #endregion

    #region Hashing/Message Linking
    private void FindMessageReferences(Dictionary<string, (int, FlashMessage[])> methodsReferencingMessages)
    {
        var names = new Stack<ASMultiname>();
        var nameBuilder = new StringBuilder();
        foreach (ASMethod method in _abcFileTags.Values.SelectMany(abc => abc.Methods))
        {
            if (method.IsAnonymous) continue;
            if (method.Body == null) continue;

            if (method.IsConstructor) continue;
            if (method.Container.IsStatic) continue;
            if (method.Container is not ASInstance instance) continue;

            int order = 0;
            var codeReader = new SpanFlashReader(method.Body.Code);
            while (codeReader.IsDataAvailable)
            {
                ASMultiname? slotName = null;
                var instruction = ASInstruction.Create(method.ABC, ref codeReader);
                if (instruction.OP == OPCode.GetProperty)
                {
                    slotName = ((GetPropertyIns)instruction).PropertyName;
                }
                else if (instruction.OP == OPCode.GetLex)
                {
                    slotName = ((GetLexIns)instruction).TypeName;
                }
                if (slotName != null)
                {
                    if (slotName.Kind == MultinameKind.Multiname) continue;
                    if (slotName.Namespace?.Name == instance.ProtectedNamespace?.Name) // This is a private field in the current container.
                    {
                        ASTrait? trait = method.Container.Traits.FirstOrDefault(t => t.QName == slotName);
                        if (trait?.Type == null || trait.Type.Kind == MultinameKind.TypeName) continue;

                        names.Push(trait.Type);
                    }
                    else if (names.Count > 0)
                    {
                        ASMultiname slotReturnType = names.Peek();
                        ASInstance slotInstance = method.ABC.GetInstance(slotReturnType);
                        if (slotInstance != null)
                        {
                            ASTrait? innerTrait = slotInstance.Traits.FirstOrDefault(t => t.QName == slotName);
                            if (innerTrait != null)
                            {
                                ASMultiname name = innerTrait.Method?.ReturnType ?? innerTrait.Type;
                                if (name == null || name.Kind == MultinameKind.TypeName) continue;

                                names.Pop();
                                names.Push(name);
                            }
                        }
                    }
                }
                if (instruction.OP != OPCode.CallPropVoid) continue;

                var callPropVoidIns = (CallPropVoidIns)instruction;
                if (string.IsNullOrWhiteSpace(callPropVoidIns.PropertyName.Namespace?.Name) && names.Count == 0) continue;

                if (names.Count > 0)
                {
                    ASMultiname slotType = names.Pop();
                    nameBuilder.Append(slotType.Namespace.Name);
                    nameBuilder.Append(':');
                    nameBuilder.Append(slotType.Name);
                }
                else nameBuilder.Append(callPropVoidIns.PropertyName.Namespace?.Name);

                nameBuilder.Append('.');
                nameBuilder.Append(callPropVoidIns.PropertyName.Name);

                string fullName = nameBuilder.ToString();
                if (methodsReferencingMessages.TryGetValue(fullName, out (int Count, FlashMessage[] Messages) referencedMessages))
                {
                    for (int i = 0; i < referencedMessages.Count; i++)
                    {
                        referencedMessages.Messages[i].References.Add(new FlashMessageReference
                        {
                            Method = method,
                            OrderInMethod = order++,
                            ArgumentsUsed = callPropVoidIns.ArgCount
                        });
                    }
                }
                nameBuilder.Clear();
            }
            names.Clear();
        }
    }
    private (int OutCount, int InCount) FindMessageReferences(ASMethod method, Span<FlashMessage> referencedMessagesBuffer)
    {
        int outCount = 0, inCount = 0;
        var names = new Stack<ASMultiname>(2);
        ConstructPropIns? messageConstructIns = null;
        var codeReader = new SpanFlashReader(method.Body.Code);
        while (codeReader.IsDataAvailable)
        {
            var instruction = ASInstruction.Create(method.ABC, ref codeReader);
            switch (instruction.OP)
            {
                case OPCode.GetLex: names.Push(((GetLexIns)instruction).TypeName); continue;

                case OPCode.GetProperty: names.Push(((GetPropertyIns)instruction).PropertyName); continue;

                // Break on this instruction, as it could be calling the constructor of a message.
                case OPCode.ConstructProp: messageConstructIns = (ConstructPropIns)instruction; break;

                case OPCode.NewFunction:
                {
                    long currentPos = codeReader.Position;
                    if (ASInstruction.Create(method.ABC, ref codeReader).OP != OPCode.Pop)
                    {
                        (int OutCount, int InCount) = FindMessageReferences(((NewFunctionIns)instruction).Method, referencedMessagesBuffer[(outCount + inCount)..]);
                        outCount += OutCount;
                        inCount += InCount;

                        codeReader.Position = (int)currentPos;
                    }
                    continue;
                }

                default: continue;
            }

            if (!_flashMessagesByClassName.TryGetValue(messageConstructIns.PropertyName.Name, out FlashMessage? flashMessage)) continue;
            if (flashMessage.IsOutgoing)
            {
                referencedMessagesBuffer[referencedMessagesBuffer.Length - 1 - outCount++] = flashMessage;
            }
            else referencedMessagesBuffer[inCount++] = flashMessage;

            ASMethod? callbackMethod = null;
            if (flashMessage.ParserClass != null)
            {
                ASMultiname callbackName = names.Pop(); // This should be the QName of the callback method.
                if (!flashMessage.IsOutgoing && callbackName == flashMessage.ParserClass.QName)
                {
                    callbackName = names.Pop();
                }

                ASContainer callbackContainer = method.Container;
                ASTrait? trait = callbackContainer.Traits.FirstOrDefault(t => t.QName == callbackName);
                if (trait == null && names.Count > 0) // This is perhaps a reference to an instance slot/field, or a method in a static class.
                {
                    trait = callbackContainer.Traits.FirstOrDefault(t => t.QName == names.Peek()); // Is the next name possibly a trait in the current container?
                    if (trait != null)
                    {
                        names.Pop(); // No longer a possibility of this being a static class, remove it from the stack.
                        callbackContainer = method.ABC.GetInstance(trait.Type);
                    }
                    else callbackContainer = method.ABC.GetClass(names.Pop()); // This HAS to be a static class, right?
                    trait = callbackContainer.Traits.FirstOrDefault(t => t.QName == callbackName);
                }
                callbackMethod = trait?.Method;
            }

            flashMessage.References.Add(new FlashMessageReference
            {
                Method = method,
                Callback = callbackMethod,
                OrderInMethod = outCount + inCount,
                ArgumentsUsed = messageConstructIns.ArgCount
            });
        }

        // Return the amount of message references that were found in this method.
        return (outCount, inCount);
    }
    #endregion

    #region Structure Extraction
    private static ASClass? GetMessageParser(ASClass messageClass)
    {
        ABCFile abc = messageClass.ABC;
        ASInstance instance = messageClass.Instance;

        ASInstance superInstance = abc.GetInstance(instance.Super);
        if (superInstance == null) superInstance = instance;

        ASMethod? parserGetterMethod = superInstance.GetGetter("parser")?.Method;
        if (parserGetterMethod == null) return null;

        IEnumerable<ASMethod> methods = instance.GetMethods();
        foreach (ASMethod method in methods.Concat(new[] { instance.Constructor }))
        {
            ASCode code = method.Body.ParseCode();
            foreach (ASInstruction instruction in code)
            {
                ASMultiname? multiname = null;
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
    private static ASMultiname? GetTraitType(ASContainer? container, ASMultiname traitName)
    {
        return container == null
            ? traitName
            : (container.GetTraits(TraitKind.Slot, TraitKind.Constant, TraitKind.Getter).Where(t => t.QName == traitName).FirstOrDefault()?.Type);
    }
    private static bool TryGetStructurePiece(bool isOutgoing, ASMultiname? multiname, ASClass? @class, out char piece)
    {
        ASMultiname? returnValueType = multiname;
        if (@class != null && multiname != null)
        {
            returnValueType = GetTraitType(@class, multiname) ?? GetTraitType(@class.Instance, multiname);
        }

        if (returnValueType == null)
        {
            piece = char.MinValue;
            return false;
        }

        switch (returnValueType.Name.ToLower())
        {
            case "int":
            case "readint":
            case "readinteger":
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
                if (!isOutgoing && !IsValidIdentifier(returnValueType.Name))
                {
                    piece = 'i'; // This reference call is most likely towards 'readInt'
                }
                else piece = char.MinValue;
                break;
            }
        }
        return piece != char.MinValue;
    }

    private static string? GetOutgoingStructure(ASClass messageClass, ASCode code, Local getLocal)
    {
        string? structure = null;
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
                    classToCheck = classToCheck.ABC.GetClass(getLex.TypeName);
                }

                if (!TryGetStructurePiece(true, propertyName, classToCheck, out char piece)) return null;
                structure += piece;
            }
        }
        return structure;
    }
    private static string? GetOutgoingStructure(ASClass messageClass, ASCode code, ASInstruction beforeReturn, int length)
    {
        var getLocalEndIndex = -1;
        int pushingEndIndex = code.IndexOf(beforeReturn);

        ASMultiname? propertyName = null;
        var structure = new char[length];
        var pushedLocals = new Dictionary<int, int>();
        for (int i = pushingEndIndex - 1; i >= 0; i--)
        {
            ASInstruction instruction = code[i];
            if (instruction.OP == OPCode.PushByte)
            {
                structure[--length] = 'b';
                continue;
            }
            if (instruction.OP == OPCode.GetProperty)
            {
                propertyName = ((GetPropertyIns)instruction).PropertyName;
            }
            else if (instruction.OP == OPCode.GetLex)
            {
                propertyName = ((GetLexIns)instruction).TypeName;
            }

            if (propertyName != null)
            {
                ASClass classToCheck = messageClass;
                ASInstruction previous = code[i - 1];
                if (previous.OP == OPCode.GetLex)
                {
                    var getLex = (GetLexIns)previous;
                    classToCheck = classToCheck.ABC.GetClass(getLex.TypeName);

                    if (classToCheck != null) i--;
                }

                if (!TryGetStructurePiece(true, propertyName, classToCheck, out char piece)) return null;
                structure[--length] = piece;
                propertyName = null;
            }
            else if (Local.IsGetLocal(instruction.OP) && instruction.OP != OPCode.GetLocal_0)
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
                    _ => throw new NotSupportedException($"Don't know what this value type is, tell someone about this please.\r\nOP: {beforeSet.OP}"),
                };
            }
            if (pushedLocals.Count == 0) break;
        }
        return new string(structure);
    }

    private string? GetIncomingStructure(ASClass @class)
    {
        ASMethod parseMethod = @class.Instance.GetMethod("parse", "Boolean", 1);
        return GetIncomingStructure(@class.Instance, parseMethod);
    }
    private string? GetIncomingStructure(ASInstance instance, ASMethod method)
    {
        if (method.Body.Exceptions.Count > 0) return null;

        ASCode code = method.Body.ParseCode();
        if (code.JumpExits.Count > 0 || code.SwitchExits.Count > 0) return null;

        string? structure = null;
        ABCFile abc = method.ABC;
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
                        ASMultiname? propertyName = null;
                        ASInstruction previous = code[i - 2];

                        switch (previous.OP)
                        {
                            case OPCode.FindPropStrict:
                            {
                                var findPropStrict = (FindPropStrictIns)previous;
                                propertyName = findPropStrict.PropertyName;
                                break;
                            }
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

                        ASInstance innerInstance = abc.GetInstance(propertyName) ?? instance;
                        ASMethod innerMethod = innerInstance.GetMethod(callProperty.PropertyName.Name, null, callProperty.ArgCount);
                        if (innerMethod == null)
                        {
                            ASClass innerClass = abc.GetClass(propertyName);
                            innerMethod = innerClass.GetMethod(callProperty.PropertyName.Name, null, callProperty.ArgCount);
                        }

                        string? innerStructure = GetIncomingStructure(innerInstance, innerMethod);
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

                    string? innerStructure = GetIncomingStructure(innerInstance, innerInstance.Constructor);
                    if (string.IsNullOrWhiteSpace(innerStructure)) return null;
                    structure += innerStructure;
                    break;
                }

                case OPCode.ConstructSuper:
                {
                    ASInstance superInstance = abc.GetInstance(instance.Super);

                    string? innerStructure = GetIncomingStructure(superInstance, superInstance.Constructor);
                    if (string.IsNullOrWhiteSpace(innerStructure)) return null;
                    structure += innerStructure;
                    break;
                }

                case OPCode.CallSuper:
                {
                    var callSuper = (CallSuperIns)next;
                    ASInstance superInstance = abc.GetInstance(instance.Super);
                    ASMethod superMethod = superInstance.GetMethod(callSuper.MethodName.Name, null, callSuper.ArgCount);

                    string? innerStructure = GetIncomingStructure(superInstance, superMethod);
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

    private string? GetOutgoingStructure(ASClass messageClass, ASClass @class)
    {
        ASMethod getArrayMethod = @class.Instance.GetMethod(null, "Array", 0);
        if (getArrayMethod == null)
        {
            ASClass superClass = @class.ABC.GetClass(@class.Instance.Super);
            return GetOutgoingStructure(messageClass, superClass);
        }
        if (getArrayMethod.Body.Exceptions.Count > 0) return null;
        ASCode getArrayCode = getArrayMethod.Body.ParseCode();

        if (getArrayMethod.ReturnType == null || getArrayCode.JumpExits.Count > 0 || getArrayCode.SwitchExits.Count > 0)
        {
            // TODO: Parse for reading loops/jumps.
            return null;
        }

        ASInstruction? resultPusher = null;
        for (int i = getArrayCode.Count - 1; i >= 0; i--)
        {
            ASInstruction instruction = getArrayCode[i];
            if (instruction.OP == OPCode.ReturnValue)
            {
                resultPusher = getArrayCode[i - 1];
                break;
            }
        }
        if (resultPusher == null) return null;

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
    private string? GetOutgoingStructure(ASClass @class, ASMultiname? propertyName)
    {
        ASMethod constructor = @class.Instance.Constructor;
        if (constructor.Body.Exceptions.Count > 0) return null;

        ASCode code = constructor.Body.ParseCode();
        if (code.JumpExits.Count > 0 || code.SwitchExits.Count > 0) return null;

        string? structure = null;
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
                    ASClass superClass = @class.ABC.GetClass(@class.Instance.Super);
                    structure += GetOutgoingStructure(superClass, propertyName);
                }
            }
            if (instruction.OP != OPCode.GetProperty) continue;

            var getProperty = (GetPropertyIns)instruction;
            if (getProperty.PropertyName != propertyName) continue;

            ASInstruction next = code[++i];
            ASClass? classToCheck = @class;
            if (Local.IsGetLocal(next.OP))
            {
                if (next.OP == OPCode.GetLocal_0) continue;

                var local = (Local)next;
                ASParameter parameter = constructor.Parameters[local.Register - 1];

                if (!TryGetStructurePiece(true, parameter.Type, null, out char piece)) return null;
                structure += piece;
            }
            else if (Primitive.IsValid(next.OP) && code[i + 1].OP == OPCode.CallPropVoid) // Could it be 'push(pop())'?
            {
                switch (next.OP)
                {
                    case OPCode.PushByte: structure += 'B'; break;
                    default: break;
                }
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
                    classToCheck = classToCheck.ABC.GetClass(getLex.TypeName);
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
                while (next.OP is not OPCode.GetProperty and not OPCode.CallProperty);

                if (!TryGetStructurePiece(true, propertyName, classToCheck, out char piece)) return null;
                structure += piece;
            }
        }
        return structure;
    }
    #endregion

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
        if (IsDisposed) return;
        if (disposing)
        {
            _flash.Dispose();
            _abcFileTags.Clear();
            _flashMessagesByHash.Clear();
            _flashMessagesByClassName.Clear();
            GC.Collect();
        }
        _scSendCode = _scSendUnencryptedCode = null;
        _scSendMethod = _scSendUnencryptedMethod = null;
        _hcmNextPortMethod = _hcmUpdateHostParametersMethod = null;
        _socketConnectionInstance = _habboCommunicationDemoInstance = _habboCommunicationManagerInstance = null;
        IsDisposed = true;
    }
}