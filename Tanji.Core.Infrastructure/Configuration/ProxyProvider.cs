using System.Net;

namespace Tanji.Core.Infrastructure.Configuration;

public sealed record class ProxyProvider
{
    public required string? Address { get; init; }
    public required string? Username { get; init; }
    public required string? Password { get; init; }

    public IWebProxy? GetProxy()
    {
        if (string.IsNullOrWhiteSpace(Address)) return null;

        var proxy = new WebProxy(Address);
        if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
        {
            proxy.Credentials = new NetworkCredential(Username, Password);
        }
        return proxy;
    }
}