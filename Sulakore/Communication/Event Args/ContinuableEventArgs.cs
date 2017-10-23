using System;
using System.Threading.Tasks;

namespace Sulakore.Communication
{
    public abstract class ContinuableEventArgs : EventArgs
    {
        internal readonly Func<Task> _continuation;

        public int Continuations { get; internal set; }
        public bool HasContinued => (Continuations >= 1);
        public bool IsContinuable => (_continuation != null);

        public ContinuableEventArgs(Func<Task> continuation)
        {
            _continuation = continuation;
        }

        public Task Continue()
        {
            if (_continuation == null)
                return null;

            Continuations++;
            return _continuation();
        }
    }
}