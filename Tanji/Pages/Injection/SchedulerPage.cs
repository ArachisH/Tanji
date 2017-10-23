using System;
using System.Windows.Forms;
using System.Collections.Generic;

using Tanji.Manipulators;

using Sulakore.Protocol;
using Sulakore.Components;

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
            get { return _interval; }
            set
            {
                _interval = value;
                RaiseOnPropertyChanged(nameof(Interval));
            }
        }

        private int _cycles = 0;
        public int Cycles
        {
            get { return _cycles; }
            set
            {
                _cycles = value;
                RaiseOnPropertyChanged(nameof(Cycles));
            }
        }

        private bool _autoStart = true;
        public bool AutoStart
        {
            get { return _autoStart; }
            set
            {
                _autoStart = value;
                RaiseOnPropertyChanged(nameof(AutoStart));
            }
        }

        private HDestination _destination = HDestination.Server;
        public HDestination Destination
        {
            get { return _destination; }
            set
            {
                _destination = value;
                RaiseOnPropertyChanged(nameof(Destination));
            }
        }

        public Keys Hotkey { get; private set; }
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
        }

        private void STClearBtn_Click(object sender, EventArgs e)
        {
            foreach (Keys hotkey in _hotkeys.Values)
                UI.Hook.UnregisterHotkey(hotkey);

            _items.Clear();
            _hotkeys.Clear();

            UI.STSchedulerVw.ClearItems();
            UpdateUI();
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
        }
        private void STCreateBtn_Click(object sender, EventArgs e)
        {
            HMessage packet = GetPacket();
            if (Parent.AuthorizeInjection(packet))
            {
                ListViewItem item = UI.STSchedulerVw
                    .AddSchedule(packet, Interval, Cycles, AutoStart);

                item.SubItems.Add(HotkeyValue);
                if (Hotkey != Keys.None)
                {
                    UI.Hook.RegisterHotkey(Hotkey);
                    _hotkeys[item] = Hotkey;
                    _items[Hotkey] = item;
                }

                Hotkey = Keys.NoName;
                UI.STHotkeyTxt.Value = string.Empty;
            }
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
                HotkeyValue = (e.Modifiers + " + ");

            Hotkey = e.KeyData;
            UI.STHotkeyTxt.Value = (HotkeyValue += e.KeyCode);

            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void STSchedulerVw_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateUI();
        }
        private void STSchedulerVw_ScheduleTick(object sender, ScheduleTickEventArgs e)
        {
            if (Parent.SendAsync(e.Packet).Result < 6)
                e.Cancel = true;
        }
        private void STSchedulerVw_ItemSelectionStateChanged(object sender, EventArgs e)
        {
            UI.STRemoveBtn.Enabled =
                UI.STSchedulerVw.HasSelectedItem;
        }

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
        public HMessage GetPacket()
        {
            return new HMessage(
                UI.STPacketTxt.Text, Destination);
        }

        private void UpdateUI()
        {
            if (!_suppressUIUpdating)
            {
                UI.SchedulesTxt.Text =
                    $"Schedules: {UI.STSchedulerVw.CheckedItems.Count}/{UI.STSchedulerVw.Items.Count}";
            }
        }

        protected override void OnTabSelecting(TabControlCancelEventArgs e)
        {
            UI.InjectionMenu.InputBox = UI.STPacketTxt;
            base.OnTabSelecting(e);
        }
    }
}