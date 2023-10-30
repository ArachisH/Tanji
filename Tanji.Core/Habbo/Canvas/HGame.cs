using System.Net;

using Tanji.Core.Habbo.Network.Formats;

namespace Tanji.Core.Habbo.Canvas;

public abstract class HGame : IGame, IDisposable
{
    private readonly HPatches _defaultPatches;

    protected bool IsDisposed { get; private set; }

    public abstract IHFormat SendPacketFormat { get; }
    public abstract IHFormat ReceivePacketFormat { get; }

    public abstract HPlatform Platform { get; }
    public abstract int MinimumConnectionAttempts { get; }

    public virtual string? Path { get; init; }
    public virtual string? Revision { get; protected set; }
    public virtual bool IsPostShuffle { get; protected set; }

    public int KeyShouterId { get; init; }
    public int RemoteEndPointShouterId => IsPostShuffle ? 4000 : 206;
    public IPEndPoint? RemoteEndPoint { get; init; }

    public HGame(HPatches defaultPatches) => _defaultPatches = defaultPatches;

    public HPatches Patch(HPatches patches)
    {
        HPatches failedPatches = HPatches.None;
        foreach (HPatches patch in Enum.GetValues(typeof(HPatches)))
        {
            if ((patches & patch) != patch || patch == HPatches.None) continue;

            bool? result = TryPatch(patch);
            if (result == null)
            {
                ThrowHelper.ThrowNotSupportedException($"Patch not supported: {patch}");
            }

            // It's a nullable, so that's why I'm not using the '!' operator...
            if (result == false)
            {
                failedPatches |= patch; // Did this patch fail?; If so, add the flag to 'failedPatches'
            }
        }
        return failedPatches;
    }
    public HPatches Patch() => Patch(_defaultPatches);
    protected virtual bool? TryPatch(HPatches patch) => false;

    public virtual void GenerateMessageHashes() => ThrowHelper.ThrowNotSupportedException();
    public virtual bool TryResolveMessage(string name, uint hash, bool isOutgoing, out HMessage message) => throw new NotSupportedException();

    public virtual byte[] ToArray() => throw new NotSupportedException();
    public virtual void Disassemble() => ThrowHelper.ThrowNotSupportedException();
    public virtual void Assemble(string path) => ThrowHelper.ThrowNotSupportedException();

    public void Dispose()
    {
        if (!IsDisposed)
        {
            Dispose(true);
            IsDisposed = true;

            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
    protected abstract void Dispose(bool disposing);
}