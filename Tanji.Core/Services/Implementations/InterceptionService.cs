namespace Tanji.Core.Services;

public sealed class InterceptionService : IInterceptionService
{
    private readonly IConfigurationService _configuration;

    public InterceptionService(IConfigurationService configuration)
    {
        _configuration = configuration;
    }
}