using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Tanji.Components;

using Sulakore.Habbo;

namespace Tanji.Applications.Dialogs
{
    [DesignerCategory("Form")]
    public partial class FindMessageDialog : ObservableForm
    {
        private readonly HGame _game;

        private string _hash = string.Empty;
        public string Hash
        {
            get => _hash;
            set
            {
                _hash = value;
                RaiseOnPropertyChanged();
            }
        }

        public FindMessageDialog(HGame game, string hash)
        {
            _game = game;

            InitializeComponent();

            Bind(HashTxt, "Text", nameof(Hash));

            Hash = hash;
            HashTxt.SelectAll();
        }

        private void FindBtn_Click(object sender, EventArgs e)
        {
            HeadersVw.ClearItems();

            List<MessageItem> messages = null;
            if (!_game.Messages.TryGetValue(Hash, out messages))
            {
                MessageBox.Show("Unable to find any messages that are associated with the specified hash.",
                    "Tanji - Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                HashTxt.Select();
                HashTxt.SelectAll();
                return;
            }

            foreach (MessageItem message in messages)
            {
                string type = (message.IsOutgoing ? "Outgoing" : "Incoming");
                ListViewItem item = HeadersVw.AddFocusedItem(type, message.Id, message.Class.QName.Name);
                item.Tag = message;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            CenterToParent();
            base.OnLoad(e);
        }
    }
}