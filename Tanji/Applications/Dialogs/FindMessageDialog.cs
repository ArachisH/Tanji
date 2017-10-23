using System;
using System.Windows.Forms;
using System.Collections.Generic;

using Tanji.Components;

using Tangine.Habbo;

using FlashInspect.ActionScript;

namespace Tanji.Applications.Dialogs
{
    public partial class FindMessageDialog : TanjiForm
    {
        private readonly HGame _game;

        private string _hash = string.Empty;
        public string Hash
        {
            get { return _hash; }
            set
            {
                _hash = value;
                RaiseOnPropertyChanged(nameof(Hash));
            }
        }

        public FindMessageDialog(HGame game)
        {
            _game = game;
            InitializeComponent();

            HashTxt.DataBindings.Add("Text", this,
                nameof(Hash), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void FindBtn_Click(object sender, EventArgs e)
        {
            HeadersVw.ClearItems();
            _game.GenerateMessageHashes();
            IEnumerable<ASClass> messages = _game.GetMessages(Hash);
            if (messages == null)
            {
                MessageBox.Show("Cannot find any Outgoing/Incoming messages that are associated with this hash.",
                    "Tanji ~ Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                HashTxt.Select();
                HashTxt.SelectAll();
                return;
            }

            foreach (ASClass msgClass in messages)
            {
                ushort header = _game.GetMessageHeader(msgClass);
                bool isOutgoing = _game.IsMessageOutgoing(msgClass);
                string messageName = msgClass.Instance.Name.Name;

                string type = "Outgoing";
                if (!isOutgoing)
                {
                    type = "Incoming";
                    messageName += (", " + _game.GetIncomingMessageParser(
                        msgClass).Instance.Name.Name);
                }
                ListViewItem item = HeadersVw.AddFocusedItem(type, header, messageName);
                item.Tag = msgClass; // Display the message/class information? GG m8?
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            CenterToParent();
            base.OnLoad(e);
        }
    }
}