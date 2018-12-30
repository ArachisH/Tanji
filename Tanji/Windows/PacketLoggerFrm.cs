using System;
using System.Drawing;
using System.Threading;
using System.Configuration;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Tanji.Network;
using Tanji.Controls;
using Tanji.Services;
using Tanji.Windows.Dialogs;

using Sulakore.Network;
using Sulakore.Habbo.Web;
using Sulakore.Habbo.Messages;
using Sulakore.Network.Protocol;

namespace Tanji.Windows
{
    [DesignerCategory("Form")]
    public partial class PacketLoggerFrm : ObservableForm, IHaltable, IReceiver
    {
        private int _streak = 1;
        private FindDialog _findDialog;
        private DateTime _latencyTestStart;
        private DataInterceptedEventArgs _lastIntercepted;

        private readonly object _queueWriteLock;
        private readonly object _queueProcessLock;
        private readonly Queue<DataInterceptedEventArgs> _intercepted;
        private readonly Action<List<(string, Color)>> _displayEntries;
        private readonly IDictionary<string, string[]> _outStructureOverrides, _inStructureOverrides;

        #region Bindable Properties
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

        private bool _isDisplayingDismantled = true;
        public bool IsDisplayingDismantled
        {
            get => _isDisplayingDismantled;
            set
            {
                _isDisplayingDismantled = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isDisplayingHashName = true;
        public bool IsDisplayingHashName
        {
            get => _isDisplayingHashName;
            set
            {
                _isDisplayingHashName = value;
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

        private bool _isDisplayingClassName = true;
        public bool IsDisplayingClassName
        {
            get => _isDisplayingClassName;
            set
            {
                _isDisplayingClassName = value;
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

                ViewOutgoingLbl.Text = ($"View Outgoing: " + value);
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

                ViewIncomingLbl.Text = ($"View Incoming: " + value);
            }
        }

        private bool _isAutoScrolling = true;
        public bool IsAutoScrolling
        {
            get => _isAutoScrolling;
            set
            {
                _isAutoScrolling = value;
                RaiseOnPropertyChanged();
            }
        }

        private double _latency = 0;
        public double Latency
        {
            get => _latency;
            set
            {
                _latency = value;
                RaiseOnPropertyChanged();
            }
        }

        public new bool TopMost
        {
            get => base.TopMost;
            set
            {
                base.TopMost = value;
                RaiseOnPropertyChanged();

                Text = "Tanji - Packet Logger";
                if (value)
                {
                    Text += " | Top Most";
                }
            }
        }
        #endregion

        public bool IsFindDialogOpened => (!(_findDialog?.IsDisposed ?? true));

        public Color DetailHighlight { get; set; } = Color.Cyan;
        public Color DefaultHighlight { get; set; } = Color.White;
        public Color IncomingHighlight { get; set; } = Color.FromArgb(178, 34, 34);
        public Color OutgoingHighlight { get; set; } = Color.FromArgb(0, 102, 204);
        public Color DismantledHighlight { get; set; } = Color.FromArgb(0, 204, 136);

        public PacketLoggerFrm()
        {
            _queueWriteLock = new object();
            _queueProcessLock = new object();
            _displayEntries = DisplayEntries;
            _intercepted = new Queue<DataInterceptedEventArgs>();
            _inStructureOverrides = PopulateStructureOverrides(ConfigurationManager.AppSettings["InStructureOverrides"]);
            _outStructureOverrides = PopulateStructureOverrides(ConfigurationManager.AppSettings["OutStructureOverrides"]);

            InitializeComponent();

            Bind(AlwaysOnTopBtn, "Checked", nameof(TopMost));
            Bind(BlockedBtn, "Checked", nameof(IsDisplayingBlocked));
            Bind(ReplacedBtn, "Checked", nameof(IsDisplayingReplaced));
            Bind(HashNameBtn, "Checked", nameof(IsDisplayingHashName));
            Bind(AutoScrollingBtn, "Checked", nameof(IsAutoScrolling));
            Bind(ViewOutgoingBtn, "Checked", nameof(IsViewingOutgoing));
            Bind(ViewIncomingBtn, "Checked", nameof(IsViewingIncoming));
            Bind(ClassNameBtn, "Checked", nameof(IsDisplayingClassName));
            Bind(ParserNameBtn, "Checked", nameof(IsDisplayingParserName));
            Bind(DismantledBtn, "Checked", nameof(IsDisplayingDismantled));
            Bind(HexadecimalBtn, "Checked", nameof(IsDisplayingHexadecimal));
        }

        private void FindBtn_Click(object sender, EventArgs e)
        {
            if (!IsFindDialogOpened)
            {
                _findDialog = new FindDialog(LogTxt);
                _findDialog.Show(this);
            }
            else _findDialog.BringToFront();
        }
        private void EmptyLogBtn_Click(object sender, EventArgs e)
        {
            _lastIntercepted = null;
            LogTxt.Clear();
        }

        private void PacketLoggerFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
        }
        private void PacketLoggerFrm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Latency):
                {
                    LatencyLbl.Text = $"Latency: {Latency:0.##}ms";
                    break;
                }
            }
        }

