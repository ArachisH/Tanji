using System;
using System.Windows.Forms;

using Tanji.Manipulators;

using Sulakore.Protocol;

namespace Tanji.Pages.Injection
{
    public class ConstructerPage : TanjiSubPage<InjectionPage>
    {
        private const string INVALID_SINT32_VALUE =
            "The given value is not a valid 32-bit signed integer.";

        private ushort _header = 0;
        public ushort Header
        {
            get { return _header; }
            set
            {
                _header = value;
                RaiseOnPropertyChanged(nameof(Header));

                Refresh();
            }
        }

        private ushort _amount = 1;
        public ushort Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                RaiseOnPropertyChanged(nameof(Amount));

                Refresh();
            }
        }

        public ConstructerPage(InjectionPage parent, TabPage tab)
            : base(parent, tab)
        {
            UI.CTHeaderTxt.DataBindings.Add("Value", this,
                nameof(Header), false, DataSourceUpdateMode.OnPropertyChanged);

            UI.CTAmountTxt.DataBindings.Add("Value", this,
                nameof(Amount), false, DataSourceUpdateMode.OnPropertyChanged);

            UI.CTValueTxt.KeyDown += CTValueTxt_KeyDown;

            UI.CTClearBtn.Click += CTClearBtn_Click;
            UI.CTMoveUpBtn.Click += CTMoveUpBtn_Click;
            UI.CTRemoveBtn.Click += CTRemoveBtn_Click;
            UI.CTMoveDownBtn.Click += CTMoveDownBtn_Click;
            UI.CTTransferBelowBtn.Click += CTTransferBelowBtn_Click;

            UI.CTWriteStringBtn.Click += CTWriteStringBtn_Click;
            UI.CTWriteIntegerBtn.Click += CTWriteIntegerBtn_Click;
            UI.CTWriteBooleanBtn.Click += CTWriteBooleanBtn_Click;

            UI.CTConstructerVw.ItemActivate += CTConstructerVw_ItemActivate;
            UI.CTConstructerVw.ItemSelected += CTConstructerVw_ItemSelected;
            UI.CTConstructerVw.ItemSelectionStateChanged += CTConstructerVw_ItemSelectionStateChanged;
        }

        public void WriteInteger(int value)
        {
            WriteInteger(value, 1);
        }
        public virtual void WriteInteger(int value, int amount)
        {
            UI.CTConstructerVw.WriteInteger(value, amount);
            AddAutocomplete(value.ToString());
            Refresh();
        }

        public void WriteBoolean(bool value)
        {
            WriteBoolean(value, 1);
        }
        public virtual void WriteBoolean(bool value, int amount)
        {
            UI.CTConstructerVw.WriteBoolean(value, amount);
            AddAutocomplete(value.ToString());
            Refresh();
        }

        public void WriteString(string value)
        {
            WriteString(value, 1);
        }
        public void WriteString(string value, int amount)
        {
            UI.CTConstructerVw.WriteString(value, amount);
            AddAutocomplete(value);
            Refresh();
        }

        public void Refresh()
        {
            UI.CTValueCountLbl.Text =
                ($"Value Count: {UI.CTConstructerVw.Values.Count:n0}");

            UI.CTStructureTxt.Text =
                UI.CTConstructerVw.GetStructure(Header);
        }
        public HMessage GetPacket()
        {
            return UI.CTConstructerVw.GetPacket(Header);
        }
        
        private void AddAutocomplete(string value)
        {
            if (!UI.CTValueTxt.Items.Contains(value))
                UI.CTValueTxt.Items.Add(value);
        }
        private object TryParseObject(string type, string value)
        {
            switch (type)
            {
                default:
                case "String":
                return value;

                case "Boolean":
                return (value == "true" || value == "1");

                case "Integer":
                {
                    int iValue = 0;
                    int.TryParse(value, out iValue);
                    return iValue;
                }
            }
        }

        private void CTValueTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            ListViewItem item = UI.CTConstructerVw.SelectedItem;
            if (item == null) return;

            string sValue = UI.CTValueTxt.Text;
            string type = item.SubItems[0].Text;
            object value = TryParseObject(type, sValue);

            UI.CTConstructerVw.UpdateSelectedValue(value);
            UI.CTValueTxt.Text = value.ToString();

            AddAutocomplete(value.ToString());
            Refresh();
        }

        private void CTClearBtn_Click(object sender, EventArgs e)
        {
            UI.CTConstructerVw.ClearItems();
            Refresh();
        }
        private void CTRemoveBtn_Click(object sender, EventArgs e)
        {
            UI.CTConstructerVw.RemoveSelectedItem();
            Refresh();
        }
        private void CTMoveUpBtn_Click(object sender, EventArgs e)
        {
            UI.CTConstructerVw.MoveSelectedItemUp();
            Refresh();
        }
        private void CTMoveDownBtn_Click(object sender, EventArgs e)
        {
            UI.CTConstructerVw.MoveSelectedItemDown();
            Refresh();
        }
        private void CTTransferBelowBtn_Click(object sender, EventArgs e)
        {
            UI.ITPacketTxt.Text = GetPacket().ToString();
        }

        private void CTWriteStringBtn_Click(object sender, EventArgs e)
        {
            WriteString(UI.CTValueTxt.Text, Amount);
        }
        private void CTWriteIntegerBtn_Click(object sender, EventArgs e)
        {
            int value = 0;
            string vString = UI.CTValueTxt.Text;
            if (!string.IsNullOrWhiteSpace(vString) &&
                !int.TryParse(vString, out value))
            {
                MessageBox.Show(INVALID_SINT32_VALUE,
                    "Tanji ~ Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else WriteInteger(value, Amount);
        }
        private void CTWriteBooleanBtn_Click(object sender, EventArgs e)
        {
            string sValue = UI.CTValueTxt.Text.ToLower();
            WriteBoolean(sValue == "true" || sValue == "1", Amount);
        }

        private void CTConstructerVw_ItemActivate(object sender, EventArgs e)
        {
            UI.CTValueTxt.Text = UI.CTConstructerVw.
                SelectedItem.SubItems[1].Text;

            UI.CTValueTxt.Focus();
        }
        private void CTConstructerVw_ItemSelected(object sender, EventArgs e)
        {
            UI.CTMoveUpBtn.Enabled = UI.CTConstructerVw.CanMoveSelectedItemUp;
            UI.CTMoveDownBtn.Enabled = UI.CTConstructerVw.CanMoveSelectedItemDown;
        }
        private void CTConstructerVw_ItemSelectionStateChanged(object sender, EventArgs e)
        {
            UI.CTRemoveBtn.Enabled = UI.CTConstructerVw.HasSelectedItem;

            if (!UI.CTConstructerVw.HasSelectedItem)
                CTConstructerVw_ItemSelected(sender, e);
        }
    }
}