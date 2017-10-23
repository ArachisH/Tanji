using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using FlashInspect;
using FlashInspect.IO;
using FlashInspect.ActionScript;
using FlashInspect.ActionScript.Traits;
using FlashInspect.ActionScript.Multinames;
using FlashInspect.ActionScript.Instructions;

namespace Tangine.Habbo
{
    /// <summary>
    ///  Represents the Habbo Hotel flash client.
    /// </summary>
    public class HGame : ShockwaveFlash
    {
        private string _clientRevision;
        private Dictionary<ASClass, ushort> _outgoingHeaders, _incomingHeaders;

        private readonly Dictionary<ASClass, string> _messageHashes;
        private readonly Dictionary<string, List<ASClass>> _messages;
        private readonly Dictionary<ASClass, bool> _isMessageOutgoing;
        private readonly Dictionary<ASClass, ASClass> _messageParsers;
        private readonly Dictionary<ASClass, List<ASReference>> _messageReferences;

        /// <summary>
        /// Gets or sets the delegate to be called when a log has been written.
        /// </summary>
        public Action<string> LoggerCallback { get; set; }

        /// <summary>
        /// Gets the dictionary of Outgoing message classes with the header as the key.
        /// </summary>
        public ReadOnlyDictionary<ushort, ASClass> OutgoingMessages { get; private set; }
        /// <summary>
        /// Gets the dictionary of Incoming message classes with the header as they key.
        /// </summary>
        public ReadOnlyDictionary<ushort, ASClass> IncomingMessages { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HGame"/> class based on the specified file path.
        /// </summary>
        /// <param name="path">The file path of the Shockwave Flash(SWF) file.</param>
        public HGame(string path)
            : this(File.ReadAllBytes(path))
        {
            Location = Path.GetFullPath(path);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="HGame"/> class based on the specified array of bytes.
        /// </summary>
        /// <param name="data">The array of bytes representing the Shockwave Flash(SWF) file.</param>
        public HGame(byte[] data)
            : base(data)
        {
            _messageHashes = new Dictionary<ASClass, string>();
            _isMessageOutgoing = new Dictionary<ASClass, bool>();
            _messageParsers = new Dictionary<ASClass, ASClass>();
            _messages = new Dictionary<string, List<ASClass>>();
            _messageReferences = new Dictionary<ASClass, List<ASReference>>();
        }

        /// <summary>
        /// Modifies the bytecode to allow the client to run from anywhere.
        /// </summary>
        public void BypassOriginCheck()
        {
            ASMethod originCheck1 = ABCFiles[0].Classes[0].FindFirstMethod(null, "Boolean");
            if (originCheck1 == null) return;

            ASClass habboClass = ABCFiles[1].FindFirstClassByName("Habbo");
            if (habboClass == null) return;

            ASMethod originCheck2 = habboClass
                .Instance.FindFirstMethod(null, "Boolean");

            if (originCheck2 == null) return;
            if (originCheck2.Parameters.Count == 0) return;
            if (originCheck2.Parameters[0].Type.Name != "String") return;

            var instructions = new OPCode[] { OPCode.PushTrue, OPCode.ReturnValue };
            PrependOperations(originCheck1, instructions);
            PrependOperations(originCheck2, instructions);
            return;
        }
        /// <summary>
        /// Modifies the bytecode to allow the client to connect to anywhere.
        /// </summary>
        public void BypassRemoteHostCheck()
        {
            ABCFile abc = ABCFiles[2];
            ASInstance habboCommMngr = abc.FindFirstInstanceByName("HabboCommunicationManager");
            if (habboCommMngr == null) return;

            int hostValueSlotObjTypeIndex = -1;
            string hostValueSlotObjName = string.Empty;
            foreach (ASTrait trait in habboCommMngr.Traits)
            {
                if (trait.TraitType != TraitType.Slot) continue;
                if (((SlotConstantTrait)trait.Data).TypeName.Name != "String") continue;

                hostValueSlotObjName = trait.Name.Name;
                hostValueSlotObjTypeIndex = trait.NameIndex;
                break;
            }

            ASMethod initCompMethod = habboCommMngr
                .FindFirstMethod("initComponent", "void");

            int getPropertyObjTypeIndex = abc.Constants
                .IndexOfMultiname("getProperty");

            ASMethod initConnectionMethod = null;
            using (var outCode = new FlashWriter())
            using (var inCode = new FlashReader(initCompMethod.Body.Bytecode))
            {
                object[] values = inCode.ReadValuesUntil(OPCode.CallPropVoid, null, 0);

                if (values == null) return;
                CopyBytecode(inCode, outCode, 0, inCode.Position);

                outCode.WriteOP(OPCode.GetLocal_0);
                outCode.WriteOP(OPCode.FindPropStrict, getPropertyObjTypeIndex);
                outCode.WriteOP(OPCode.PushString, abc.Constants.AddString("connection.info.host"));
                outCode.WriteOP(OPCode.CallProperty, getPropertyObjTypeIndex, 1);
                outCode.WriteOP(OPCode.InitProperty, hostValueSlotObjTypeIndex);
                WriteLog($"Method '{initCompMethod}' modified to include '{hostValueSlotObjName} = getProperty(\"connection.info.host\");'.");

                CopyBytecode(inCode, outCode);
                initCompMethod.Body.Bytecode = outCode.ToArray();

                values = inCode.ReadValuesUntil(OPCode.CallPropVoid);
                ASMultiname callPropVoidType = abc.Constants.Multinames[(int)values[0]];
                initConnectionMethod = habboCommMngr.FindFirstMethod(callPropVoidType.Name, "void");
            }

            using (var outCode = new FlashWriter())
            using (var inCode = new FlashReader(initConnectionMethod.Body.Bytecode))
            {
                int ifNeCount = 0;
                int byteJumpCountPos = 0;
                int differenceOffset = 0;
                uint byteJumpCountValue = 0;
                int magicNumberIndex = abc.Constants.AddInteger(65290);
                while (inCode.IsDataAvailable)
                {
                    OPCode op = inCode.ReadOP();
                    object[] values = null;

                    if (op != OPCode.PushInt)
                    {
                        values = inCode.ReadValues(op);
                        outCode.WriteOP(op, values);

                        if (op == OPCode.IfNe &&
                            (++ifNeCount == 2 || ifNeCount == 4))
                        {
                            byteJumpCountPos = (outCode.Position - 3);
                            byteJumpCountValue = (uint)values[0];
                        }
                        continue;
                    }

                    bool isFinalPushInt = false;
                    int pushIntIndex = inCode.Read7BitEncodedInt();
                    int pushIntValue = abc.Constants.Integers[pushIntIndex];
                    #region Switch: pushIntValue
                    switch (pushIntValue)
                    {
                        case 65244: //97
                        case 65185: //32
                        case 65191: //175
                        case 65189: //123
                        case 65188: //164
                        case 65174: //45
                        case 65238: //297
                        case 65184: //127
                        case 65171: //20
                        case 65172: //58
                        {
                            pushIntIndex = magicNumberIndex;
                            isFinalPushInt = (pushIntValue == 65172);
                            break;
                        }
                    }
                    #endregion
                    outCode.WriteOP(op, pushIntIndex);

                    int byteDifference = (((inCode.Position - outCode.Length) * -1) - differenceOffset);
                    if (isFinalPushInt)
                    {
                        int curPos = outCode.Position;
                        differenceOffset += byteDifference;

                        outCode.Position = byteJumpCountPos;
                        outCode.WriteS24(byteJumpCountValue + (uint)byteDifference);
                        outCode.Position = curPos;

                        if (ifNeCount == 4)
                        {
                            CopyBytecode(inCode, outCode);
                            initConnectionMethod.Body.Bytecode = outCode.ToArray();
                            WriteLog($"Method '{initConnectionMethod}' modified to not append suffix to the host value.");
                            return;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Generates, and caches a unique MD5 hash for every Outgoing/Incoming message class.
        /// </summary>
        public void GenerateMessageHashes()
        {
            foreach (ASClass msgClass in OutgoingMessages.Values)
                GetMessageHash(msgClass);

            foreach (ASClass msgClass in IncomingMessages.Values)
                GetMessageHash(msgClass);
        }
        /// <summary>
        /// Injects the specified public RSA keys into the bytecode that handles the verification of the received primes.
        /// </summary>
        /// <param name="exponent">The public exponent.</param>
        /// <param name="modulus">The public modulus.</param>
        public void ReplaceRSAKeys(int exponent, string modulus)
        {
            ABCFile abc = ABCFiles[2];
            ASInstance habboCommDemoInstance = abc.FindFirstInstanceByName("HabboCommunicationDemo");

            IEnumerable<MethodGetterSetterTrait> mgsTraits =
                habboCommDemoInstance.FindMethodGetterSetterTraits();

            ASMethod method = null;
            int rsaKeyTypeIndex = abc.Constants.IndexOfMultiname("RSAKey");
            foreach (MethodGetterSetterTrait mgsTrait in mgsTraits)
            {
                if (mgsTrait.Method.ReturnType.Name != "void") continue;
                if (mgsTrait.Method.Parameters.Count != 1) continue;

                if (ContainsOperation(mgsTrait.Method, OPCode.GetLex, rsaKeyTypeIndex))
                {
                    method = mgsTrait.Method;
                    WriteLog($"Found reference to 'RSAKey' in method '{method}'.");
                    break;
                }
            }

            using (var outCode = new FlashWriter())
            using (var inCode = new FlashReader(method.Body.Bytecode))
            {
                int modulusStringIndex = abc.Constants.AddString(modulus);
                int exponentStringIndex = abc.Constants.AddString(exponent.ToString("x")); // Turn the number to hex, remeber guys, (65537= 10001(hex))
                int keyObfuscatorTypeIndex = abc.Constants.IndexOfMultiname("KeyObfuscator");

                // Replace the first 'GetLex[KeyObfuscator]' operation with 'PushString[modulus]'.
                ReplaceNextOperation(inCode, outCode, method,
                    OPCode.GetLex, new object[] { keyObfuscatorTypeIndex },
                    OPCode.PushString, new object[] { modulusStringIndex });

                // Ignore these operations, do not write.
                inCode.ReadValuesUntil(OPCode.CallProperty);

                // Replace the second 'GetLex[KeyObfuscator]' operation with 'PushString[exponent]'.
                ReplaceNextOperation(inCode, outCode, method,
                    OPCode.GetLex, new object[] { keyObfuscatorTypeIndex },
                    OPCode.PushString, new object[] { exponentStringIndex });

                // Ignore these operations, do not write.
                inCode.ReadValuesUntil(OPCode.CallProperty);

                CopyBytecode(inCode, outCode);
                method.Body.Bytecode = outCode.ToArray();
            }
        }

        /// <summary>
        /// Returns a value that determines whether the specified class is associated with an Outgoing/Incoming header.
        /// </summary>
        /// <param name="asClass">The class to check whether it's an Outgoing/Incoming message class.</param>
        /// <returns></returns>
        public bool IsMessageClass(ASClass asClass)
        {
            return _isMessageOutgoing.ContainsKey(asClass);
        }
        /// <summary>
        /// Returns a value that determines whether the specified message class is an Outgoing type message class.
        /// </summary>
        /// <param name="messageClass">The message class to check whether it is an Outoing message class type.</param>
        /// <returns></returns>
        public bool IsMessageOutgoing(ASClass messageClass)
        {
            if (_isMessageOutgoing.ContainsKey(messageClass))
                return _isMessageOutgoing[messageClass];

            return false;
        }
        /// <summary>
        /// Returns a value that determines whether the specified message class is used/referenced in the bytecode.
        /// </summary>
        /// <param name="messageClass">The message class to check whether it is being referenced.</param>
        /// <returns></returns>
        public bool IsMessageReferenced(ASClass messageClass)
        {
            return _messageReferences.ContainsKey(messageClass);
        }

        /// <summary>
        /// Returns the hardcoded client revision string from the Outgoing[4000] message class.
        /// </summary>
        /// <returns></returns>
        public string GetClientRevision()
        {
            if (!string.IsNullOrWhiteSpace(_clientRevision))
                return _clientRevision;

            if (!(OutgoingMessages?.ContainsKey(4000) ?? false))
                return string.Empty;

            ASInstance outgoingInstance = OutgoingMessages[4000].Instance;
            ASMethod method = outgoingInstance.FindFirstMethod(null, "Array");
            if (method == null) return string.Empty;

            using (var inCode = new FlashReader(method.Body.Bytecode))
            {
                object[] values = inCode.ReadValuesUntil(OPCode.PushString);
                var pushStringIndex = (int)values[0];

                _clientRevision = method.ABC.Constants.Strings[pushStringIndex];
            }
            return _clientRevision;
        }
        /// <summary>
        /// Returns the Outgoing message class associated with the specified header.
        /// </summary>
        /// <param name="header">The header associated with the Outgoing message class.</param>
        /// <returns></returns>
        public ASClass GetOutgoingMessage(ushort header)
        {
            return GetMessage(header, true);
        }
        /// <summary>
        /// Returns the Incoming message class associated with the specified header.
        /// </summary>
        /// <param name="header">The header associated with the Incoming message class.</param>
        /// <returns></returns>
        public ASClass GetIncomingMessage(ushort header)
        {
            return GetMessage(header, false);
        }
        /// <summary>
        /// Returns a unique MD5 hash generated from various information contained in the message class.
        /// </summary>
        /// <param name="messageClass">The Outgoing/Incoming message class to create the unique hash from.</param>
        /// <returns></returns>
        public string GetMessageHash(ASClass messageClass)
        {
            if (!IsMessageClass(messageClass))
            {
                throw new ArgumentException(
                    "The specified class is not a valid Outgoing/Incoming message class.", nameof(messageClass));
            }
            if (_messageHashes.ContainsKey(messageClass))
            {
                return _messageHashes[messageClass];
            }
            else if (_messageReferences.Count == 0)
            {
                FindMessageReferences();
            }
            using (var hashOut = new FlashHasher())
            {
                hashOut.IsSummarizing = true;
                bool isOutgoing = IsMessageOutgoing(messageClass);

                hashOut.Write(isOutgoing);
                Write(hashOut, messageClass, isOutgoing);
                if (!messageClass.Instance.Name.Name.EndsWith("Composer") &&
                    _messageReferences.ContainsKey(messageClass))
                {
                    List<ASReference> msgReferences = _messageReferences[messageClass];
                    hashOut.Write(msgReferences.Count);
                    foreach (ASReference msgReference in msgReferences)
                    {
                        if (!isOutgoing)
                        {
                            string fromClassName =
                                msgReference.FromClass.Instance.Name.Name;

                            if (fromClassName == "IncomingMessages")
                                hashOut.Write(fromClassName);
                        }

                        hashOut.Write(msgReference.Id);
                        hashOut.Write(msgReference.FromMethod, true);

                        byte[] bytecode = msgReference.FromMethod.Body.Bytecode;
                        using (var inCode = new FlashReader(bytecode))
                        {
                            int lineCount = 0;
                            while (inCode.IsDataAvailable)
                            {
                                OPCode op = inCode.ReadOP();
                                object[] values = inCode.ReadValues(op); // Avoid common integer values, by starting big...
                                if (op == OPCode.DebugLine) hashOut.Write(int.MaxValue - (++lineCount));
                            }
                        }
                    }
                }

                string hash = hashOut.ToString();
                if (!_messages.ContainsKey(hash))
                    _messages[hash] = new List<ASClass>();

                if (!_messages[hash].Contains(messageClass))
                    _messages[hash].Add(messageClass);

                _messageHashes[messageClass] = hash;
                return hash;
            }
        }
        /// <summary>
        /// Returns the header associated with the specified Outgoing/Incoming message class.
        /// </summary>
        /// <param name="messageClass">The Outgoing/Incoming message class to grab the associated header from.</param>
        /// <returns></returns>
        public ushort GetMessageHeader(ASClass messageClass)
        {
            if (!IsMessageClass(messageClass))
                throw new ArgumentException("The specified class is not a valid Outgoing/Incoming message class.", nameof(messageClass));

            if (IsMessageOutgoing(messageClass))
                return _outgoingHeaders[messageClass];
            else
                return _incomingHeaders[messageClass];
        }
        /// <summary>
        /// Returns a read-only list of message classes associated with the specified unique MD5 hash.
        /// </summary>
        /// <param name="hash">The unique MD5 hash associated with the message(s).</param>
        /// <returns></returns>
        public IReadOnlyList<ASClass> GetMessages(string hash)
        {
            if (_messages.ContainsKey(hash))
                return _messages[hash].AsReadOnly();

            return null;
        }
        /// <summary>
        /// Returns a read-only list of headers associated with the specified unique MD5 hash.
        /// </summary>
        /// <param name="hash">The unique MD5 hash associated with the header(s).</param>
        /// <returns></returns>
        public IReadOnlyList<ushort> GetMessageHeaders(string hash)
        {
            IReadOnlyList<ASClass> messages = GetMessages(hash);
            if (messages == null) return null;

            var headers = new List<ushort>(messages.Count);
            foreach (ASClass message in messages)
                headers.Add(GetMessageHeader(message));

            return headers.AsReadOnly();
        }
        /// <summary>
        /// Returns the Incoming message's parser class.
        /// </summary>
        /// <param name="messageClass">The Incoming message class to extract the parser class from.</param>
        /// <returns></returns>
        public ASClass GetIncomingMessageParser(ASClass messageClass)
        {
            if (_messageParsers.ContainsKey(messageClass))
                return _messageParsers[messageClass];

            ABCFile abc = messageClass.ABC;
            ASClass incomingMsgParserClass = null;
            ASInstance incomingMsgInstance = messageClass.Instance;
            try
            {
                ASInstance incomingMsgSuperInstance = abc.FindFirstInstanceByName(
                    incomingMsgInstance.SuperType.Name);

                ASMultiname parserReturnType = incomingMsgSuperInstance
                    .FindFirstGetter("parser", null).ReturnType;

                List<ASMethod> methods =
                    incomingMsgInstance.FindMethodGetterSetterTraits()
                    .Select(mgsTrait => mgsTrait.Method).ToList();

                methods.Add(incomingMsgInstance.Constructor);
                foreach (ASMethod method in methods)
                {
                    var referencedClasses = new List<ASClass>();
                    using (var inCode = new FlashReader(method.Body.Bytecode))
                    {
                        while (inCode.IsDataAvailable)
                        {
                            OPCode op = 0;
                            object[] values = inCode.ReadValuesUntilEither(out op, OPCode.FindPropStrict, OPCode.GetLex);
                            if (values == null) break;

                            var typeIndex = (int)values[0];
                            ASMultiname type = abc.Constants.Multinames[typeIndex];

                            List<ASClass> instances = abc.FindClassesByName(type.Name);
                            referencedClasses.AddRange(instances);
                        }
                    }
                    foreach (ASClass referencedClass in referencedClasses)
                    {
                        ASInstance referencedInstance = referencedClass.Instance;
                        if (referencedInstance.ContainsInterface(parserReturnType.Name))
                        {
                            incomingMsgParserClass = referencedClass;
                            return incomingMsgParserClass;
                        }
                    }
                }
            }
            finally
            {
                if (incomingMsgParserClass != null)
                {
                    _messageParsers[messageClass] =
                        incomingMsgParserClass;
                }
            }
            return incomingMsgParserClass;
        }
        /// <summary>
        /// Returns an enumerable containing references for the specified message class.
        /// </summary>
        /// <param name="messageClass">The message class being referenced.</param>
        /// <returns></returns>
        public IEnumerable<ASReference> GetMessageReferences(ASClass messageClass)
        {
            if (_messageReferences.ContainsKey(messageClass))
                return _messageReferences[messageClass];

            return null;
        }

        #region Protected Methods
        protected void WriteLog(string log)
        {
            LoggerCallback?.Invoke(log);
        }
        protected void Write(FlashHasher hashOut, ASClass messageClass, bool isOutgoing)
        {
            if (isOutgoing)
            {
                string messageName = messageClass.Instance.Name.Name;
                string messageSuperName = messageClass.Instance.SuperType.Name;
                if (messageName.EndsWith("Composer"))
                {
                    hashOut.Write(messageName);
                    return;
                }
                else if (messageSuperName.EndsWith("Composer"))
                {
                    hashOut.Write(messageSuperName);
                    hashOut.Write(messageClass, false);
                    return;
                }
            }
            else
            {
                ASClass incomingMsgParser =
                    GetIncomingMessageParser(messageClass);

                if (incomingMsgParser == null)
                    throw new NullReferenceException(nameof(incomingMsgParser));

                string parserObjName = incomingMsgParser.Instance.Name.Name;
                if (parserObjName.EndsWith("Parser"))
                    hashOut.Write(parserObjName);

                hashOut.Write(incomingMsgParser, true);
            }
            hashOut.Write(messageClass, true);
        }

        protected virtual ASClass GetMessage(ushort header, bool isOutgoing)
        {
            ReadOnlyDictionary<ushort, ASClass> messages =
                (isOutgoing ? OutgoingMessages : IncomingMessages);

            if (messages.ContainsKey(header))
                return messages[header];

            throw new ArgumentException(
                $"The specified header is not associated with any {(isOutgoing ? "Outgoing" : "Incoming")} message class.", nameof(header));
        }
        protected void FindHabboMessageClasses(ASClass habboMessageClass)
        {
            if (habboMessageClass == null)
                throw new NullReferenceException(nameof(habboMessageClass));

            if (OutgoingMessages != null && IncomingMessages != null)
                return;

            ABCFile abc = habboMessageClass.ABC;
            if (habboMessageClass.Traits.Count < 2) return;

            int incomingMapTypeIndex = habboMessageClass.Traits[0].NameIndex;
            int outgoingMapTypeIndex = habboMessageClass.Traits[1].NameIndex;

            var outgoingClasses = new Dictionary<ushort, ASClass>();
            var incomingClasses = new Dictionary<ushort, ASClass>();

            ASMethod constructor = habboMessageClass.Constructor;
            using (var inCode = new FlashReader(constructor.Body.Bytecode))
            {
                while (inCode.IsDataAvailable)
                {
                    object[] values = inCode.ReadValuesUntil(OPCode.GetLex);
                    if (values == null) break;

                    // Check if the instruction is referencing one of the in/out map types.
                    int getLexTypeIndex = (int)values[0];
                    bool isOutgoing = (getLexTypeIndex == outgoingMapTypeIndex);
                    if (!isOutgoing && getLexTypeIndex != incomingMapTypeIndex) continue;

                    OPCode op = 0;
                    values = inCode.ReadValuesUntilEither(out op, OPCode.PushByte, OPCode.PushShort);

                    if (values == null) return;
                    var header = Convert.ToUInt16(values[0]);

                    values = inCode.ReadValuesUntil(OPCode.GetLex);
                    if (values == null) return;

                    getLexTypeIndex = (int)values[0];
                    ASMultiname messageType = abc.Constants.Multinames[getLexTypeIndex];
                    ASClass messageClass = abc.FindFirstClassByName(messageType.Name);

                    Dictionary<ushort, ASClass> messageClasses =
                        (isOutgoing ? outgoingClasses : incomingClasses);

                    messageClasses[header] = messageClass;
                    _isMessageOutgoing[messageClass] = isOutgoing;
                }
            }

            #region Organize Outgoing Message Dictionaries
            IOrderedEnumerable<KeyValuePair<ushort, ASClass>> orderedOutgoing =
                outgoingClasses.OrderBy(kvp => kvp.Key);

            _outgoingHeaders = orderedOutgoing
                .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

            OutgoingMessages = new ReadOnlyDictionary<ushort, ASClass>(
                orderedOutgoing.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            #endregion
            #region Organize Incoming Message Dictionaries
            IOrderedEnumerable<KeyValuePair<ushort, ASClass>> orderedIncoming =
                incomingClasses.OrderBy(kvp => kvp.Key);

            _incomingHeaders = orderedIncoming
                .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

            IncomingMessages = new ReadOnlyDictionary<ushort, ASClass>(
                orderedIncoming.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            #endregion
        }

        protected void FindMessageReferences()
        {
            ABCFile abc = ABCFiles[2];
            WriteLog($"Searching {abc.Methods.Count:n0} methods for references to Outgoing({OutgoingMessages.Count})/Incoming({IncomingMessages.Count}) messages...");
            foreach (ASClass asClass in abc.Classes)
            {
                ASInstance asInstance = asClass.Instance;
                if (asInstance.Flags.HasFlag(ClassFlags.Interface)) continue;

                int referencesFound = 0;
                referencesFound += FindMessageReferences(asClass, asClass, asClass.Constructor, -1, 0);
                referencesFound += FindMessageReferences(asClass, asClass.Instance, asClass.Instance.Constructor, -2, 0);

                referencesFound += FindMessageReferences(asClass, asClass);
                referencesFound += FindMessageReferences(asClass, asClass.Instance);
            }   // What to do with the total message reference count in a class?

            int usedOutMsgs = _messageReferences.Count(msgClass => _isMessageOutgoing[msgClass.Key]);
            int unusedOutMsgs = (OutgoingMessages.Count - usedOutMsgs);
            int usedInMsgs = (_messageReferences.Count - usedOutMsgs);
            int unusedInMsgs = (IncomingMessages.Count - usedInMsgs);

            WriteLog($@"Outgoing/Incoming message reference search complete.
Unused Outgoing messages: {unusedOutMsgs}/{OutgoingMessages.Count}
Unused Incoming messages: {unusedInMsgs}/{IncomingMessages.Count}");
        }
        protected int FindMessageReferences(ASClass asClass, TraitContainer container)
        {
            IEnumerable<MethodGetterSetterTrait> mgsTraits =
                container.FindMethodGetterSetterTraits();

            int rank = 0;
            int msgReferencesFound = 0;
            foreach (MethodGetterSetterTrait mgsTrait in mgsTraits)
            {
                msgReferencesFound +=
                    FindMessageReferences(asClass, container, mgsTrait.Method, ++rank, 0);
            }
            return msgReferencesFound;
        }
        protected int FindMessageReferences(ASClass fromClass, TraitContainer container, ASMethod fromMethod, int rank, int msgReferencesFound)
        {
            ABCFile abc = fromClass.ABC;
            using (var inCode = new FlashReader(fromMethod.Body.Bytecode))
            {
                var multinameStack = new Stack<ASMultiname>();
                while (inCode.IsDataAvailable)
                {
                    OPCode op = inCode.ReadOP();
                    object[] values = inCode.ReadValues(op);

                    switch (op)
                    {
                        #region Case: GetProperty
                        case OPCode.GetProperty:
                        {
                            multinameStack.Push(abc.Constants.Multinames[(int)values[0]]);
                            break;
                        }
                        #endregion
                        #region Case: NewFunction
                        case OPCode.NewFunction:
                        {
                            ASMethod newFuncMethod = abc.Methods[(int)values[0]];
                            msgReferencesFound = FindMessageReferences(fromClass, container, newFuncMethod, rank, msgReferencesFound);
                            break;
                        }
                        #endregion
                        #region Case: ConstructProp
                        case OPCode.ConstructProp:
                        {
                            ASMultiname constructPropType =
                                abc.Constants.Multinames[(int)values[0]];

                            ASClass messageClass =
                                abc.FindFirstClassByName(constructPropType.Name);

                            if (messageClass == null || !IsMessageClass(messageClass))
                            {
                                continue;
                            }

                            if (!_messageReferences.ContainsKey(messageClass))
                                _messageReferences[messageClass] = new List<ASReference>();

                            var msgReference = new ASReference(messageClass, fromClass, fromMethod);
                            if (!IsMessageOutgoing(messageClass))
                            {
                                ASMultiname topName = multinameStack.Pop();
                                msgReference.FromMethod = null;

                                IEnumerable<MethodGetterSetterTrait> mgsTraits =
                                    container.FindMethodGetterSetterTraits();

                                rank = 0; // TODO: Move this into a method or something, I'm re-writing it below as well.
                                foreach (MethodGetterSetterTrait mgsTrait in mgsTraits)
                                {
                                    rank++;
                                    if (mgsTrait.Method.TraitName == topName.Name)
                                    {
                                        msgReference.FromMethod = mgsTrait.Method;
                                        break;
                                    }
                                }
                                if (msgReference.FromMethod == null && multinameStack.Count > 0)
                                {
                                    ASMultiname bottomName = multinameStack.Pop();
                                    foreach (ASTrait trait in container.Traits)
                                    {
                                        switch (trait.TraitType)
                                        {
                                            #region Case: Slot, Constant
                                            case TraitType.Slot:
                                            case TraitType.Constant:
                                            {
                                                if (trait.Name.Name != bottomName.Name) continue;

                                                var scTrait = (SlotConstantTrait)trait.Data;
                                                if (scTrait.TypeName.MultinameType != ConstantType.QName) continue;
                                                ASClass slotValueClass = abc.FindFirstClassByName(scTrait.TypeName.Name);

                                                rank = 0;
                                                mgsTraits = slotValueClass.Instance.FindMethodGetterSetterTraits();
                                                foreach (MethodGetterSetterTrait mgsTrait in mgsTraits)
                                                {
                                                    rank++;
                                                    if (mgsTrait.Method.TraitName == topName.Name)
                                                    {
                                                        msgReference.FromMethod = mgsTrait.Method;
                                                        break;
                                                    }
                                                }

                                                if (msgReference.FromMethod != null)
                                                    msgReference.FromClass = slotValueClass;

                                                break;
                                            }
                                            #endregion
                                        }
                                        if (msgReference.FromMethod != null) break;
                                    }
                                }
                            }

                            msgReference.Id = ((++msgReferencesFound) + rank);
                            // We can't rely on the amount of references found, since the hooking of incoming messages are randomized each revision.
                            if (!IsMessageOutgoing(messageClass))
                                msgReference.Id = rank;

                            _messageReferences[messageClass].Add(msgReference);
                            break;
                        }
                        #endregion
                    }
                }
            }
            return msgReferencesFound;
        }

        protected void FixLocalRegisters(ABCFile abc)
        {
            foreach (ASMethod method in abc.Methods)
                FixLocalRegisters(method);
        }
        protected void FixLocalRegisters(ASMethod method)
        {
            // But, what about nested branches? Does not work with those yet, sadly.
            if (method.Body == null) return;
            if (method.Body.LocalCount == (1 + method.Parameters.Count)) return;

            ABCFile abc = method.ABC;
            using (var outCode = new FlashWriter())
            using (var inCode = new FlashReader(method.Body.Bytecode))
            {
                uint jumpValue = 0;
                int jumpValueEnd = 0;
                int jumpValueStart = 0;
                int totalDifference = 0;
                bool isJumpingOver = false;
                var labelPositions = new Stack<int>();
                while (inCode.IsDataAvailable)
                {
                    if (isJumpingOver && inCode.Position >= jumpValueEnd)
                        isJumpingOver = false;

                    OPCode op = inCode.ReadOP();
                    object[] values = inCode.ReadValues(op);
                    if (op != OPCode.Debug || ((byte)values[0] != 1))
                    {
                        if (IsJumpInstruction(op)) // If a label position is contained in the stack, the next jump instruction will go backwards.
                        {
                            // Not a backwards jump, also check to make sure the jump value isn't crazy big, otherwise it might be a reverse jump instruction.
                            // If large: (uint.MaxValue - value[0]) == Bytes to jump back to get to the last saved label position in stack.
                            if (labelPositions.Count == 0 || ((uint)values[0] <= inCode.Length))
                            {
                                isJumpingOver = true;
                                jumpValue = (uint)values[0]; // To later check if we're in the middle of the jumping range.
                                jumpValueStart = (outCode.Position + 1); // To re-write the jump value if the data in-between has sgrown.
                                jumpValueEnd = (int)(inCode.Position + jumpValue);
                            }
                            else
                            {
                                // Find by how many bytes we need to move back from current position.
                                values[0] = (uint)(uint.MaxValue -
                                    ((outCode.Position + 4) - labelPositions.Pop()));
                            }
                        }
                        else if (op == OPCode.Label) // Store label positions, to easily determine by how many bytes we need to jump back to this position.
                            labelPositions.Push(outCode.Position + 1);

                        outCode.WriteOP(op, values);
                        continue;
                    }

                    var local = (byte)values[2];
                    if (local > method.Parameters.Count)
                    {
                        values[1] = abc.Constants
                            .AddString("local" + local);
                    }
                    outCode.WriteOP(op, values);

                    int difference = (((inCode.Position - outCode.Length) * -1) - totalDifference);
                    totalDifference += difference;

                    if (isJumpingOver)
                    {
                        int curPos = outCode.Position;
                        outCode.Position = jumpValueStart;
                        outCode.WriteS24(jumpValue += (uint)difference); // PlusEqual the value, in-case we've already modifed within the jumping range.
                        outCode.Position = curPos;
                    }
                }
                method.Body.Bytecode = outCode.ToArray();
            }
        }

        protected void CopyBytecode(FlashReader inCode, FlashWriter outCode)
        {
            CopyBytecode(inCode, outCode, inCode.Position);
        }
        protected void CopyBytecode(FlashReader inCode, FlashWriter outCode, int index)
        {
            CopyBytecode(inCode, outCode, index, inCode.Length - index);
        }
        protected void CopyBytecode(FlashReader inCode, FlashWriter outCode, int index, int count)
        {
            byte[] bytecode = inCode.ToArray();
            outCode.Write(bytecode, index, count);
        }

        protected bool IsJumpInstruction(OPCode op)
        {
            switch (op)
            {
                case OPCode.IfEq:
                case OPCode.IfGe:
                case OPCode.IfGt:
                case OPCode.IfLe:
                case OPCode.IfLt:
                case OPCode.Jump:
                case OPCode.IfNe:
                case OPCode.IfNGe:
                case OPCode.IfNGt:
                case OPCode.IfNLe:
                case OPCode.IfNLt:
                case OPCode.IfTrue:
                case OPCode.IfFalse:
                case OPCode.IfStrictEq:
                case OPCode.IfStrictNE: return true;
            }
            return false;
        }
        protected void PrependOperations(ASMethod method, params OPCode[] operations)
        {
            if (operations.Length == 0) return;
            byte[] bytecode = method.Body.Bytecode;
            byte[] buffer = new byte[operations.Length + bytecode.Length];

            for (int i = 0; i < operations.Length; i++)
                buffer[i] = (byte)operations[i];

            Buffer.BlockCopy(bytecode, 0, buffer, operations.Length, bytecode.Length);
            method.Body.Bytecode = buffer;

            WriteLog($"Prepended operations '{string.Join(", ", operations)}' in method '{method}'.");
        }
        protected bool ContainsOperation(ASMethod method, OPCode operation, params object[] expectedValues)
        {
            using (var inCode = new FlashReader(method.Body.Bytecode))
            {
                while (inCode.IsDataAvailable)
                {
                    return (inCode.ReadValuesUntil(
                        operation, expectedValues) != null);
                }
            }
            return false;
        }

        protected void ReplaceNextOperation(ASMethod method, OPCode oldOP, object[] oldValues, OPCode newOP, object[] newValues)
        {
            using (var outCode = new FlashWriter())
            using (var inCode = new FlashReader(method.Body.Bytecode))
            {
                ReplaceNextOperation(inCode, outCode, method, oldOP, oldValues, newOP, newValues);

                CopyBytecode(inCode, outCode);
                method.Body.Bytecode = outCode.ToArray();
            }
        }
        protected void ReplaceNextOperation(FlashReader inCode, FlashWriter outCode, ASMethod method, OPCode oldOP, object[] oldValues, OPCode newOP, object[] newValues)
        {
            while (inCode.IsDataAvailable)
            {
                OPCode op = inCode.ReadOP();
                object[] values = inCode.ReadValues(op);
                if (op != oldOP)
                {
                    outCode.WriteOP(op, values);
                    continue;
                }
                if (oldValues != null && (oldValues.Length == values.Length))
                {
                    bool valuesMatch = true;
                    for (int i = 0; i < oldValues.Length; i++)
                    {
                        if (oldValues[i] != null &&
                            !oldValues[i].Equals(values[i]))
                        {
                            valuesMatch = false;
                            break;
                        }
                    }
                    if (!valuesMatch)
                    {
                        outCode.WriteOP(op, values);
                        continue;
                    }
                }
                outCode.WriteOP(newOP, newValues);
                WriteLog($"Replaced operation '{oldOP}[{string.Join(", ", oldValues)}]' with '{newOP}[{string.Join(", ", newValues)}]' in method '{method}'.");
                break;
            }
        }
        #endregion

        /// <summary>
        /// Dissassembles the Habbo Hotel flash client.
        /// </summary>
        public override void Disassemble()
        {
            WriteLog("Disassembling...");
            base.Disassemble();

            ABCFile abc = ABCFiles[2];
            ASClass habboMessagesClass = abc.FindFirstClassByName("HabboMessages");
            if (habboMessagesClass != null)
            {
                FindHabboMessageClasses(habboMessagesClass);

                WriteLog(string.Format("Outgoing({0})/Incoming({1}) messages extracted.",
                    OutgoingMessages.Count, IncomingMessages.Count));
            }
        }
    }
}