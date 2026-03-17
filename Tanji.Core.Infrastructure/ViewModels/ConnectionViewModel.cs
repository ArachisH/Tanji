using Tanji.Core.Net;
using Tanji.Core.Canvas;
using Tanji.Core.Net.Interception;
using Tanji.Core.Infrastructure.Services;

using Microsoft.Extensions.Logging;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Tanji.Core.Infrastructure.ViewModels;

public partial class ConnectionViewModel : ObservableObject
{
    private readonly ILogger<ConnectionViewModel> _logger;
    private readonly IWebInterceptionService _webInterception;
    private readonly IClientHandlerService _clientHandlerService;
    private readonly IConnectionHandlerService _connectionHandler;

    #region Status Constants
    private const string STANDING_BY = "Standing By...";
    private const string REPLACING_RESOURCES = "Replacing Resources...";

    private const string INTERCEPTING_CLIENT = "Intercepting Client...";
    private const string INTERCEPTING_CONNECTION = "Intercepting Connection...";
    private const string INTERCEPTING_CLIENT_PAGE = "Intercepting Client Page...";

    private const string PATCHING_CLIENT = "Patching Client...";
    private const string INJECTING_CLIENT = "Injecting Client...";
    private const string GENERATING_MESSAGE_HASHES = "Generating Message Hashes...";

    private const string ASSEMBLING_CLIENT = "Assembling Client...";
    private const string DISASSEMBLING_CLIENT = "Disassembling Client...";
    #endregion

    [ObservableProperty]
    private string _status = STANDING_BY;

    [ObservableProperty]
    private HotelEndPoint? _hotelServer;

    [ObservableProperty]
    private string? _customClientPath;

    public ConnectionViewModel(ILogger<ConnectionViewModel> logger,
        IClientHandlerService clientHandler,
        IWebInterceptionService webInterception,
        IConnectionHandlerService connectionHandler)
    {
        _logger = logger;
        _webInterception = webInterception;
        _clientHandlerService = clientHandler;
        _connectionHandler = connectionHandler;
    }

    [RelayCommand]
    private async Task ConnectAsync()
    {
        _webInterception.Start();

        Status = INTERCEPTING_CLIENT_PAGE;
        string ticket = await _webInterception.InterceptTicketAsync();

        // TODO: Do not stop unless there are external resources we are replacing/modifying.
        _webInterception.Stop();

        Status = PATCHING_CLIENT;
        IGame game = await _clientHandlerService.PatchClientAsync(HPlatform.Flash, CustomClientPath); // TODO: Radio button for selecting client type

        Status = INTERCEPTING_CONNECTION;
        // Begin listening for connection attempts from the client, before launching the client.
        var context = new HConnectionContext(game);
        Task<HConnection> interceptConnectionTask = _connectionHandler.InterceptConnectionAsync(ticket, context);
        _ = _clientHandlerService.LaunchClientAsync(context.Platform, ticket, context.ClientPath);

        await interceptConnectionTask;
        Status = STANDING_BY;
    }
}