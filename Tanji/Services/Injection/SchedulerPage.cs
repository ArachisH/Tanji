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
        private int _previousDraggedItemIndex;

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

            ChainedToPreviousChbx.CheckedChanged += ChainToPreviousChbx_CheckedChanged;

            if (Master != null)
            {
                Master.Hook.HotkeyPressed += Hook_HotkeyPressed;
            }

#if INTERFACEDEBUG
            HSchedule previousSchedule = null;
            for (int i = 0; i < 10; i++)
            {
                var packet = new EvaWirePacket(4000, Guid.NewGuid().ToString()[..8]);
                HSchedule schedule = AddSchedule(packet, true, 150 * (i + 1), 0, Keys.None, string.Empty);
                if (i >= 6)
                {
                    schedule.IsLinkActivated = true;
                    SchedulesVw.Items[i].Checked = true;
                    previousSchedule.AddToChain(schedule);
                }
                else previousSchedule = schedule;
                SchedulesVw.Items[i].ForeColor = schedule.IsChainLink ? Color.FromArgb(243, 63, 63) : Color.Black;
            }
#endif
        }

        private void SchedulerCxm_Opening(object sender, CancelEventArgs e)
        {
            lock (ChainedToPreviousChbx)
            {
                ChainedToPreviousChbx.Enabled = SchedulesVw.HasSelectedItem && SchedulesVw.SelectedItem.Index != 0;
                ChainedToPreviousChbx.Checked = GetSchedule(SchedulesVw.SelectedItem)?.IsChainLink ?? false;
            }
        }
        private void ChainToPreviousChbx_CheckedChanged(object sender, EventArgs e)
        {
            if (Monitor.IsEntered(ChainedToPreviousChbx)) return;

            HSchedule schedule = GetSchedule(SchedulesVw.SelectedItem);
            for (int i = SchedulesVw.SelectedItem.Index - 1; i >= 0; i--)
            {
                HSchedule previousSchedule = GetSchedule(SchedulesVw.Items[i]);
                if (previousSchedule.IsChainLink) continue;

                if (ChainedToPreviousChbx.Checked)
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

            AddSchedule(packet, ToServer, Interval, Cycles, Hotkeys, HotkeysText);
        }

        private void SchedulesVw_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _previousDraggedItemIndex = ((ListViewItem)e.Item).Index;
        }
        private void SchedulesVw_ItemDragged(object sender, ItemDragEventArgs e)
        {
            var item = (ListViewItem)e.Item;
            HSchedule schedule = GetSchedule(item);

            if (!schedule.IsChainLink && schedule.Chain.Count > 0)
            {
                for (int i = 0; i < schedule.Chain.Count; i++)
                {
                    HSchedule chainedSchedule = schedule.Chain[i];
                    chainedSchedule.PhysicalItem.Remove();
                    SchedulesVw.Items.Insert(item.Index + i + 1, chainedSchedule.PhysicalItem);
                }
            }
            else if (schedule.IsChainLink)
            {
                item.Checked = false;
                schedule.Parent.RemoveFromChain(schedule);
                schedule.PhysicalItem.ForeColor = Color.Black;
            }


            if (item.Index != SchedulesVw.Items.Count - 1)
            {
                ListViewItem nextItem = SchedulesVw.Items[item.Index + 1];
                HSchedule nextSchedule = GetSchedule(nextItem);

                if (nextSchedule.IsChainLink)
                {
                    nextSchedule.Parent.AddToChain(schedule, nextSchedule.Parent.GetChainedScheduleIndex(nextSchedule));

                    item.Checked = true;
                    item.ForeColor = Color.FromArgb(243, 63, 63);
                }
            }
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
        private HSchedule AddSchedule(HPacket packet, bool toServer, int interval, int cycles, Keys hotkeys, string hotkeysText)
        {
            ListViewItem item = SchedulesVw.AddItem(packet.ToString(), toServer ? "Server" : "Client", interval.ToString(), cycles.ToString(), hotkeysText);
            var schedule = new HSchedule(packet, toServer, interval, cycles, hotkeys, item);

            item.Tag = schedule;
            return schedule;
        }

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