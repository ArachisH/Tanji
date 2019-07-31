using System;
using System.Drawing;
using System.Threading;
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

        private bool _isDisplayingHash = true;
        public bool IsDisplayingHash
        {
            get => _isDisplayingHash;
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

                ViewOutgoingLbl.Text = $"View Outgoing: " + value;
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

                ViewIncomingLbl.Text = $"View Incoming: " + value;
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

        public bool IsFindDialogOpened => !(_findDialog?.IsDisposed ?? true);

        public Color DetailHighlight { get; set; } = Color.Cyan;
        public Color DefaultHighlight { get; set; } = Color.White;
        public Color BlockedHighlight { get; set; } = Color.Yellow;
        public Color ReplacedHighlight { get; set; } = Color.Yellow;
        public Color IncomingHighlight { get; set; } = Color.FromArgb(178, 34, 34);
        public Color OutgoingHighlight { get; set; } = Color.FromArgb(0, 102, 204);
        public Color StructureHighlight { get; set; } = Color.FromArgb(170, 244, 66);
        public Color DismantledHighlight { get; set; } = Color.FromArgb(0, 204, 136);
        public Color ConcurrentPacketHighlight { get; set; } = Color.FromArgb(255, 174, 61);

        public PacketLoggerFrm()
        {
            _queueWriteLock = new object();
            _queueProcessLock = new object();
            _displayEntries = DisplayEntries;
            _intercepted = new Queue<DataInterceptedEventArgs>();

            InitializeComponent();

            Bind(AlwaysOnTopBtn, "Checked", nameof(TopMost));
            Bind(HashBtn, "Checked", nameof(IsDisplayingHash));
            Bind(BlockedBtn, "Checked", nameof(IsDisplayingBlocked));
            Bind(ReplacedBtn, "Checked", nameof(IsDisplayingReplaced));
            Bind(AutoScrollingBtn, "Checked", nameof(IsAutoScrolling));
            Bind(ViewOutgoingBtn, "Checked", nameof(IsViewingOutgoing));
            Bind(ViewIncomingBtn, "Checked", nameof(IsViewingIncoming));
            Bind(ClassNameBtn, "Checked", nameof(IsDisplayingClassName));
            Bind(StructureBtn, "Checked", nameof(IsDisplayingStructure));
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
                        entries.Add(($" x{++_streak}", ConcurrentPacketHighlight));
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
                HMessage message = GetMessage(intercepted);

                if (IsDisplayingStructure && !string.IsNullOrWhiteSpace(message?.Structure))
                {
                    AddEntryLine(entries, message.Structure, StructureHighlight);
                }

                if (intercepted.IsBlocked)
                {
                    AddEntryLine(entries, "Blocked", BlockedHighlight);
                }
                if (!intercepted.IsOriginal)
                {
                    AddEntryLine(entries, "Replaced", BlockedHighlight);
                }

                if (IsDisplayingHash && !string.IsNullOrWhiteSpace(message?.Hash))
                {
                    if (!string.IsNullOrWhiteSpace(message.Name))
                    {
                        AddEntry(entries, message.Name, DetailHighlight, right: ", ");
                    }
                    else AddEntry(entries, null, DefaultHighlight, right: null);
                    AddEntryLine(entries, message.Hash, DetailHighlight, left: null);
                }

                if (IsDisplayingHexadecimal)
                {
                    AddEntryLine(entries, BitConverter.ToString(intercepted.Packet.ToBytes()).Replace("-", string.Empty), DetailHighlight);
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

                AddEntry(entries, title + "[", entryHighlight, null, intercepted.Packet.Id.ToString());
                if (message != null)
                {
                    if (IsDisplayingClassName)
                    {
                        AddEntry(entries, ", ", entryHighlight, null, message.ClassName);
                    }
                    if (IsDisplayingParserName && !string.IsNullOrWhiteSpace(message.ParserName))
                    {
                        AddEntry(entries, ", ", entryHighlight, null, message.ParserName);
                    }
                }
                AddEntry(entries, "]", entryHighlight, null, $" {arrow} ");
                entries.Add(($"{intercepted.Packet}\r\n", entryHighlight));

                if (IsDisplayingDismantled && !string.IsNullOrWhiteSpace(message?.Structure))
                {
                    int index = 0;
                    int position = 0;
                    HPacket packet = intercepted.Packet;
                    string dismantled = $"{{id:{packet.Id}}}{LoopDismantle(packet, ref position, message.Structure, ref index, 1)}";

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

        private HMessage GetMessage(DataInterceptedEventArgs e)
        {
            HMessages messages = e.IsOutgoing ? Program.Master.Out : (HMessages)Program.Master.In;
            return messages.GetMessage(e.Packet.Id);
        }
        private void CalculateLatency(DataInterceptedEventArgs e)
        {
            if (e.IsOutgoing && e.Packet.Id == Master.Out.LatencyPingRequest)
            {
                _latencyTestStart = e.Timestamp;
            }
            else if (e.Packet.Id == Master.In.LatencyPingResponse)
            {
                Latency = (e.Timestamp - _latencyTestStart).TotalMilliseconds;
            }
        }
        private void DisplayEntries(List<(string, Color)> entries)
        {
            IDisposable suspender = !IsAutoScrolling ? LogTxt.GetSuspender() : null;
            try
            {
                foreach ((string message, Color highlight) in entries)
                {
                    if (message.StartsWith("RL")) // Replace Line
                    {
                        int currentLineIndex = LogTxt.Lines.Length - 1;
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
        private bool IsLoggingAuthorized(DataInterceptedEventArgs e)
        {
            if (!IsReceiving) return false;
            if (IsFindDialogOpened) return false;
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

        private string LoopDismantle(HPacket packet, ref int position, string structure, ref int index, int loops)
        {
            int previousInt = 0;
            string dismantled = "";
            int previousIndex = index;
            for (int j = 0; j < loops; j++)
            {
                for (; index < structure.Length; index++)
                {
                    char piece = structure[index];
                    if (piece == ')')
                    {
                        if (j + 1 < loops)
                        {
                            index = previousIndex;
                        }
                        break;
                    }

                    switch (piece)
                    {
                        case '(':
                        index++;
                        dismantled += LoopDismantle(packet, ref position, structure, ref index, previousInt);
                        break;

                        case 'i':
                        previousInt = packet.ReadInt32(ref position);
                        dismantled += "{i:" + previousInt + "}";
                        break;

                        case 's':
                        dismantled += "{s:" + packet.ReadUTF8(ref position) + "}";
                        break;

                        case 'd':
                        dismantled += "{s:" + packet.ReadDouble(ref position) + "}";
                        break;

                        case 'b':
                        dismantled += "{b:" + packet.ReadByte(ref position) + "}";
                        break;

                        case 'B':
                        dismantled += "{b:" + packet.ReadBoolean(ref position) + "}";
                        break;
                    }
                }
            }
            return dismantled;
        }

        private void AddEntry(IList<(string, Color)> entries, string middle, Color highlight, string left = "[", string right = "]")
        {
            if (!string.IsNullOrEmpty(left))
            {
                entries.Add((left, DefaultHighlight));
            }
            if (!string.IsNullOrEmpty(middle))
            {
                entries.Add((middle, highlight));
            }
            if (!string.IsNullOrEmpty(right))
            {
                entries.Add((right, DefaultHighlight));
            }
        }
        private void AddEntryLine(IList<(string, Color)> entries, string middle, Color highlight, string left = "[", string right = "]\r\n")
        {
            AddEntry(entries, middle, highlight, left, right);
        }

        #region IHaltable Implementation
        public void Halt()
        {
            _isReceiving = false;
            Close();
        }
        public void Restore(ConnectedEventArgs e)
        {
            _isReceiving = true;
            WindowState = FormWindowState.Normal;
            RevisionLbl.Text = "Revision: " + Master.Game.Revision;
        }
        #endregion
        #region IReceiver Implementation
        private bool _isReceiving = true;
        public bool IsReceiving => _isReceiving && (IsViewingOutgoing || IsViewingIncoming);

        public void HandleOutgoing(DataInterceptedEventArgs e) => PushToQueue(e);
        public void HandleIncoming(DataInterceptedEventArgs e) => PushToQueue(e);
        #endregion
    }
}