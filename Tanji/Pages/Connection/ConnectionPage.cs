﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

using Tanji.Windows;
using Tanji.Manipulators;

using Sulakore.Habbo;
using Sulakore.Crypto;
using Sulakore.Communication;

using Flazzy;

using Eavesdrop;

namespace Tanji.Pages.Connection;

public class ConnectionPage : TanjiPage, IReceiver
{
    private Uri _swfUri;
    private Guid _randomQuery;
    private Dictionary<string, string> _variableReplacements;

    #region Status Constants
    private const string STANDING_BY = "Standing By...";
    private const string REPLACING_RESOURCES = "Replacing Resources...";

    private const string INTERCEPTING_CLIENT = "Intercepting Client...";
    private const string INTERCEPTING_CONNECTION = "Intercepting Connection...";
    private const string INTERCEPTING_CLIENT_PAGE = "Intercepting Client Page...";

    private const string MODIFYING_CLIENT = "Modifying Client...";
    private const string INJECTING_CLIENT = "Injecting Client...";
    private const string GENERATING_MESSAGE_HASHES = "Generating Message Hashes...";

    private const string ASSEMBLING_CLIENT = "Assembling Client...";
    private const string DISASSEMBLING_CLIENT = "Disassembling Client...";
    #endregion

    private string _customClientPath = null;
    public string CustomClientPath
    {
        get => _customClientPath;
        set
        {
            _customClientPath = value;
            RaiseOnPropertyChanged();
        }
    }

    private HotelEndPoint _hotelServer = null;
    public HotelEndPoint HotelServer
    {
        get => _hotelServer;
        set
        {
            _hotelServer = value;
            RaiseOnPropertyChanged();
        }
    }

