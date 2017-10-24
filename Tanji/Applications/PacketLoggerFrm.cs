using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Tanji.Components;
using Tanji.Manipulators;
using Tanji.Applications.Dialogs;

using Sulakore.Habbo;
using Sulakore.Protocol;
using Sulakore.Communication;

namespace Tanji.Applications
{
    [DesignerCategory("Form")]
    public partial class PacketLoggerFrm : ObservableForm, IReceiver, IHaltable
    {
        private FindDialog _currentFindUI;
        private FindMessageDialog _currentFindMessageUI;
        private IgnoreMessagesDialog _currentIgnoreMessagesUI;

        private readonly MainFrm _main;
        private readonly Dictionary<int, bool> _ignoredMessages;
        private readonly object _writeQueueLock, _processQueueLock;
        private readonly Queue<DataInterceptedEventArgs> _intercepted;
        private readonly Action<List<Tuple<string, Color>>> _displayEntries;

        public Color FilterHighlight { get; set; } = Color.Yellow;
        public Color DetailHighlight { get; set; } = Color.DarkGray;
        public Color IncomingHighlight { get; set; } = Color.FromArgb(178, 34, 34);
        public Color OutgoingHighlight { get; set; } = Color.FromArgb(0, 102, 204);
        public Color StructureHighlight { get; set; } = Color.FromArgb(0, 204, 136);

        public bool IsFindDialogOpened => (!(_currentFindUI?.IsDisposed ?? true));
        public bool IsFindMessageDialogOpened => (!(_currentFindMessageUI?.IsDisposed ?? true));
        public bool IsIgnoreMessagesDialogOpened => (!(_currentIgnoreMessagesUI?.IsDisposed ?? true));

        private bool _isReceiving = true;
        public bool IsReceiving => (_isReceiving && (IsViewingOutgoing || IsViewingIncoming));

