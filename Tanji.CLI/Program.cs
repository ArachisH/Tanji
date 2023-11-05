using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using Tanji.Core;
using Tanji.Core.Services;
using Tanji.Core.Configuration;

namespace Tanji.CLI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.Configure<TanjiOptions>(builder.Configuration);
        builder.Services.AddSingleton<Program>();
        builder.Services.AddTanjiCore();

        Console.Title = $"Tanji(Core) - Press any key to exit...";
        IHost host = builder.Build();

        var app = host.Services.GetRequiredService<Program>();
        await app.RunAsync();
    }

    private readonly ILogger<Program> _logger;
    private readonly IInterceptionService _interception;

    public Program(ILogger<Program> logger, IInterceptionService interception)
    {
        _logger = logger;
        _interception = interception;

        _logger.LogDebug($"{nameof(Program)} ctor");
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Intercepting Game Token(s)...");
        do
        {
            string gameToken = await _interception.InterceptGameTicketAsync();
            _logger.LogInformation("Game Token: {Token}", gameToken);

            // TODO: Launch local client
        }
        while (_interception.IsInterceptingWebTraffic);
    }
}