        private void ProcessQueue()
        {
            while (IsReceiving && _intercepted.Count > 0)
            {
                DataInterceptedEventArgs intercepted = _intercepted.Dequeue();
                if (!IsLoggingAuthorized(intercepted)) continue;

                var entries = new List<(string, Color)>();
                if (_lastIntercepted != null)
                {
                    if (_lastIntercepted.Packet.Equals(intercepted.Packet))
                    {
                        entries.Add(("RL--------------------", DefaultHighlight));
                        entries.Add((" < +" + (_streak++) + " > ", Color.MediumPurple));
                        if (IsReceiving)
                        {
                            Invoke(_displayEntries, entries);
                            Application.DoEvents();
                        }
                        continue;
                    }
                    else
                    {
                        _streak = 1;
                        entries.Add(("\r\n", DefaultHighlight));
                    }
                }
                _lastIntercepted = intercepted;

                if (intercepted.IsBlocked)
                {
                    entries.Add(("[", DefaultHighlight));
                    entries.Add(("Blocked", Color.Yellow));
                    entries.Add(("]\r\n", DefaultHighlight));
                }
                if (!intercepted.IsOriginal)
                {
                    entries.Add(("[", DefaultHighlight));
                    entries.Add(("Replaced", Color.Yellow));
                    entries.Add(("]\r\n", DefaultHighlight));
                }

                MessageItem message = GetMessage(intercepted);
                if (IsDisplayingHashName && !string.IsNullOrWhiteSpace(message?.Hash))
                {
                    var identifiers = (intercepted.IsOutgoing ? (Identifiers)Program.Master.Out : Program.Master.In);

                    string name = identifiers.GetName(message.Hash);
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        entries.Add(("[", DefaultHighlight));
                        entries.Add((name, DetailHighlight));
                        entries.Add(("] ", DefaultHighlight));
                    }
                    entries.Add(("[", DefaultHighlight));
                    entries.Add((message.Hash, DetailHighlight));
                    entries.Add(("]\r\n", DefaultHighlight));
                }

                if (IsDisplayingHexadecimal)
                {
                    string hex = BitConverter.ToString(intercepted.Packet.ToBytes());
                    entries.Add(("[", DefaultHighlight));
                    entries.Add((hex.Replace("-", string.Empty), DetailHighlight));
                    entries.Add(("]\r\n", DefaultHighlight));
                }

                string arrow = "->";
                string title = "Outgoing";
                Color entryHighlight = OutgoingHighlight;
                if (!intercepted.IsOutgoing)
                {
                    arrow = "<-";
                    title = "Incoming";
                    entryHighlight = IncomingHighlight;
                }

                entries.Add((title + "[", entryHighlight));
                entries.Add((intercepted.Packet.Id.ToString(), DefaultHighlight));

                if (message != null)
                {
                    if (IsDisplayingClassName)
                    {
                        entries.Add((", ", entryHighlight));
                        entries.Add((message.Class.QName.Name, DefaultHighlight));
                    }
                    if (IsDisplayingParserName && message.Parser != null)
                    {
                        entries.Add((", ", entryHighlight));
                        entries.Add((message.Parser.QName.Name, DefaultHighlight));
                    }
                }
                entries.Add(("]", entryHighlight));
                entries.Add(($" {arrow} ", DefaultHighlight));
                entries.Add(($"{intercepted.Packet}\r\n", entryHighlight));

                string[] structure = message?.Structure;
                if ((intercepted.IsOutgoing ? _outStructureOverrides : _inStructureOverrides).TryGetValue(message?.Hash, out string[] structureOverride))
                {
                    structure = structureOverride;
                }

                if (IsDisplayingDismantled && structure?.Length >= 0)
                {
                    int index = 0;
                    int position = 0;
                    HPacket packet = intercepted.Packet;
                    string dismantled = $"{{id:{packet.Id}}}{LoopDismantle(packet, ref position, structure, ref index, 1)}";

                    if (packet.GetReadableBytes(position) == 0)
                    {
                        entries.Add((dismantled + "\r\n", DismantledHighlight));
                    }
                }
                entries.Add(("--------------------", DefaultHighlight));
                if (IsReceiving)
                {
                    Invoke(_displayEntries, entries);
                    Application.DoEvents();
                }
            }
        }
        private void PushToQueue(DataInterceptedEventArgs e)
        {
            CalculateLatency(e);
            lock (_queueWriteLock)
            {
                if (IsLoggingAuthorized(e))
                {
                    _intercepted.Enqueue(e);
                }
                else return;
            }
            if (IsReceiving && Monitor.TryEnter(_queueProcessLock))
            {
                e.Continue(true);
                try
                {
                    while (!IsHandleCreated)
                    {
                        CreateHandle();
                    }
                    while (IsReceiving && _intercepted.Count > 0)
                    {
                        ProcessQueue();
                    }
                }
                finally { Monitor.Exit(_queueProcessLock); }
            }
        }

