using System;
using System.Windows.Forms;
using System.Collections.Generic;

using Tanji.Components;

namespace Tanji.Applications.Dialogs
{
    public partial class IgnoreMessagesDialog : TanjiForm
    {
        private readonly IDictionary<ushort, bool> _ignoredOutgoing, _ignoredIncoming;

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

        public IgnoreMessagesDialog(
            IDictionary<ushort, bool> ignoredOutgoing,
            IDictionary<ushort, bool> ignoredIncoming)
        {
            _ignoredOutgoing = ignoredOutgoing;
            _ignoredIncoming = ignoredIncoming;

            InitializeComponent();
            DisplayIgnoredMessages("Outgoing", _ignoredOutgoing);
            DisplayIgnoredMessages("Incoming", _ignoredIncoming);

            HeaderTxt.DataBindings.Add("Value", this,
                nameof(Header), false, DataSourceUpdateMode.OnPropertyChanged);

            TypeTxt.SelectedIndex = 0;

            IgnoredVw.ItemChecked += IgnoredVw_ItemChecked;
            IgnoredVw.ItemSelectionStateChanged += IgnoredVw_ItemSelectionStateChanged;
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            ListViewItem item = IgnoredVw.SelectedItem;
            var ignoredMsg = (item.Tag as Tuple<ushort, bool>);
            if (ignoredMsg != null)
            {
                ushort header = ignoredMsg.Item1;
                bool isOutgoing = ignoredMsg.Item2;

                IDictionary<ushort, bool> ignored = (isOutgoing ?
                    _ignoredOutgoing : _ignoredIncoming);

                if (ignored.ContainsKey(header))
                    ignored.Remove(header);
            }
            IgnoredVw.RemoveSelectedItem();
        }
        private void AddHeaderBtn_Click(object sender, EventArgs e)
        {
            IgnoreMessage(Header, (TypeTxt.Text == "Outgoing"));
        }

        private void IgnoredVw_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var ignoredMsg = (e.Item.Tag as Tuple<ushort, bool>);
            if (ignoredMsg != null)
            {
                ushort header = ignoredMsg.Item1;
                bool isOutgoing = ignoredMsg.Item2;

                IDictionary<ushort, bool> ignored = (isOutgoing ?
                    _ignoredOutgoing : _ignoredIncoming);

                ignored[header] = e.Item.Checked;
            }
        }
        private void IgnoredVw_ItemSelectionStateChanged(object sender, EventArgs e)
        {
            RemoveBtn.Enabled = IgnoredVw.HasSelectedItem;
        }

        private void IgnoreMessage(ushort header, bool isOutgoing)
        {
            IDictionary<ushort, bool> ignored = (isOutgoing ?
                _ignoredOutgoing : _ignoredIncoming);

            if (!ignored.ContainsKey(header))
            {
                ignored[header] = true;

                ListViewItem item = IgnoredVw.AddItem(
                    (isOutgoing ? "Outgoing" : "Incoming"), header);

                item.Checked = true;
                item.Tag = new Tuple<ushort, bool>(header, isOutgoing);
            }
        }
        private void DisplayIgnoredMessages(string type, IDictionary<ushort, bool> ignored)
        {
            foreach (ushort header in ignored.Keys)
            {
                ListViewItem item = IgnoredVw.AddItem(type, header);
                item.Tag = new Tuple<ushort, bool>(header, (type == "Outgoing"));
                item.Checked = ignored[header];
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            CenterToParent();
            base.OnLoad(e);
        }
    }
}