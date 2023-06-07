using System.Configuration;

using Tanji.Core.Services;

namespace Tanji.Services;

public sealed class ConfigurationDataProviderService : IConfigurationDataProviderService
{
    public string? GetValue(string? name) => ConfigurationManager.AppSettings.Get(name);
}