        private bool _isDisplayingBlocked = true;
        public bool IsDisplayingBlocked
        {
            get => _isDisplayingBlocked;
            set
            {
                _isDisplayingBlocked = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isDisplayingReplaced = true;
        public bool IsDisplayingReplaced
        {
            get => _isDisplayingReplaced;
            set
            {
                _isDisplayingReplaced = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isDisplayingHash = true;
        public bool IsDisplayingHash
        {
            get
            {
                if (_main?.Game?.IsPostShuffle ?? true)
                {
                    return _isDisplayingHash;
                }
                return false;
            }
            set
            {
                _isDisplayingHash = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isDisplayingHexadecimal = false;
        public bool IsDisplayingHexadecimal
        {
            get => _isDisplayingHexadecimal;
            set
            {
                _isDisplayingHexadecimal = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isDisplayingStructure = true;
        public bool IsDisplayingStructure
        {
            get => _isDisplayingStructure;
            set
            {
                _isDisplayingStructure = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isDisplayingParserName = true;
        public bool IsDisplayingParserName
        {
            get => _isDisplayingParserName;
            set
            {
                _isDisplayingParserName = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isDisplayingMessageName = true;
        public bool IsDisplayingMessageName
        {
            get => _isDisplayingMessageName;
            set
            {
                _isDisplayingMessageName = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isViewingOutgoing = true;
        public bool IsViewingOutgoing
        {
            get => _isViewingOutgoing;
            set
            {
                _isViewingOutgoing = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isViewingIncoming = true;
        public bool IsViewingIncoming
        {
            get => _isViewingIncoming;
            set
            {
                _isViewingIncoming = value;
                RaiseOnPropertyChanged();
            }
        }

        private string _revision = string.Empty;
        public string Revision
        {
            get => _revision;
            set
            {
                _revision = value;
                RaiseOnPropertyChanged();
            }
        }

        public bool IsAlwaysOnTop
        {
            get => TopMost;
            set
            {
                _main.TopMost = value;
                TopMost = value;

                RaiseOnPropertyChanged();
            }
        }

        public PacketLoggerFrm(MainFrm main)
        {
            InitializeComponent();

            Bind(BlockedBtn, "Checked", nameof(IsDisplayingBlocked));
            Bind(ReplacedBtn, "Checked", nameof(IsDisplayingReplaced));

            Bind(HashBtn, "Checked", nameof(IsDisplayingHash));
            Bind(HexadecimalBtn, "Checked", nameof(IsDisplayingHexadecimal));
            Bind(StructureBtn, "Checked", nameof(IsDisplayingStructure));
            Bind(ParserNameBtn, "Checked", nameof(IsDisplayingParserName));
            Bind(MessageName, "Checked", nameof(IsDisplayingMessageName));

            Bind(ViewOutgoingBtn, "Checked", nameof(IsViewingOutgoing));
            Bind(ViewIncomingBtn, "Checked", nameof(IsViewingIncoming));

            Bind(AlwaysOnTopBtn, "Checked", nameof(IsAlwaysOnTop));

            _main = main;
            _writeQueueLock = new object();
            _processQueueLock = new object();
            _displayEntries = DisplayEntries;
            _ignoredMessages = new Dictionary<int, bool>();
            _intercepted = new Queue<DataInterceptedEventArgs>();
        }

        private void CopyBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(LoggerTxt.SelectedText))
            {
                Clipboard.SetText(LoggerTxt.SelectedText);
            }
        }
        private void EmptyLogBtn_Click(object sender, EventArgs e)
        {
            LoggerTxt.Clear();
        }

        private void FindBtn_Click(object sender, EventArgs e)
        {
            if (IsFindDialogOpened)
            {
                _currentFindUI.BringToFront();
            }
            else
            {
                _currentFindUI = new FindDialog(LoggerTxt);
                _currentFindUI.Show(this);
            }
        }
        private void FindMessageBtn_Click(object sender, EventArgs e)
        {
            if (IsFindMessageDialogOpened)
            {
                _currentFindMessageUI.BringToFront();
            }
            else
            {
                _currentFindMessageUI = new FindMessageDialog(_main.Game, LoggerTxt.SelectedText);
                _currentFindMessageUI.Show(this);
            }
        }
        private void IgnoreMessagesBtn_Click(object sender, EventArgs e)
        {
            if (IsIgnoreMessagesDialogOpened)
            {
                _currentIgnoreMessagesUI.BringToFront();
            }
            else
            {
                _currentIgnoreMessagesUI = new IgnoreMessagesDialog(_ignoredMessages);
                _currentIgnoreMessagesUI.Show(this);
            }
        }

        public void Halt()
        {
            Close();
            Hide();
        }
        public void HandleOutgoing(DataInterceptedEventArgs e) => PushToQueue(e);
        public void HandleIncoming(DataInterceptedEventArgs e) => PushToQueue(e);

        private void ProcessQueue()
        {
            while (IsReceiving && _intercepted.Count > 0)
            {
                DataInterceptedEventArgs args = _intercepted.Dequeue();
                if (!IsLoggingAuthorized(args)) continue;

                bool isOutgoing = (args.Packet.Destination == HDestination.Server);

                var entry = new List<Tuple<string, Color>>();
                if (args.IsBlocked)
                {
                    entry.Add(Tuple.Create("[Blocked]\r\n", FilterHighlight));
                }
                if (!args.IsOriginal)
                {
                    entry.Add(Tuple.Create("[Replaced]\r\n", FilterHighlight));
                }

                MessageItem message = GetMessage(args);
                if (IsDisplayingHash && message != null && !string.IsNullOrWhiteSpace(message.Hash))
                {
                    entry.Add(Tuple.Create($"[{message.Hash}]\r\n", DetailHighlight));
                }

                if (IsDisplayingHexadecimal)
                {
                    string hex = BitConverter.ToString(args.Packet.ToBytes());
                    entry.Add(Tuple.Create($"[{hex.Replace("-", string.Empty)}]\r\n", DetailHighlight));
                }

                string arrow = "->";
                string title = "Outgoing";
                Color entryHighlight = OutgoingHighlight;
                if (!isOutgoing)
                {
                    arrow = "<-";
                    title = "Incoming";
                    entryHighlight = IncomingHighlight;
                }

                entry.Add(Tuple.Create(title + "[", entryHighlight));
                entry.Add(Tuple.Create(args.Packet.Header.ToString(), DetailHighlight));

                if (message != null)
                {
                    if (IsDisplayingMessageName)
                    {
                        entry.Add(Tuple.Create(", ", entryHighlight));
                        entry.Add(Tuple.Create(message.Class.QName.Name, DetailHighlight));
                    }
                    if (IsDisplayingParserName && message.Parser != null)
                    {
                        entry.Add(Tuple.Create(", ", entryHighlight));
                        entry.Add(Tuple.Create(message.Parser.QName.Name, DetailHighlight));
                    }
                }
                entry.Add(Tuple.Create("]", entryHighlight));
                entry.Add(Tuple.Create($" {arrow} ", DetailHighlight));
                entry.Add(Tuple.Create($"{args.Packet}\r\n", entryHighlight));

                if (IsDisplayingStructure && message?.Structure?.Length >= 0)
                {
                    int position = 0;
                    bool endReading = false;
                    HMessage packet = args.Packet;
                    string structure = $"{{l}}{{u:{packet.Header}}}";
                    foreach (string valueType in message.Structure)
                    {
                        if (endReading) break;
                        switch (valueType.ToLower())
                        {
                            case "int":
                            structure += ("{i:" + packet.ReadInteger(ref position) + "}");
                            break;

                            case "string":
                            structure += ("{s:" + packet.ReadString(ref position) + "}");
                            break;

                            case "double":
                            endReading = true;
                            break;

                            case "byte":
                            structure += ("{b:" + packet.ReadBytes(1, ref position)[0] + "}");
                            break;

                            case "boolean":
                            structure += ("{b:" + packet.ReadBoolean(ref position) + "}");
                            break;
                        }
                    }
                    if (packet.ReadableAt(position) == 0)
                    {
                        entry.Add(Tuple.Create(structure + "\r\n", StructureHighlight));
                    }
                }
                entry.Add(Tuple.Create("--------------------\r\n", DetailHighlight));

                while (!IsHandleCreated) ;
                if (IsReceiving)
                {
                    BeginInvoke(_displayEntries, entry);
                }
            }
        }
        private void PushToQueue(DataInterceptedEventArgs e)
        {
            lock (_writeQueueLock)
            {
                if (IsLoggingAuthorized(e))
                {
                    _intercepted.Enqueue(e);
                }
            }
            if (IsReceiving && Monitor.TryEnter(_processQueueLock))
            {
                try
                {
                    while (IsReceiving && _intercepted.Count > 0)
                    {
                        ProcessQueue();
                    }
                }
                finally { Monitor.Exit(_processQueueLock); }
            }
        }

        private MessageItem GetMessage(DataInterceptedEventArgs e)
        {
            IDictionary<ushort, MessageItem> messages = ((e.Packet.Destination == HDestination.Server) ?
                _main.Game.OutMessages : _main.Game.InMessages);

            MessageItem message = null;
            messages.TryGetValue(e.Packet.Header, out message);

            return message;
        }
        private bool IsLoggingAuthorized(DataInterceptedEventArgs e)
        {
            bool isOutgoing = (e.Packet.Destination == HDestination.Server);

            if (!IsReceiving) return false;
            if (IsFindDialogOpened) return false;
            if (IsFindMessageDialogOpened) return false;

            if (!IsDisplayingBlocked && e.IsBlocked) return false;
            if (!IsDisplayingReplaced && !e.IsOriginal) return false;

            if (!IsViewingOutgoing && isOutgoing) return false;
            if (!IsViewingIncoming && !isOutgoing) return false;

            if (_ignoredMessages.Count > 0)
            {
                int container = e.Packet.Header;
                if (isOutgoing)
                {
                    container += ushort.MaxValue;
                }
                if (_ignoredMessages.TryGetValue(container, out bool isIgnoring) && isIgnoring) return false;
            }
            return true;
        }
        private void DisplayEntries(List<Tuple<string, Color>> entry)
        {
            foreach (Tuple<string, Color> chunk in entry)
            {
                LoggerTxt.SelectionStart = LoggerTxt.TextLength;
                LoggerTxt.SelectionLength = 0;

                LoggerTxt.SelectionColor = chunk.Item2;
                LoggerTxt.AppendText(chunk.Item1);
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            switch (e.PropertyName)
            {
                case nameof(IsAlwaysOnTop):
                {
                    Text = "Tanji ~ Packet Logger";
                    if (IsAlwaysOnTop)
                    {
                        Text += " | Top Most";
                    }
                    break;
                }
                case nameof(IsViewingOutgoing):
                {
                    ViewOutgoingLbl.Text = ("View Outgoing: " + IsViewingOutgoing);
                    break;
                }
                case nameof(IsViewingIncoming):
                {
                    ViewIncomingLbl.Text = ("View Incoming: " + IsViewingIncoming);
                    break;
                }
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            if (!_isReceiving)
            {
                LoggerTxt.Clear();
                _intercepted.Clear();

                _isReceiving = true;
            }
            base.OnActivated(e);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _isReceiving = false;
            _intercepted.Clear();

            LoggerTxt.Clear();
            WindowState = FormWindowState.Minimized;

            e.Cancel = true;
            base.OnFormClosing(e);
        }
    }
}