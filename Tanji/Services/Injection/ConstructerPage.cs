using System.Windows.Forms;
using System.ComponentModel;

using Tanji.Controls;

namespace Tanji.Services.Injection
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ConstructerPage : ObservablePage
    {
        private ushort _identifier = 0;
        public ushort Identifier
        {
            get => _identifier;
            set
            {
                _identifier = value;
                RaiseOnPropertyChanged();

                Refresh();
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

                Refresh();
            }
        }

        private string _structure = "{id:0}";
        public string Structure
        {
            get => _structure;
            set
            {
                _structure = value;
                RaiseOnPropertyChanged();

                Refresh();
            }
        }

        public ConstructerPage()
        {
            InitializeComponent();

            Bind(IdentifierTxt, "Value", nameof(Identifier));
            Bind(StructureTxt, "Text", nameof(Structure));
            Bind(AmountTxt, "Value", nameof(Amount));
        }

        private void ValueTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            if (!ValuesVw.HasSelectedItem) return;

            string sValue = ValueTxt.Text;
            string type = ValuesVw.SelectedItem.SubItems[0].Text;
            
            Refresh();
        }
        private void ValuesVw_ItemActivate(object sender, System.EventArgs e)
        {
            ValueTxt.Text = ValuesVw.SelectedItem.SubItems[1].Text;
            ValueTxt.Focus();
        }

        private void WriteIntegerBtn_Click(object sender, System.EventArgs e)
        { }
        private void WriteStringBtn_Click(object sender, System.EventArgs e)
        { }
        private void WriteBooleanBtn_Click(object sender, System.EventArgs e)
        { }
    }
}