        private void CalculateLatency(DataInterceptedEventArgs e)
        {
            if (e.IsOutgoing && e.Packet.Id == Master.Out.LatencyTest)
            {
                _latencyTestStart = e.Timestamp;
            }
            else if (e.Packet.Id == Master.In.LatencyResponse)
            {
                Latency = (e.Timestamp - _latencyTestStart).TotalMilliseconds;
            }
        }
        private void DisplayEntries(List<(string, Color)> entries)
        {
            IDisposable suspender = (!IsAutoScrolling ? LogTxt.GetSuspender() : null);
            try
            {
                foreach ((string message, Color highlight) in entries)
                {
                    if (message.StartsWith("RL")) // Replace Line
                    {
                        int currentLineIndex = (LogTxt.Lines.Length - 1);
                        string currentLine = LogTxt.Lines[currentLineIndex];
                        int currentLineFirstCharIndex = LogTxt.Find(currentLine, RichTextBoxFinds.Reverse | RichTextBoxFinds.NoHighlight);

                        LogTxt.SelectionStart = currentLineFirstCharIndex;
                        LogTxt.SelectionLength = currentLine.Length;

                        LogTxt.SelectionColor = highlight;
                        LogTxt.SelectedText = message.Substring(2);
                    }
                    else
                    {
                        LogTxt.SelectionStart = LogTxt.TextLength;
                        LogTxt.SelectionLength = 0;

                        LogTxt.SelectionColor = highlight;
                        LogTxt.AppendText(message);
                    }
                }
            }
            finally { suspender?.Dispose(); }
        }
        private MessageItem GetMessage(DataInterceptedEventArgs e)
        {
            IDictionary<ushort, MessageItem> messages = (e.IsOutgoing ?
                Program.Master.Game.OutMessages : Program.Master.Game.InMessages);

            MessageItem message = null;
            messages.TryGetValue(e.Packet.Id, out message);

            return message;
        }
        private bool IsLoggingAuthorized(DataInterceptedEventArgs e)
        {
            if (!IsReceiving) return false;
            //if (IsFindDialogOpened) return false;
            //if (IsFindMessageDialogOpened) return false;

            if (!IsDisplayingBlocked && e.IsBlocked) return false;
            if (!IsDisplayingReplaced && !e.IsOriginal) return false;

            if (!IsViewingOutgoing && e.IsOutgoing) return false;
            if (!IsViewingIncoming && !e.IsOutgoing) return false;

            //if (_ignoredMessages.Count > 0)
            //{
            //    int container = e.Packet.Header;
            //    if (isOutgoing)
            //    {
            //        container += ushort.MaxValue;
            //    }
            //    if (_ignoredMessages.TryGetValue(container, out bool isIgnoring) && isIgnoring) return false;
            //}
            return true;
        }
        public static IDictionary<string, string[]> PopulateStructureOverrides(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var structureOverrides = new Dictionary<string, string[]>();
            string[] sections = value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string section in sections)
            {
                string[] pieces = section.Split('_');
                string hash = pieces[0];

                var structure = new List<string>();
                for (int i = 0; i < pieces[1].Length; i++)
                {
                    char token = pieces[1][i];
                    switch (token)
                    {
                        case '[': structure.Add("+"); break;
                        case ']': structure.Add("-"); break;
                        default: structure.Add(token.ToString()); break;
                    }
                }
                structureOverrides.Add(hash, structure.ToArray());
            }
            return structureOverrides;
        }
        public static string LoopDismantle(HPacket packet, ref int position, string[] structure, ref int index, int loops)
        {
            int previousInt = 0;
            string dismantled = "";
            int previousIndex = index;
            for (int j = 0; j < loops; j++)
            {
                for (; index < structure.Length; index++)
                {
                    string valueType = structure[index];
                    if (valueType == "-")
                    {
                        if (j + 1 < loops)
                        {
                            index = previousIndex;
                        }
                        break;
                    }

                    switch (valueType.ToLower())
                    {
                        case "+":
                        index++;
                        dismantled += LoopDismantle(packet, ref position, structure, ref index, previousInt);
                        break;

                        case "i":
                        case "int":
                        previousInt = packet.ReadInt32(ref position);
                        dismantled += ("{i:" + previousInt + "}");
                        break;

                        case "s":
                        case "string":
                        dismantled += ("{s:" + packet.ReadUTF8(ref position) + "}");
                        break;

                        case "d":
                        case "double":
                        dismantled += ("{s:" + packet.ReadDouble(ref position) + "}");
                        break;

                        case "b":
                        case "byte":
                        dismantled += ("{b:" + packet.ReadByte(ref position) + "}");
                        break;

                        case "B":
                        case "boolean":
                        dismantled += ("{b:" + packet.ReadBoolean(ref position) + "}");
                        break;
                    }
                }
            }
            return dismantled;
        }

        #region IHaltable Implementation
        public void Halt()
        {
            Close();
        }
        public void Restore(ConnectedEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            RevisionLbl.Text = "Revision: " + Master.Game.Revision;
        }
        #endregion
        #region IReceiver Implementation
        private bool _isReceiving = true;
        public bool IsReceiving => (_isReceiving && (IsViewingOutgoing || IsViewingIncoming));

        public void HandleOutgoing(DataInterceptedEventArgs e) => PushToQueue(e);
        public void HandleIncoming(DataInterceptedEventArgs e) => PushToQueue(e);
        #endregion
    }
}