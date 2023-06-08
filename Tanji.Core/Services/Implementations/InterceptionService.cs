using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Tanji.Core.Configuration;

namespace Tanji.Core.Services;

public sealed class InterceptionService : IInterceptionService
{
    private readonly TanjiOptions _options;
    private readonly ILogger<InterceptionService> _logger;

    public InterceptionService(ILogger<InterceptionService> logger, IOptions<TanjiOptions> options)
    {
        _options = options.Value;
        _logger = logger;

        _logger.LogInformation("InterceptionService ctor");
    }
}