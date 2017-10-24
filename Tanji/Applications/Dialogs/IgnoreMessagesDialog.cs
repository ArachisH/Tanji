using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Tanji.Components;

namespace Tanji.Applications.Dialogs
{
    [DesignerCategory("Form")]
    public partial class IgnoreMessagesDialog : ObservableForm
    {
        private readonly Dictionary<int, bool> _ignoredMessages;

        private ushort _id = 0;
        public ushort Id
        {
            get => _id;
            set
            {
                _id = value;
                RaiseOnPropertyChanged();
            }
        }

        public IgnoreMessagesDialog(Dictionary<int, bool> ignoredMessages)
        {
            _ignoredMessages = ignoredMessages;

            InitializeComponent();

            IgnoredVw.BeginUpdate();
            foreach (int container in ignoredMessages.Keys)
            {
                var id = (ushort)((container >> 1) & ushort.MaxValue);
                bool isOutgoing = ((container & 1) == 1);

                string type = (isOutgoing ? "Outgoing" : "Incoming");
                ListViewItem item = IgnoredVw.AddItem(type, id);

                item.Checked = ignoredMessages[id];
            }
            IgnoredVw.EndUpdate();

            Bind(HeaderTxt, "Value", nameof(Id));

            TypeTxt.SelectedIndex = 0;
            IgnoredVw.ItemChecked += IgnoredVw_ItemChecked;
            IgnoredVw.ItemSelectionStateChanged += IgnoredVw_ItemSelectionStateChanged;
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            var container = (ushort)IgnoredVw.SelectedItem.Tag;
            if (_ignoredMessages.ContainsKey(container))
            {
                _ignoredMessages.Remove(container);
            }
            IgnoredVw.RemoveSelectedItem();
        }
        private void AddHeaderBtn_Click(object sender, EventArgs e)
        {
            ushort container = 0;
            bool isOutgoing = (TypeTxt.Text == "Outgoing");
            container |= (ushort)((Id & ushort.MaxValue) << 1);
            container |= (ushort)(isOutgoing ? 1 : 0);

            if (!_ignoredMessages.ContainsKey(container))
            {
                _ignoredMessages[container] = true;

                var item = new ListViewItem(new string[] { TypeTxt.Text, Id.ToString() });
                item.Tag = container;
                item.Checked = true;

                IgnoredVw.AddItem(item);
            }
        }

        private void IgnoredVw_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            var container = (ushort)e.Item.Tag;
            _ignoredMessages[container] = e.Item.Checked;
        }
        private void IgnoredVw_ItemSelectionStateChanged(object sender, EventArgs e)
        {
            RemoveBtn.Enabled = IgnoredVw.HasSelectedItem;
        }

        protected override void OnLoad(EventArgs e)
        {
            CenterToParent();
            base.OnLoad(e);
        }
    }
}