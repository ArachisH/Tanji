using System;
using System.Drawing;
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

            ChainToPreviousChbx.CheckedChanged += ChainToPreviousChbx_CheckedChanged;

            if (Master != null)
            {
                Master.Hook.HotkeyPressed += Hook_HotkeyPressed;
            }
        }

        private void SchedulerCxm_Opening(object sender, CancelEventArgs e)
        {
            lock (ChainToPreviousChbx)
            {
                ChainToPreviousChbx.Enabled = SchedulesVw.HasSelectedItem && SchedulesVw.SelectedItem.Index != 0;
                ChainToPreviousChbx.Checked = GetSchedule(SchedulesVw.SelectedItem)?.IsChainLink ?? false;
            }
        }
        private void ChainToPreviousChbx_CheckedChanged(object sender, EventArgs e)
        {
            if (Monitor.IsEntered(ChainToPreviousChbx)) return;

            HSchedule schedule = GetSchedule(SchedulesVw.SelectedItem);
            for (int i = SchedulesVw.SelectedItem.Index - 1; i >= 0; i--)
            {
                HSchedule previousSchedule = GetSchedule(SchedulesVw.Items[i]);
                if (previousSchedule.IsChainLink) continue;

                // If 'IsChainLink' is true, the 'Checked' event will only change the 'IsLinkActivated' property, and will not run independently of parent link.
                if (ChainToPreviousChbx.Checked)
                {
                    previousSchedule.AddToChain(schedule);
                    SchedulesVw.SelectedItem.Checked = true;
                }
                else
                {
                    SchedulesVw.SelectedItem.Checked = false;
                    previousSchedule.RemoveFromChain(schedule);
                }
                break;
            }
            SchedulesVw.SelectedItem.ForeColor = schedule.IsChainLink ? Color.FromArgb(243, 63, 63) : Color.Black;
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

        private void SchedulesVw_ItemDragged(object sender, EventArgs e)
        {
            // TODO: Moved schedule within the chain if need be.
        }
        private void SchedulesVw_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // Item has been toggled internally, do not handle this event.
            if (Monitor.IsEntered(e.Item)) return;

            HSchedule schedule = GetSchedule(e.Item);
            if (schedule == null) return;

            // This schedule is part of a chain, only signify that this schedule is allowed to be ran internally by the parent.
            if (schedule.IsChainLink)
            {
                schedule.IsLinkActivated = e.Item.Checked;
                return;
            }

            // Schedule has been toggled/checked, undo it if there is currently no established connection.
#if !INTERFACEDEBUG
            if (e.Item.Checked && !Program.Master.IsConnected)
            {
                lock (e.Item)
                {
                    e.Item.Checked = false;
                }
                return;
            }
#endif

            // This schedule will run, and toggle the item's checkbox based on its' state.
            schedule?.ToggleAsync(e.Item.Checked)
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
        {
            foreach (ListViewItem item in SchedulesVw.Items)
            {
                HSchedule schedule = GetSchedule(item);
                schedule.ToggleAsync(false);
            }
        }
        public void Restore(ConnectedEventArgs e)
        { }
        #endregion
    }
}