    private string _status = STANDING_BY;
    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            RaiseOnPropertyChanged();
        }
    }

    public ConnectionPage(MainFrm ui, TabPage tab)
        : base(ui, tab)
    {
        //UI.CoTProxyPortLbl.Text = $"Proxy Port: {Program.Configuration.ProxyListenPort}";
        PropertyChanged += ConnectionPage_PropertyChanged;

        UI.CoTStatusTxt.DataBindings.Add("Text", this, nameof(Status), false, DataSourceUpdateMode.OnPropertyChanged);
        UI.CoTCustomClientTxt.DataBindings.Add("Text", this, nameof(CustomClientPath), false, DataSourceUpdateMode.OnPropertyChanged);

        UI.CoTBrowseBtn.Click += CoTBrowseBtn_Click;
        UI.CoTConnectBtn.Click += CoTConnectBtn_Click;

        UI.CoTDestroyCertificatesBtn.Click += CoTDestroyCertificatesBtn_Click;
        UI.CoTExportCertificateAuthorityBtn.Click += CoTExportCertificateAuthorityBtn_Click;

        UI.CoTResetBtn.Click += CoTResetBtn_Click;
        UI.CoTUpdateBtn.Click += CoTUpdateVariableBtn_Click;

        UI.CoTVariablesVw.ItemSelected += CoTVariablesVw_ItemSelected;
        UI.CoTVariablesVw.ItemSelectionStateChanged += CoTVariablesVw_ItemSelectionStateChanged;
    }

    private void ConnectionPage_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Status):
            {
                bool isBusy = (Status != STANDING_BY);
                UI.CoTConnectBtn.Text = (isBusy ? "Cancel" : "Connect");

                UI.CoTBrowseBtn.Enabled = !isBusy;
                UI.CoTCustomClientTxt.IsReadOnly = isBusy;
                UI.CoTDestroyCertificatesBtn.Enabled = !isBusy;
                break;
            }
        }
    }

    private void CoTBrowseBtn_Click(object sender, EventArgs e)
    {
        UI.CustomClientDlg.FileName = string.Empty;
        if (UI.CustomClientDlg.ShowDialog() != DialogResult.OK) return;
        CustomClientPath = UI.CustomClientDlg.FileName;
    }
    private void CoTConnectBtn_Click(object sender, EventArgs e)
    {
        if (Status != STANDING_BY)
        {
            TerminateProxy();
            DisableReplacements();
            UI.Connection.Disconnect();

            if (UI.Game != null)
            {
                UI.Game.Dispose();
                UI.Game = null;
            }
            Status = STANDING_BY;
            return;
        }

        if (UI.Connection.IsConnected)
        {
            if (MessageBox.Show("Are you sure you want to disconnect from the current session?", "Tanji - Alert!",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                UI.Connection.Disconnect();
            }
            else return;
        }

        _variableReplacements = UI.CoTVariablesVw.CheckedItems
            .Cast<ListViewItem>()
            .Where(i => !string.IsNullOrWhiteSpace(i.SubItems[1].Text))
            .ToDictionary(i => i.Text, i => i.SubItems[1].Text);

        if (Eavesdropper.Certifier.CreateTrustedRootCertificate())
        {
            Eavesdropper.ResponseInterceptedAsync += InterceptClientPageAsync;
            //Eavesdropper.Initiate(Program.Configuration.ProxyListenPort);
            Status = INTERCEPTING_CLIENT_PAGE;
        }
    }

    private void CoTResetBtn_Click(object sender, EventArgs e)
    {
        ListViewItem item =
            UI.CoTVariablesVw.SelectedItem;

        item.SubItems[1].Text = string.Empty;
        UI.CoTResetBtn.Enabled = false;
        UI.CoTValueTxt.Text = string.Empty;
        item.Checked = false;
    }
    private void CoTUpdateVariableBtn_Click(object sender, EventArgs e)
    {
        UI.CoTVariablesVw.SelectedItem.SubItems[1].Text = UI.CoTValueTxt.Text;
        if (!string.IsNullOrWhiteSpace(UI.CoTValueTxt.Text))
        {
            UI.CoTVariablesVw.SelectedItem.Checked = true;
        }
    }

    private void CoTDestroyCertificatesBtn_Click(object sender, EventArgs e)
    {
        Eavesdropper.Certifier.DestroyCertificates();
    }
    private void CoTExportCertificateAuthorityBtn_Click(object sender, EventArgs e)
    {
        string fileName = Eavesdropper.Certifier.CertificateAuthorityName.Replace(" ", "_") + ".cer";
        string filePath = Path.GetFullPath(fileName);

        bool wasExported = Eavesdropper.Certifier.CreateTrustedRootCertificate() && Eavesdropper.Certifier.ExportTrustedRootCertificate(filePath);
        string message = wasExported ? $"Successfully exported '{fileName}' to:\r\n\r\n{filePath}"
            : "Unable to export root CA, perhaps try running the application with administrative privileges.";

        MessageBox.Show(message, "Tanji ~ Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void CoTVariablesVw_ItemSelected(object sender, EventArgs e)
    {
        ListViewItem item = UI.CoTVariablesVw.SelectedItem;

        ToggleClearVariableButton(item);
        UI.CoTUpdateBtn.Enabled = true;

        UI.CoTVariableTxt.Text = item.Text;
        UI.CoTValueTxt.Text = item.SubItems[1].Text;
    }
    private void CoTVariablesVw_ItemSelectionStateChanged(object sender, EventArgs e)
    {
        if (!UI.CoTVariablesVw.HasSelectedItem)
        {
            UI.CoTUpdateBtn.Enabled = (UI.CoTResetBtn.Enabled = false);
            UI.CoTVariableTxt.Text = (UI.CoTValueTxt.Text = string.Empty);
        }
    }

    private Task InjectGameClientAsync(object sender, RequestInterceptedEventArgs e)
    {
        string clientName = Path.GetFileName(e.Uri.LocalPath);
        string clientPath = Path.GetFullPath($"Modified Clients/{e.Uri.Host}{e.Uri.LocalPath}");

        if (!e.Uri.Query.EndsWith("?" + _randomQuery) && !File.Exists(clientPath)) return null;
        Eavesdropper.RequestInterceptedAsync -= InjectGameClientAsync;

        if (!string.IsNullOrWhiteSpace(CustomClientPath))
        {
            clientPath = Path.GetFullPath(CustomClientPath);
        }
        if (!File.Exists(clientPath))
        {
            Status = INTERCEPTING_CLIENT;
            Eavesdropper.ResponseInterceptedAsync += InterceptGameClientAsync;
        }
        else
        {
            Status = DISASSEMBLING_CLIENT;
            UI.Game = new HGame(clientPath);
            UI.Game.Disassemble();

            Status = GENERATING_MESSAGE_HASHES;
            UI.Game.GenerateMessageHashes();

            UI.In.Load(UI.Game, "Hashes.ini");
            UI.Out.Load(UI.Game, "Hashes.ini");
            UI.ModulesPg.ModifyGame(UI.Game);

            if (_variableReplacements.Count > 0)
            {
                Eavesdropper.ResponseInterceptedAsync += ReplaceResourcesAsync;
            }
            else TerminateProxy();

            Task interceptConnectionTask = InterceptConnectionAsync();

            e.Response = new HttpResponseMessage(HttpStatusCode.OK);
            e.Response.Content = new StreamContent(File.OpenRead(clientPath));
        }
        return null;
    }
    private async Task ReplaceResourcesAsync(object sender, ResponseInterceptedEventArgs e)
    {
        string absoluteUri = e.Uri.AbsoluteUri;
        if (_variableReplacements.TryGetValue(absoluteUri, out string replacementUrl))
        {
            if (e.StatusCode == HttpStatusCode.TemporaryRedirect)
            {
                _variableReplacements.Remove(absoluteUri);

                absoluteUri = e.Headers.Location.AbsoluteUri;
                _variableReplacements[absoluteUri] = replacementUrl;
                return;
            }

            if (replacementUrl.StartsWith("http"))
            {
                using var webClient = new WebClient();
                e.Content = new ByteArrayContent(await webClient.DownloadDataTaskAsync(replacementUrl).ConfigureAwait(false));
            }
            else e.Content = new ByteArrayContent(File.ReadAllBytes(replacementUrl));

            _variableReplacements.Remove(absoluteUri);
            if (_variableReplacements.Count == 0)
            {
                TerminateProxy();
                Status = STANDING_BY;
            }
        }
    }
    private async Task InterceptGameClientAsync(object sender, ResponseInterceptedEventArgs e)
    {
        if (e.Request.RequestUri != _swfUri) return;
        if (e.Content.Headers.ContentType.MediaType != "application/x-shockwave-flash") return;
        Eavesdropper.ResponseInterceptedAsync -= InterceptGameClientAsync;

        string clientPath = Path.GetFullPath($"Modified Clients/{e.Uri.Host}{e.Uri.LocalPath}");
        string clientDirectory = Path.GetDirectoryName(clientPath);
        Directory.CreateDirectory(clientDirectory);

        Status = DISASSEMBLING_CLIENT;
        UI.Game = new HGame(await e.Content.ReadAsByteArrayAsync().ConfigureAwait(false));
        UI.Game.Location = clientPath;
        UI.Game.Disassemble();

        Status = GENERATING_MESSAGE_HASHES;
        UI.Game.GenerateMessageHashes();

        Status = MODIFYING_CLIENT;
        UI.Game.DisableHostChecks();
        UI.Game.InjectKeyShouter(4001);
        UI.Game.InjectEndPointShouter(4000);
        UI.Game.InjectEndPoint("127.0.0.1", UI.Connection.ListenPort);

        UI.In.Load(UI.Game, "Hashes.ini");
        UI.Out.Load(UI.Game, "Hashes.ini");
        UI.ModulesPg.ModifyGame(UI.Game);

        CompressionKind compression = CompressionKind.ZLib;
#if DEBUG
        compression = CompressionKind.None;
#endif
        Status = ASSEMBLING_CLIENT;
        byte[] payload = UI.Game.ToArray(compression);
        using (var clientStream = File.Open(clientPath, FileMode.Create, FileAccess.Write))
        {
            clientStream.Write(payload, 0, payload.Length);
        }

        if (_variableReplacements.Count > 0)
        {
            Eavesdropper.ResponseInterceptedAsync += ReplaceResourcesAsync;
        }
        else TerminateProxy();

        e.Content = new ByteArrayContent(payload);
        Task interceptConnectionTask = InterceptConnectionAsync();
    }
    private async Task InterceptClientPageAsync(object sender, ResponseInterceptedEventArgs e)
    {
        if (e.Content == null) return;

        string contentType = e.Content.Headers.ContentType.MediaType;
        if (!contentType.Contains("text") && !contentType.Contains("javascript")) return;

        int triggerSumIndices = 0;
        string body = await e.Content.ReadAsStringAsync().ConfigureAwait(false);
        //foreach (string trigger in Program.Configuration.FlashInterceptionTriggers)
        //{
        //    triggerSumIndices += body.IndexOf(trigger, StringComparison.OrdinalIgnoreCase);
        //}
        if (triggerSumIndices < 0) return;


        int swfStartIndex = GetSWFStartIndex(body);
        if (swfStartIndex == -1) return;

        Eavesdropper.ResponseInterceptedAsync -= InterceptClientPageAsync;

        UI.GameData.Source = body;
        UI.ModulesPg.ModifyGameData(UI.GameData);
        body = UI.GameData.Source;

        string[] resourceKeys = _variableReplacements.Keys.ToArray();
        foreach (string variable in resourceKeys)
        {
            string fakeValue = _variableReplacements[variable];
            string realValue = UI.GameData[variable].Replace("\\/", "/");

            _variableReplacements.Remove(variable);
            _variableReplacements[realValue] = fakeValue;
        }

        if (swfStartIndex != -1)
        {
            do
            {
                if (body[swfStartIndex++] == ')') continue;
                var embedSWFEnd = body.IndexOf(',', swfStartIndex);

                if (embedSWFEnd == -1) break;
                body = body.Insert(embedSWFEnd, $"+\"?{(_randomQuery = Guid.NewGuid())}\"");
            }
            while ((swfStartIndex = GetSWFStartIndex(body, swfStartIndex)) != -1);
            byte[] payload = Encoding.UTF8.GetBytes(body);

            e.Content = new ByteArrayContent(payload);
        }
        Status = INJECTING_CLIENT;
        Eavesdropper.RequestInterceptedAsync += InjectGameClientAsync;
    }

    private void TerminateProxy()
    {
        Eavesdropper.Terminate();
        Eavesdropper.RequestInterceptedAsync -= InjectGameClientAsync;
        Eavesdropper.ResponseInterceptedAsync -= InterceptGameClientAsync;
        Eavesdropper.ResponseInterceptedAsync -= InterceptClientPageAsync;
        Eavesdropper.ResponseInterceptedAsync -= ReplaceResourcesAsync;
    }
    private async Task InterceptConnectionAsync()
    {
        Status = INTERCEPTING_CONNECTION;
        UI.Connection.SocketSkip = UI.Game.HasPingInstructions ? 2 : 0;

        await UI.Connection.InterceptAsync(HotelServer).ConfigureAwait(false);

        UI.Game.Dispose();
        foreach (MessageItem message in UI.Game.OutMessages.Values.Concat(UI.Game.InMessages.Values))
        {
            message.Class = null;
            message.Parser = null;
            message.References.Clear();
        }

        if (UI.Connection.IsConnected)
        {
            if (_variableReplacements.Count > 0)
            {
                Status = REPLACING_RESOURCES;
            }
            else TerminateProxy();
        }
        else
        {
            TerminateProxy();
            DisableReplacements();
            UI.Connection.Disconnect();

            UI.Game = null;
        }

        Status = STANDING_BY;

        GC.Collect();
        await Task.Delay(1000).ContinueWith(t => GC.Collect()).ConfigureAwait(false);
    }

    private void DisableReplacements()
    {
        foreach (ListViewItem item in UI.CoTVariablesVw.Items)
        {
            item.Checked = false;
        }
    }
    private int GetSWFStartIndex(string body, int index = 0)
    {
        int swfStartIndex = (body.IndexOf("embedswf(", index, StringComparison.OrdinalIgnoreCase) + 9);
        if (swfStartIndex == 8)
        {
            swfStartIndex = (body.IndexOf("swfobject(", index, StringComparison.OrdinalIgnoreCase) + 10);
            if (swfStartIndex == 9) return -1;
        }
        return swfStartIndex;
    }
    private void ToggleClearVariableButton(ListViewItem item)
    {
        UI.CoTResetBtn.Enabled = (!string.IsNullOrWhiteSpace(item.SubItems[1].Text));
    }

    protected override void OnTabSelecting(TabControlCancelEventArgs e)
    {
        if (!UI.Connection.IsConnected)
        {
            UI.TopMost = true;
        }
        base.OnTabSelecting(e);
    }
    protected override void OnTabDeselecting(TabControlCancelEventArgs e)
    {
        UI.TopMost = UI.PacketLoggerUI.TopMost;
        base.OnTabDeselecting(e);
    }

    #region IReceiver Implementation
    public bool IsReceiving { get; set; }
    public bool IsIncomingEncrypted { get; private set; }
    public void HandleOutgoing(DataInterceptedEventArgs e)
    {
        if (e.Packet.Header == 4001)
        {
            string sharedKeyHex = e.Packet.ReadString();
            if (sharedKeyHex.Length % 2 != 0)
            {
                sharedKeyHex = ("0" + sharedKeyHex);
            }

            byte[] sharedKey = Enumerable.Range(0, sharedKeyHex.Length / 2)
                .Select(x => Convert.ToByte(sharedKeyHex.Substring(x * 2, 2), 16))
                .ToArray();

            UI.Connection.Remote.Encrypter = new RC4(sharedKey);
            UI.Connection.Remote.IsEncrypting = true;

            if (IsIncomingEncrypted)
            {
                UI.Connection.Remote.Decrypter = new RC4(sharedKey);
                UI.Connection.Remote.IsDecrypting = true;
            }

            e.IsBlocked = true;
            IsReceiving = false;
        }
        else if (e.Step >= 10)
        {
            IsReceiving = false;
        }
    }
    public void HandleIncoming(DataInterceptedEventArgs e)
    {
        if (e.Step == 2)
        {
            e.Packet.ReadString();
            if (e.Packet.Readable > 0)
            {
                IsIncomingEncrypted = e.Packet.ReadBoolean();
                e.Packet.ReplaceBoolean(false, e.Packet.Position - 1);
            }
        }
    }
    #endregion
}