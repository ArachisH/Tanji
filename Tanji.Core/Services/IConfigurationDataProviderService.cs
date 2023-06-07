namespace Tanji.Core.Services;

public interface IConfigurationDataProviderService
{
    string? GetValue(string? name);
}