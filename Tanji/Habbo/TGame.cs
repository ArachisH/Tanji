using System;

using Sulakore.Habbo;

namespace Tanji.Habbo
{
    public abstract class TGame : IGame, IDisposable
    {
        private bool _isDisposed;

        public virtual string Path { get; init; }
        public virtual bool IsUnity { get; init; }

        public virtual Incoming In { get; protected set; }
        public virtual Outgoing Out { get; protected set; }

        public virtual string Revision { get; protected set; }
        public virtual bool IsPostShuffle { get; protected set; }
        public virtual bool HasPingInstructions { get; protected set; }

        public abstract short Resolve(string name, bool isOutgoing);
        public abstract MessageInfo GetInformation(HMessage message);

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Dispose(true);
                GC.Collect();
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }
        protected abstract void Dispose(bool disposing);
    }
}