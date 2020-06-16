using System;
using System.Windows.Forms;
using System.ComponentModel;

using Tanji.Network;
using Tanji.Controls;

namespace Tanji.Services.Injection
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class SchedulerPage : NotifiablePage, IHaltable
    {
        private string _packetText = string.Empty;
        public string PacketText
        {
            get => _packetText;
            set
            {
                _packetText = value;
                RaiseOnPropertyChanged();
            }
        }

        private int _interval = 250;
        public int Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _toServer = true;
        public bool ToServer
        {
            get => _toServer;
            set
            {
                _toServer = value;
                RaiseOnPropertyChanged();
            }
        }

        private string _hotkeyText = string.Empty;
        public string HotkeyText
        {
            get => _hotkeyText;
            set
            {
                _hotkeyText = value;
                RaiseOnPropertyChanged();
            }
        }

        private int _cycles = 1;
        public int Cycles
        {
            get => _cycles;
            set
            {
                _cycles = value;
                RaiseOnPropertyChanged();
            }
        }

        public SchedulerPage()
        {
            InitializeComponent();

            Bind(PacketTxt, "Text", nameof(PacketText));
            Bind(IntervalTxt, "Text", nameof(Interval));
            Bind(ToServerChbx, "Checked", nameof(ToServer));
            Bind(HotkeyTxt, "Text", nameof(HotkeyText));
            Bind(CyclesTxt, "Text", nameof(Cycles));

            if (Master != null)
            {
                Master.Hook.HotkeyPressed += Hook_HotkeyPressed;
            }
        }

        private void HotkeyTxt_KeyDown(object sender, KeyEventArgs e)
        {
            _hotkeyText = string.Empty;
            if (e.Modifiers != Keys.None)
            {
                _hotkeyText = e.Modifiers + "+";
            }

            HotkeyText = _hotkeyText += e.KeyCode;

            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        private void Hook_HotkeyPressed(object sender, KeyEventArgs e)
        { }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            SchedulesVw.ClearItems();
        }
        private void CreateBtn_Click(object sender, EventArgs e)
        { }
        private void RemoveBtn_Click(object sender, EventArgs e)
        { }

        #region IHaltable Implementation
        public void Halt()
        { }
        public void Restore(ConnectedEventArgs e)
        { }
        #endregion
    }
}