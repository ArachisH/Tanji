using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

using Tanji.Manipulators;

using Sulakore.Protocol;
using Sulakore.Components;

using static Sulakore.Components.SKoreScheduleView;

namespace Tanji.Pages.Injection
{
    public class SchedulerPage : TanjiSubPage<InjectionPage>, IHaltable
    {
        private bool _suppressUIUpdating;
        private readonly Dictionary<Keys, ListViewItem> _items;
        private readonly Dictionary<ListViewItem, Keys> _hotkeys;

        private ushort _interval = 100;
        public ushort Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                RaiseOnPropertyChanged(nameof(Interval));
            }
        }

        private int _cycles = 0;
        public int Cycles
        {
            get => _cycles;
            set
            {
                _cycles = value;
                RaiseOnPropertyChanged(nameof(Cycles));
            }
        }

        private bool _autoStart = true;
        public bool AutoStart
        {
            get => _autoStart;
            set
            {
                _autoStart = value;
                RaiseOnPropertyChanged(nameof(AutoStart));
            }
        }

        private HDestination _destination = HDestination.Server;
        public HDestination Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                RaiseOnPropertyChanged(nameof(Destination));
            }
        }

        public Keys HotKey { get; private set; }
        public string HotkeyValue { get; private set; }

        public SchedulerPage(InjectionPage parent, TabPage tab)
            : base(parent, tab)
        {
            _items = new Dictionary<Keys, ListViewItem>();
            _hotkeys = new Dictionary<ListViewItem, Keys>();

            UI.Hook.HotkeyActivated += Hook_HotkeyActivated;

            UI.STDestinationTxt.DataSource = Enum.GetValues(typeof(HDestination));
            UI.STDestinationTxt.DataBindings.Add("SelectedItem", this,
                nameof(Destination), false, DataSourceUpdateMode.OnPropertyChanged);

            UI.STCyclesTxt.DataBindings.Add("Value", this,
                nameof(Cycles), false, DataSourceUpdateMode.OnPropertyChanged);

            UI.STIntervalTxt.DataBindings.Add("Value", this,
                nameof(Interval), false, DataSourceUpdateMode.OnPropertyChanged);

            UI.STAutoStartChckbx.DataBindings.Add("Checked", this,
                nameof(AutoStart), false, DataSourceUpdateMode.OnPropertyChanged);

            UI.STClearBtn.Click += STClearBtn_Click;
            UI.STRemoveBtn.Click += STRemoveBtn_Click;
            UI.STCreateBtn.Click += STCreateBtn_Click;

            UI.STHotkeyTxt.Box.KeyDown += STHotkeyTxt_KeyDown;

            UI.STSchedulerVw.ItemChecked += STSchedulerVw_ItemChecked;
            UI.STSchedulerVw.ScheduleTick += STSchedulerVw_ScheduleTick;
            UI.STSchedulerVw.ItemSelectionStateChanged += STSchedulerVw_ItemSelectionStateChanged;

            LoadState();
        }

        private void STClearBtn_Click(object sender, EventArgs e)
        {
            foreach (Keys hotkey in _hotkeys.Values)
                UI.Hook.UnregisterHotkey(hotkey);

            _items.Clear();
            _hotkeys.Clear();

            UI.STSchedulerVw.ClearItems();
            UpdateUI();
            SaveState();
        }
        private void STRemoveBtn_Click(object sender, EventArgs e)
        {
            ListViewItem item = UI.STSchedulerVw.SelectedItem;
            if (_hotkeys.ContainsKey(item))
            {
                Keys hotkey = _hotkeys[item];

                _hotkeys.Remove(item);
                _items.Remove(hotkey);
                UI.Hook.UnregisterHotkey(hotkey);
            }
            UI.STSchedulerVw.RemoveSelectedItem();
            UpdateUI();
            SaveState();
        }
        private void STCreateBtn_Click(object sender, EventArgs e)
        {
            HMessage packet = GetPacket();
            if (!Parent.AuthorizeInjection(packet)) return;

            ListViewItem item = UI.STSchedulerVw.AddSchedule(packet, Interval, Cycles, AutoStart);

            if (HotKey != Keys.None)
            {
                item.SubItems.Add(HotkeyValue);
                UI.Hook.RegisterHotkey(HotKey);

                _hotkeys[item] = HotKey;
                _items[HotKey] = item;
            }

            HotKey = Keys.None;
            UI.STHotkeyTxt.Text = string.Empty;

            SaveState();
        }

        private void Hook_HotkeyActivated(object sender, KeyEventArgs e)
        {
            if (_items.ContainsKey(e.KeyData))
            {
                ListViewItem item = _items[e.KeyData];
                item.Checked = !item.Checked;
            }
        }
        private void STHotkeyTxt_KeyDown(object sender, KeyEventArgs e)
        {
            HotkeyValue = string.Empty;
            if (e.Modifiers != Keys.None)
            {
                HotkeyValue = (e.Modifiers + " + ");
            }

            HotKey = e.KeyData;
            UI.STHotkeyTxt.Text = (HotkeyValue += e.KeyCode);

            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void STSchedulerVw_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateUI();
        }
        private void STSchedulerVw_ScheduleTick(object sender, ScheduleTickEventArgs e)
        {
            if (Parent.SendAsync(e.Packet, e.Cancellation).Result < 6)
            {
                e.Cancel = true;
            }
        }
        private void STSchedulerVw_ItemSelectionStateChanged(object sender, EventArgs e)
        {
            UI.STRemoveBtn.Enabled =
                UI.STSchedulerVw.HasSelectedItem;
        }

        private void UpdateUI()
        {
            if (_suppressUIUpdating) return;
            UI.SchedulesTxt.Text = $"Schedules: {UI.STSchedulerVw.CheckedItems.Count}/{UI.STSchedulerVw.Items.Count}";
        }
        private HMessage GetPacket()
        {
            if (UI.STPacketTxt.Text.StartsWith("{l}{u:"))
            {
                UI.STPacketTxt.Text = UI.STPacketTxt.Text.Trim();
            }
            return new HMessage(UI.STPacketTxt.Text, Destination);
        }

        private void SaveState()
        {
            var schedulerState = new HMessage('S');
            schedulerState.WriteInteger(UI.STSchedulerVw.Items.Count);
            for (int i = 0; i < UI.STSchedulerVw.Items.Count; i++)
            {
                ListViewItem item = UI.STSchedulerVw.Items[i];
                var schedule = (HSchedule)item.Tag;

                schedulerState.WriteInteger(schedule.Packet.Length + 4);
                schedulerState.WriteBytes(schedule.Packet.ToBytes());
                schedulerState.WriteInteger((int)schedule.Packet.Destination);
                schedulerState.WriteShort((ushort)schedule.Interval);
                schedulerState.WriteInteger(schedule.Cycles);

                bool hasHotKey = _hotkeys.TryGetValue(item, out Keys hotKey);
                schedulerState.WriteBoolean(hasHotKey);

                if (hasHotKey)
                {
                    schedulerState.WriteInteger((int)hotKey);
                    schedulerState.WriteString(item.SubItems[4].Text);
                }
            }

            File.WriteAllBytes("Schedules.pkt", schedulerState.ToBytes());
        }
        private void LoadState()
        {
            if (!File.Exists("Schedules.pkt")) return;

            var schedulerState = new HMessage(File.ReadAllBytes("Schedules.pkt"));

            int count = schedulerState.ReadInteger();
            for (int i = 0; i < count; i++)
            {
                var packet = new HMessage(schedulerState.ReadBytes(schedulerState.ReadInteger()))
                {
                    Destination = (HDestination)schedulerState.ReadInteger()
                };

                int interval = schedulerState.ReadShort();
                int cycles = schedulerState.ReadInteger();

                ListViewItem item = UI.STSchedulerVw.AddSchedule(packet, interval, cycles, false);
                if (schedulerState.ReadBoolean())
                {
                    var hotKey = (Keys)schedulerState.ReadInteger();
                    item.SubItems.Add(schedulerState.ReadString());

                    if (hotKey != Keys.None)
                    {
                        UI.Hook.RegisterHotkey(hotKey);
                        _hotkeys[item] = hotKey;
                        _items[hotKey] = item;
                    }
                }
            }
        }

        protected override void OnTabSelecting(TabControlCancelEventArgs e)
        {
            UI.InjectionMenu.InputBox = UI.STPacketTxt;
            base.OnTabSelecting(e);
        }

        #region IHaltable Implementation
        public void Halt()
        {
            try
            {
                _suppressUIUpdating = true;
                foreach (ListViewItem item in UI.STSchedulerVw.Items)
                {
                    item.Checked = false;
                }
            }
            finally
            {
                _suppressUIUpdating = false;
                UpdateUI();
            }
        }
        public void Restore()
        { }
        #endregion
    }
}