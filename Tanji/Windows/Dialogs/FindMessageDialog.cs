using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Tanji.Components;

using Sulakore.Habbo;
using Sulakore.Habbo.Messages;

namespace Tanji.Windows.Dialogs
{
    [DesignerCategory("Form")]
    public partial class FindMessageDialog : ObservableForm
    {
        private readonly MainFrm _mainUI;

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

        public FindMessageDialog(MainFrm mainUI, string selectedText)
        {
            _mainUI = mainUI;

            InitializeComponent();
            PropertyChanged += FindMessageDialog_PropertyChanged;

            // TODO: Implement 'Find By ID'
            //Bind(IdNum, "Value", nameof(Id));
            Bind(HashTxt, "Text", nameof(Hash));
        }

        private void FindBtn_Click(object sender, EventArgs e)
        {
            HeadersVw.ClearItems();
            if (!TryDisplayMessages(Hash))
            {
                HashTxt.Select();
                HashTxt.Box.SelectAll();
                return;
            }
        }
        private void HeadersVw_ItemSelected(object sender, EventArgs e)
        {
            Console.WriteLine(nameof(HeadersVw_ItemSelected));
        }
        private void FindMessageDialog_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Id):
                {
                    TryDisplayMessages(Id);
                    //IdNum.Focus();
                    break;
                }
            }
        }

        private bool TryDisplayMessages(ushort id)
        {
            HeadersVw.Items.Clear();
            if (_mainUI.Game.InMessages.TryGetValue(id, out MessageItem inMessage))
            {
                DisplayMessage(inMessage);
            }
            if (_mainUI.Game.OutMessages.TryGetValue(id, out MessageItem outMessage))
            {
                DisplayMessage(outMessage);
            }
            return (inMessage != null || outMessage != null);
        }
        private bool TryDisplayMessages(string hash)
        {
            HeadersVw.Items.Clear();
            if (!_mainUI.Game.Messages.TryGetValue(hash, out List<MessageItem> messages)) return false;

            ListViewItem lastItem = null;
            foreach (MessageItem message in messages)
            {
                lastItem = DisplayMessage(message);
            }
            return true;
        }
        private ListViewItem DisplayMessage(MessageItem message)
        {
            string type = (message.IsOutgoing ? "Outgoing" : "Incoming");
            var identifiers = (message.IsOutgoing ? _mainUI.Out : (Identifiers)_mainUI.In);
            ListViewItem item = HeadersVw.AddItem(type, message.Id, "<Unknown>", message.Class.QName.Name);

            string name = identifiers.GetName(message.Hash);
            if (!string.IsNullOrWhiteSpace(name))
            {
                item.SubItems[2].Text = name;
            }

            if (!message.IsOutgoing)
            {
                item.SubItems.Add(message.Parser.QName.Name);
            }
            return item;
        }

        protected override void OnLoad(EventArgs e)
        {
            CenterToParent();
            base.OnLoad(e);
        }
    }
}