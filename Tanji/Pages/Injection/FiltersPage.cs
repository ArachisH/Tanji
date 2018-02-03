using System;
using System.Windows.Forms;
using System.Collections.Generic;

using Tanji.Manipulators;

using Sulakore.Protocol;
using Sulakore.Communication;

namespace Tanji.Pages.Injection
{
    public enum FilterAction
    {
        Block = 0,
        Replace = 1
    }

    public class FiltersPage : TanjiSubPage<InjectionPage>, IReceiver, IHaltable
    {
        private bool _suppressUIUpdating;

        private readonly Dictionary<HDestination, List<ushort>> _blocked, _disabled;
        private readonly Dictionary<HDestination, Dictionary<ushort, HMessage>> _replacements;

        private ushort _header = 0;
        public ushort Header
        {
            get { return _header; }
            set
            {
                _header = value;
                RaiseOnPropertyChanged(nameof(Header));
            }
        }

        private FilterAction _action = FilterAction.Block;
        public FilterAction Action
        {
            get { return _action; }
            set
            {
                _action = value;
                RaiseOnPropertyChanged(nameof(Action));
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

        public bool IsReceiving { get; } = true;

        public FiltersPage(InjectionPage parent, TabPage tab)
            : base(parent, tab)
        {
            _blocked = new Dictionary<HDestination, List<ushort>>();
            _blocked[HDestination.Client] = new List<ushort>();
            _blocked[HDestination.Server] = new List<ushort>();

            _disabled = new Dictionary<HDestination, List<ushort>>();
            _disabled[HDestination.Client] = new List<ushort>();
            _disabled[HDestination.Server] = new List<ushort>();

            _replacements = new Dictionary<HDestination, Dictionary<ushort, HMessage>>();
            _replacements[HDestination.Client] = new Dictionary<ushort, HMessage>();
            _replacements[HDestination.Server] = new Dictionary<ushort, HMessage>();

            UI.FTActionTxt.DataSource = Enum.GetValues(typeof(FilterAction));
            UI.FTDestinationTxt.DataSource = Enum.GetValues(typeof(HDestination));

            UI.FTHeaderTxt.DataBindings.Add("Value", this,
                nameof(Header), false, DataSourceUpdateMode.OnPropertyChanged);

            UI.FTActionTxt.DataBindings.Add("SelectedItem", this,
                nameof(Action), false, DataSourceUpdateMode.OnPropertyChanged);

            UI.FTDestinationTxt.DataBindings.Add("SelectedItem", this,
                nameof(Destination), false, DataSourceUpdateMode.OnPropertyChanged);

            UI.FTActionTxt.SelectedValueChanged += FTActionTxt_SelectedValueChanged;

            UI.FTCreateBtn.Click += FTCreateBtn_Click;
            UI.FTRemoveBtn.Click += FTRemoveBtn_Click;

            UI.FTFiltersVw.ItemChecked += FTFiltersVw_ItemChecked;
            UI.FTFiltersVw.ItemSelectionStateChanged += FTFiltersVw_ItemSelectionStateChanged;
        }

        private void FTCreateBtn_Click(object sender, EventArgs e)
        {
            if (!AuthorizeFilter(Header, Destination, Action))
                return;

            HMessage packet = null;
            if (Action == FilterAction.Replace)
            {
                packet = GetPacket();
                if (!Parent.ValidatePacket(packet)) return;
                _replacements[Destination][Header] = packet;
            }
            else _blocked[Destination].Add(Header);

            ListViewItem item = UI.FTFiltersVw.AddFocusedItem(
                Header, Destination, Action, (packet?.ToString() ?? string.Empty));

            var filter = new Tuple<HDestination, ushort>(Destination, Header);
            item.Checked = true;
            item.Tag = filter;

            Refresh();
        }
        private void FTRemoveBtn_Click(object sender, EventArgs e)
        {
            ListViewItem item = UI.FTFiltersVw.SelectedItem;
            var filter = (item.Tag as Tuple<HDestination, ushort>);
            if (filter == null) return;

            ushort header = filter.Item2;
            HDestination destination = filter.Item1;

            if (_blocked[destination].Contains(header))
                _blocked[destination].Remove(header);

            if (_replacements[destination].ContainsKey(header))
                _replacements[destination].Remove(header);

            if (_disabled[destination].Contains(header))
                _disabled[destination].Remove(header);

            UI.FTFiltersVw.RemoveSelectedItem();
            Refresh();
        }

        private void FTActionTxt_SelectedValueChanged(object sender, EventArgs e)
        {
            UI.FTReplacementTxt.IsReadOnly = ((FilterAction)UI.FTActionTxt.SelectedValue == FilterAction.Block);
        }

        private void FTFiltersVw_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var filter = (e.Item.Tag as Tuple<HDestination, ushort>);
            if (filter == null) return;

            List<ushort> disabled = _disabled[filter.Item1];
            if (e.Item.Checked)
            {
                if (disabled.Contains(filter.Item2))
                    disabled.Remove(filter.Item2);
            }
            else if (!disabled.Contains(filter.Item2))
                disabled.Add(filter.Item2);

            Refresh();
        }
        private void FTFiltersVw_ItemSelectionStateChanged(object sender, EventArgs e)
        {
            UI.FTRemoveBtn.Enabled =
                UI.FTFiltersVw.HasSelectedItem;
        }
        
        public void Refresh()
        {
            if (!_suppressUIUpdating)
            {
                UI.FiltersTxt.Text =
                    $"Filters: {UI.FTFiltersVw.CheckedItems.Count}/{UI.FTFiltersVw.Items.Count}";
            }
        }
        public HMessage GetPacket()
        {
            return new HMessage(UI.FTReplacementTxt.Text, Destination);
        }
        public bool AuthorizeFilter(ushort key, HDestination destination, FilterAction action)
        {
            return !_blocked[destination].Contains(key) &&
                !_replacements[destination].ContainsKey(key);
        }

        public void HandleOutgoing(DataInterceptedEventArgs e) => HandleFilter(e);
        public void HandleIncoming(DataInterceptedEventArgs e) => HandleFilter(e);

        private void HandleFilter(DataInterceptedEventArgs e)
        {
            HMessage packet = e.Packet;
            List<ushort> blocked = _blocked[packet.Destination];
            List<ushort> disabled = _disabled[packet.Destination];
            Dictionary<ushort, HMessage> replacements = _replacements[packet.Destination];

            if (disabled.Contains(packet.Header)) return;
            else if (blocked.Contains(packet.Header)) e.IsBlocked = true;
            else if (replacements.ContainsKey(packet.Header))
            {
                e.Packet = replacements[packet.Header];
            }
        }

        protected override void OnTabSelecting(TabControlCancelEventArgs e)
        {
            UI.InjectionMenu.InputBox = UI.FTReplacementTxt.Box;
            base.OnTabSelecting(e);
        }

        #region IHaltable Implementation
        public void Halt()
        {
            try
            {
                _suppressUIUpdating = true;
                foreach (ListViewItem item in UI.FTFiltersVw.Items)
                {
                    item.Checked = false;
                }
            }
            finally
            {
                _suppressUIUpdating = false;
                Refresh();
            }
        }
        public void Restore()
        { }
        #endregion
    }
}