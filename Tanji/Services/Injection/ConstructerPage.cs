using System;
using System.Windows.Forms;
using System.ComponentModel;

using Tanji.Controls;
using System.DirectoryServices.ActiveDirectory;

namespace Tanji.Services.Injection
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ConstructerPage : NotifiablePage
    {
        private ushort _id = 1;
        public ushort Id
        {
            get => _id;
            set
            {
                _id = value;
                RaiseOnPropertyChanged();
                Dismantle(true);
            }
        }

        private ushort _amount = 1;
        public ushort Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                RaiseOnPropertyChanged();
            }
        }

        private string _value = string.Empty;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                RaiseOnPropertyChanged();
                Refresh();
            }
        }

        public ConstructerPage()
        {
            InitializeComponent();

            ValueTxt.Box.AutoCompleteMode = AutoCompleteMode.Append;
            ValueTxt.Box.AutoCompleteSource = AutoCompleteSource.CustomSource;

            Bind(IDTxt, "Text", nameof(Id));
            Bind(ValueTxt, "Text", nameof(Value));
            Bind(AmountTxt, "Text", nameof(Amount));
        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            ValuesVw.Items.Clear();
            Dismantle();
        }
        private void WriteIntegerBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(Value, out int value))
            {
                WriteObject("Integer", value);
            }
        }
        private void WriteBooleanBtn_Click(object sender, EventArgs e)
        {
            string value = Value.ToLower();
            WriteObject("Boolean", value == "true" || value == "1");
        }
        private void WriteStringBtn_Click(object sender, EventArgs e) => WriteObject("String", Value);

        private void CopyBtn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(DismantledTxt.Text);
        }
        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            ValuesVw.SelectedItem.Remove();
            Dismantle();
        }
        private void MoveUpBtn_Click(object sender, EventArgs e)
        {
            ValuesVw.MoveSelectedItemUp();
            Dismantle();
        }
        private void MoveDownBtn_Click(object sender, EventArgs e)
        {
            ValuesVw.MoveSelectedItemDown();
            Dismantle();
        }

        private void ValueTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (!ValuesVw.HasSelectedItem) return;

            ValuesVw.SelectedItem.SubItems[1].Text = Value;
            Dismantle();
        }
        private void ValuesVw_ItemActivate(object sender, EventArgs e)
        {
            Value = ValuesVw.SelectedItem.SubItems[1].Text;
            ValueTxt.Focus();
        }
        private void ValuesVw_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            RemoveBtn.Enabled = ValuesVw.HasSelectedItem;
            MoveUpBtn.Enabled = ValuesVw.CanMoveSelectedItemUp;
            MoveDownBtn.Enabled = ValuesVw.CanMoveSelectedItemDown;
        }

        private void Dismantle(bool idOnly = false)
        {
            string dismantled = $"{{id:{Id}}}";
            if (!idOnly)
            {
                foreach (ListViewItem item in ValuesVw.Items)
                {
                    dismantled += $"{{{char.ToLower(item.Text[0])}:{item.Tag}}}";
                }
            }
            else if (!string.IsNullOrWhiteSpace(DismantledTxt.Text))
            {
                int endIdIndex = DismantledTxt.Text.IndexOf('}');
                dismantled += DismantledTxt.Text.Substring(endIdIndex + 1);
            }
            DismantledTxt.Text = dismantled;
        }
        private void WriteObject(string typeName, object value)
        {
            if (Amount > 1)
            {
                var subItems = new string[2] { typeName, value.ToString() };
                var items = new ListViewItem[Amount];
                for (int i = 0; i < Amount; i++)
                {
                    items[i] = new ListViewItem(subItems) { Tag = value };
                }
                ValuesVw.Items.AddRange(items);
                ValuesVw.EnsureVisible(ValuesVw.Items.Count - 1);
            }
            else ValuesVw.AddItem(typeName, value.ToString()).Tag = value;

            Dismantle();
            ValueTxt.Box.AutoCompleteCustomSource.Add(value.ToString());
        }
    }
}