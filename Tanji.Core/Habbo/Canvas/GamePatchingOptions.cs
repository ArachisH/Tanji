using System.Net;
using System.Diagnostics.CodeAnalysis;

namespace Tanji.Core.Habbo.Canvas;

public readonly record struct GamePatchingOptions
{
    public int KeyShoutingId { get; init; }
    public int AddressShoutingId { get; init; }

    public string? RSAModulus { get; init; }
    public string? RSAExponent { get; init; }

    public IPEndPoint? RemoteAddress { get; init; }
    public required HPatches Patches { get; init; }

    [SetsRequiredMembers]
    public GamePatchingOptions(HPatches patches)
    {
        Patches = patches;
    }
}