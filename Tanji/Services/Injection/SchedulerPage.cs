using System;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;

using Tanji.Network;
using Tanji.Controls;

using Sulakore.Network.Protocol;

namespace Tanji.Services.Injection
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class SchedulerPage : NotifiablePage, IHaltable
    {
        private string _packetText = string.Empty;
        public string PacketText
        {
            get => _packetText;
            set
            {
                _packetText = value;
                RaiseOnPropertyChanged();
            }
        }

        private int _interval = 250;
        public int Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _toServer = true;
        public bool ToServer
        {
            get => _toServer;
            set
            {
                _toServer = value;
                RaiseOnPropertyChanged();
            }
        }

        private int _cycles = 1;
        public int Cycles
        {
            get => _cycles;
            set
            {
                _cycles = value;
                RaiseOnPropertyChanged();
            }
        }

        private string _hotkeysText = string.Empty;
        public string HotkeysText
        {
            get => _hotkeysText;
            set
            {
                _hotkeysText = value;
                RaiseOnPropertyChanged();
            }
        }

        [Browsable(false)]
        public Keys Hotkeys { get; private set; }

        public SchedulerPage()
        {
            InitializeComponent();

            Bind(PacketTxt, "Text", nameof(PacketText));
            Bind(IntervalTxt, "Text", nameof(Interval));
            Bind(ToServerChbx, "Checked", nameof(ToServer));
            Bind(HotkeyTxt, "Text", nameof(HotkeysText));
            Bind(CyclesTxt, "Text", nameof(Cycles));

            if (Master != null)
            {
                Master.Hook.HotkeyPressed += Hook_HotkeyPressed;
            }
        }

        private void HotkeyTxt_KeyDown(object sender, KeyEventArgs e)
        {
            _hotkeysText = string.Empty;
            if (e.Modifiers != Keys.None)
            {
                _hotkeysText = e.Modifiers + "+";
            }

            Hotkeys = e.KeyData;
            HotkeysText = _hotkeysText += e.KeyCode;

            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        private void Hook_HotkeyPressed(object sender, KeyEventArgs e)
        { }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            SchedulesVw.ClearItems();
        }
        private void RemoveBtn_Click(object sender, EventArgs e)
        { }
        private void CreateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PacketText)) return;

            HPacket packet = Master.ConvertToPacket(PacketText, ToServer);
            if (packet == null || Master.NotifyIfCorrupt(packet)) return;

            ListViewItem item = SchedulesVw.AddItem(packet.ToString(), ToServer ? "Server" : "Client", Interval.ToString(), Cycles.ToString(), HotkeysText);
            item.Tag = new HSchedule(packet, ToServer, Interval, Cycles, Hotkeys);
        }

        private void SchedulesVw_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // Item has been toggled internally, do not handle this event.
            if (Monitor.IsEntered(e.Item)) return;

            GetSchedule(e.Item)?.ToggleAsync(e.Item.Checked)
                .ContinueWith(t =>
                {
                    if (t == Task.CompletedTask) return;
                    lock (e.Item)
                    {
                        e.Item.Checked = false;
                    } 
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private HSchedule GetSchedule(ListViewItem item) => (HSchedule)item?.Tag;

        #region IHaltable Implementation
        public void Halt()
        { }
        public void Restore(ConnectedEventArgs e)
        { }
        #endregion
    }
}