using System.Drawing;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Tanji.Core.Infrastructure.Configuration;

public sealed partial class PacketLoggingOptions : ObservableObject
{
    [ObservableProperty]
    private Color _detailHighlight = Color.Cyan;

    [ObservableProperty]
    private Color _defaultHighlight = Color.FromArgb(160, 160, 160);

    [ObservableProperty]
    private Color _blockedHighlight = Color.Yellow;

    [ObservableProperty]
    private Color _replacedHighlight = Color.Yellow;

    [ObservableProperty]
    private Color _incomingHighlight = Color.FromArgb(178, 34, 34);

    [ObservableProperty]
    private Color _outgoingHighlight = Color.FromArgb(0, 102, 204);

    [ObservableProperty]
    private Color _repeatedHighlight = Color.FromArgb(255, 174, 61);

    [ObservableProperty]
    private Color _structureHighlight = Color.FromArgb(170, 244, 66);

    [ObservableProperty]
    private Color _dismantledHighlight = Color.FromArgb(0, 204, 136);

    [ObservableProperty]
    private bool _isAutoScrolling = true;

    [ObservableProperty]
    private bool _isLoggingIncoming = true;

    [ObservableProperty]
    private bool _isLoggingOutgoing = true;

    [ObservableProperty]
    private bool _isLoggingBlocked = true;

    [ObservableProperty]
    private bool _isLogingReplaced = true;

    [ObservableProperty]
    private bool _isLoggingStructure = true;

    [ObservableProperty]
    private bool _isLoggingDismantled = true;

    [ObservableProperty]
    private bool _isLoggingMessageHash = true;

    [ObservableProperty]
    private bool _isLoggingMessageName = true;

    [ObservableProperty]
    private bool _isCompactingRepetitions = true;
}