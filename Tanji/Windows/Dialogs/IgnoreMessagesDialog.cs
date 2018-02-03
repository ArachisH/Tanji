using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Tanji.Components;

namespace Tanji.Windows.Dialogs
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
            foreach (int identifier in ignoredMessages.Keys)
            {
                bool isOutgoing = (identifier >= ushort.MaxValue);
                int id = (isOutgoing ? (identifier - ushort.MaxValue) : identifier);

                var item = new ListViewItem(new string[]
                {
                    (isOutgoing ? "Outgoing" : "Incoming"),
                    id.ToString()
                });
                item.Tag = identifier;
                item.Checked = ignoredMessages[identifier];

                IgnoredVw.AddItem(item);
            }
            IgnoredVw.EndUpdate();

            Bind(HeaderTxt, "Value", nameof(Id));

            TypeTxt.SelectedIndex = 0;
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            var identifier = (int)IgnoredVw.SelectedItem.Tag;
            if (_ignoredMessages.ContainsKey(identifier))
            {
                _ignoredMessages.Remove(identifier);
            }
            IgnoredVw.RemoveSelectedItem();
        }
        private void IgnoreBtn_Click(object sender, EventArgs e)
        {
            bool isOutgoing = (TypeTxt.Text == "Outgoing");
            int identifier = (isOutgoing ? (Id + ushort.MaxValue) : Id);

            if (!_ignoredMessages.ContainsKey(identifier))
            {
                _ignoredMessages.Add(identifier, true);
                var item = new ListViewItem(new string[]
                {
                    (isOutgoing ? "Outgoing" : "Incoming"),
                    Id.ToString()
                });

                item.Checked = true;
                item.Tag = identifier;
                IgnoredVw.AddItem(item);
            }
        }

        private void IgnoredVw_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            _ignoredMessages[(int)e.Item.Tag] = e.Item.Checked;
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