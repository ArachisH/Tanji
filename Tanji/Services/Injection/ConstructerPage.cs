using System.ComponentModel;

using Tanji.Controls;

namespace Tanji.Services.Injection
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ConstructerPage : ObservablePage
    {
        private ushort _header = 0;
        public ushort Header
        {
            get => _header;
            set
            {
                _header = value;
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

        public ConstructerPage()
        {
            InitializeComponent();

            Bind(IdentifierTxt, "Value", nameof(Header));
            Bind(AmountTxt, "Value", nameof(Amount));
        }

        private void ValuesVw_ItemActivate(object sender, System.EventArgs e)
        {
            ValueTxt.Text = ValuesVw.SelectedItem.SubItems[1].Text;
            ValueTxt.Focus();
        }
    }
}