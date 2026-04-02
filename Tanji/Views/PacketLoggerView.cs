using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using CommunityToolkit.Mvvm.ComponentModel;

using Tanji.Properties;
using Tanji.Core.Infrastructure.Models;
using Tanji.Core.Infrastructure.Services;
using Tanji.Core.Infrastructure.ViewModels;

namespace Tanji.Views;

public partial class PacketLoggerView : Form
{
    private readonly ILogger<PacketLoggerView> _logger;
    private IPacketLogHandlerService _packetLogHandler;
    private readonly PacketLoggerViewModel _packetLoggerViewModel;

    public PacketLoggerView(ILogger<PacketLoggerView> logger,
        PacketLoggerViewModel packetLoggerViewModel,
        IPacketLogHandlerService packetLogHandler)
    {
        _logger = logger;
        _packetLogHandler = packetLogHandler;
        _packetLoggerViewModel = packetLoggerViewModel;

        InitializeComponent();
        Icon = Resources.Tanji_256;

        _ = RenderPacketLogsAsync();
    }

    private async Task RenderPacketLogsAsync()
    {
        while (await _packetLogHandler.WaitForPacketLogsAsync())
        {
            PacketLog pLog = await _packetLogHandler.ReadPacketLogAsync();

            // Suspend Text Drawing
            loggerVw.SuspendPaint();

            if (pLog.Repetitions < 2)
            {
                int start = loggerVw.TextLength;
                loggerVw.AppendText(pLog.ToString());
                foreach ((int length, Color highlight) in pLog.GetHighlights())
                {
                    HighlightText(start, length, highlight);
                    start += length;
                }
            }
            else
            {
                int previousLineIndex = loggerVw.Lines.Length - 2;
                string previousLine = loggerVw.Lines[previousLineIndex];
                int previousLineFirstCharIndex = loggerVw.Find(previousLine, RichTextBoxFinds.Reverse | RichTextBoxFinds.NoHighlight);

                HighlightText(previousLineFirstCharIndex, previousLine.Length, _packetLoggerViewModel.PacketLoggingOptions.RepeatedHighlight);
                loggerVw.SelectedText = $"--------------- x{pLog.Repetitions}";
            }

            // Resume Text Drawing
            loggerVw.ResumePaint();

            if (_packetLoggerViewModel.PacketLoggingOptions.IsAutoScrolling)
            {
                loggerVw.ScrollToBottom();
            }
        }
    }

    private void HighlightText(int start, int length, Color highlight)
    {
        loggerVw.SelectionStart = start;
        loggerVw.SelectionLength = length;
        loggerVw.SelectionColor = highlight;
    }

    protected override void OnLoad(EventArgs e)
    {
        if (Program.Services is not null)
        {
            DataContext = Program.Services.GetRequiredService<PacketLoggerViewModel>();
        }
        base.OnLoad(e);
    }
    protected override void OnDataContextChanged(EventArgs e)
    {
        if (DataContext is ObservableObject observable)
        {
            viewModelSrc.DataSource = observable;
        }
        base.OnDataContextChanged(e);
    }
}