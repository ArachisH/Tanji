using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Tanji.Controls;
using Tanji.Services;

using Sulakore.Habbo;
using Sulakore.Network;
using Sulakore.Habbo.Messages;
using Sulakore.Network.Protocol;

namespace Tanji.Windows
{
    [DesignerCategory("Form")]
    public partial class PacketLoggerFrm : ObservableForm, IHaltable, IReceiver
    {
        private readonly object _queueWriteLock;
        private readonly object _queueProcessLock;
        private readonly Queue<DataInterceptedEventArgs> _intercepted;
        private readonly Action<List<(string, Color)>> _displayEntries;

        private int _streak = 1;
        private DataInterceptedEventArgs _lastIntercepted;

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

            InitializeComponent();

            Bind(BlockedBtn, "Checked", nameof(IsDisplayingBlocked));
            Bind(ReplacedBtn, "Checked", nameof(IsDisplayingReplaced));
            Bind(DismantledBtn, "Checked", nameof(IsDisplayingDismantled));
            Bind(HashNameBtn, "Checked", nameof(IsDisplayingHashName));
            Bind(HexadecimalBtn, "Checked", nameof(IsDisplayingHexadecimal));
            Bind(ParserNameBtn, "Checked", nameof(IsDisplayingParserName));
            Bind(ClassNameBtn, "Checked", nameof(IsDisplayingClassName));
            Bind(ViewOutgoingBtn, "Checked", nameof(IsViewingOutgoing));
            Bind(ViewIncomingBtn, "Checked", nameof(IsViewingIncoming));
            Bind(AlwaysOnTopBtn, "Checked", nameof(TopMost));
        }

        private void PacketLoggerFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
        }

        private void ProcessQueue()
        {
            while (IsReceiving && _intercepted.Count > 0)
            {
                DataInterceptedEventArgs intercepted = _intercepted.Dequeue();
                if (!IsLoggingAuthorized(intercepted)) continue;

                var entries = new List<(string, Color)>();
                if (_lastIntercepted != null && AreEqual(_lastIntercepted.Packet, intercepted.Packet))
                {
                    entries.Add(("RL--------------------", DefaultHighlight));
                    entries.Add((" < +" + (_streak++) + " > ", Color.Red));
                }
                else _streak = 1;

                _lastIntercepted = intercepted;
                if (_streak >= 2)
                {
                    while (!IsHandleCreated) ;
                    if (IsReceiving)
                    {
                        Invoke(_displayEntries, entries);
                        Application.DoEvents();
                    }
                    continue;
                }

                if (_lastIntercepted != null)
                {
                    entries.Add(("\r\n", DefaultHighlight));
                }

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

                if (IsDisplayingDismantled && message?.Structure?.Length >= 0)
                {
                    int position = 0;
                    HPacket packet = intercepted.Packet;
                    string dismantled = $"{{id:{packet.Id}}}";
                    foreach (string valueType in message.Structure)
                    {
                        switch (valueType.ToLower())
                        {
                            case "int":
                            dismantled += ("{i:" + packet.ReadInt32(ref position) + "}");
                            break;

                            case "string":
                            dismantled += ("{s:" + packet.ReadUTF8(ref position) + "}");
                            break;

                            case "double":
                            dismantled += ("{s:" + packet.ReadDouble(ref position) + "}");
                            break;

                            case "byte":
                            dismantled += ("{b:" + packet.ReadByte(ref position) + "}");
                            break;

                            case "boolean":
                            dismantled += ("{b:" + packet.ReadBoolean(ref position) + "}");
                            break;
                        }
                    }
                    if (packet.GetReadableBytes(position) == 0)
                    {
                        entries.Add((dismantled + "\r\n", DismantledHighlight));
                    }
                }
                entries.Add(("--------------------", DefaultHighlight));

                while (!IsHandleCreated) ;
                if (IsReceiving)
                {
                    Invoke(_displayEntries, entries);
                    Application.DoEvents();
                }
            }
        }
        private void PushToQueue(DataInterceptedEventArgs e)
        {
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
                    while (IsReceiving && _intercepted.Count > 0)
                    {
                        ProcessQueue();
                    }
                }
                finally { Monitor.Exit(_queueProcessLock); }
            }
        }

        private bool AreEqual(HPacket p1, HPacket p2)
        {
            byte[] p1Data = p1.ToBytes();
            byte[] p2Data = p2.ToBytes();
            if (p1Data.Length != p2Data.Length) return false;

            return (NativeMethods.memcmp(p1Data, p2Data, p2Data.Length) == 0);
        }
        private void DisplayEntries(List<(string, Color)> entries)
        {
            foreach ((string message, Color highlight) in entries)
            {
                if (message.StartsWith("RL")) // Replace Line
                {
                    int currentLineIndex = (LogTxt.Lines.Length - 1);
                    string currentLine = LogTxt.Lines[currentLineIndex];
                    LogTxt.Find(currentLine, RichTextBoxFinds.Reverse);

                    LogTxt.SelectionColor = highlight;
                    LogTxt.SelectedText = message.Substring(2);
                }
                else
                {
                    LogTxt.SelectionColor = highlight;
                    LogTxt.AppendText(message);
                }
            }
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

        #region IHaltable Implementation
        public void Halt()
        {
            Close();
        }
        public void Restore()
        {
            WindowState = FormWindowState.Normal;
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