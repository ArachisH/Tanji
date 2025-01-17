using Tanji.Core.Net;
using Tanji.Core.Net.Interception;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Tanji.Core.Infrastructure.ViewModels;

public partial class InjectionViewModel : ObservableObject, IMiddleman
{
    public bool IsHandlingInbound { get; }
    public bool IsHandlingOutbound { get; }

    public ValueTask<bool> PacketInboundAsync(Memory<byte> buffer, HNode source, HNode destination)
    {
        return new ValueTask<bool>(false);
    }
    public ValueTask<bool> PacketOutboundAsync(Memory<byte> buffer, HNode source, HNode destination)
    {
        return new ValueTask<bool>(false);
    }
}