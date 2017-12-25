using System;
using System.Linq;
using System.ComponentModel;

using Tanji.Controls;

using Sulakore.Network;
using Sulakore.Crypto;

namespace Tanji.Services
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class ConnectionPage : ObservablePage, IReceiver
    {
        #region Status Constants
        private const string STANDING_BY = "Standing By...";

        private const string INTERCEPTING_CLIENT = "Intercepting Client...";
        private const string INTERCEPTING_CONNECTION = "Intercepting Connection...";
        private const string INTERCEPTING_CLIENT_PAGE_DATA = "Intercepting Client Page...";

        private const string MODIFYING_CLIENT = "Modifying Client...";
        private const string INJECTING_CLIENT = "Injecting Client...";
        private const string GENERATING_MESSAGE_HASHES = "Generating Message Hashes...";

        private const string ASSEMBLING_CLIENT = "Assembling Client...";
        private const string DISASSEMBLING_CLIENT = "Disassembling Client...";

        private const string SYNCHRONIZING_GAME = "Synchronizing Game...";
        private const string SYNCHRONIZING_GAME_DATA = "Synchronizing Game Data...";
        #endregion

        private ushort _proxyPort = 8282;
        [DefaultValue(typeof(ushort), "8282")]
        public ushort ProxyPort
        {
            get => _proxyPort;
            set
            {
                _proxyPort = value;
                RaiseOnPropertyChanged();
            }
        }

        private string _customClientPath = null;
        [DefaultValue(null)]
        public string CustomClientPath
        {
            get => _customClientPath;
            set
            {
                _customClientPath = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isAutoServerExtraction = true;
        [DefaultValue(true)]
        public bool IsAutoServerExtraction
        {
            get => _isAutoServerExtraction;
            set
            {
                _isAutoServerExtraction = value;
                RaiseOnPropertyChanged();
            }
        }

        //private HotelEndPoint _hotelServer = null;
        //[Browsable(false)]
        //[DefaultValue(null)]
        //public HotelEndPoint HotelServer
        //{
        //    get => _hotelServer;
        //    set
        //    {
        //        _hotelServer = value;
        //        RaiseOnPropertyChanged();
        //    }
        //}

        private string _status = STANDING_BY;
        [Browsable(false)]
        [DefaultValue(STANDING_BY)]
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                RaiseOnPropertyChanged();
            }
        }

        public ConnectionPage()
        {
            InitializeComponent();
        }

        #region IReceiver Implementation
        [Browsable(false)]
        public bool IsReceiving { get; set; }
        public void HandleOutgoing(DataInterceptedEventArgs e)
        {
            if (e.Packet.Id == 4001)
            {
                string sharedKeyHex = e.Packet.ReadUTF8();
                if (sharedKeyHex.Length % 2 != 0)
                {
                    sharedKeyHex = ("0" + sharedKeyHex);
                }

                byte[] sharedKey = Enumerable.Range(0, sharedKeyHex.Length / 2)
                    .Select(x => Convert.ToByte(sharedKeyHex.Substring(x * 2, 2), 16))
                    .ToArray();

                Master.Connection.Remote.Encrypter = new RC4(sharedKey);
                Master.Connection.Remote.IsEncrypting = true;

                e.IsBlocked = true;
                IsReceiving = false;
            }
            else if (e.Step >= 10)
            {
                IsReceiving = false;
            }
        }
        public void HandleIncoming(DataInterceptedEventArgs e)
        { }
        #endregion
    }
}