using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Communication
{
    public class DataInterceptedEventArgs : EventArgs
    {
        private readonly byte[] _ogData;
        private readonly string _ogString;
        private readonly object _continueLock;
        private readonly HDestination _ogDestination;
        private readonly DataInterceptedEventArgs _args;
        private readonly Func<DataInterceptedEventArgs, Task<int>> _relayer;
        private readonly Func<DataInterceptedEventArgs, Task> _continuation;

        public int Step { get; }
        public bool IsOutgoing { get; }
        public DateTime Timestamp { get; }
        public List<HMessage> Executions { get; }

        public bool IsOriginal => Packet.ToString().Equals(_ogString);
        public bool IsContinuable => (_continuation != null && !HasContinued);

        private bool _isBlocked;
        public bool IsBlocked
        {
            get => (_args?.IsBlocked ?? _isBlocked);
            set
            {
                if (_args != null)
                {
                    _args.IsBlocked = value;
                }
                _isBlocked = value;
            }
        }

        private HMessage _packet;
        public HMessage Packet
        {
            get => (_args?.Packet ?? _packet);
            set
            {
                if (_args != null)
                {
                    _args.Packet = value;
                }
                _packet = value;
            }
        }

        private bool _wasRelayed;
        public bool WasRelayed
        {
            get => (_args?.WasRelayed ?? _wasRelayed);
            private set
            {
                if (_args != null)
                {
                    _args.WasRelayed = value;
                }
                _wasRelayed = value;
            }
        }

        private bool _hasContinued;
        public bool HasContinued
        {
            get => (_args?.HasContinued ?? _hasContinued);
            private set
            {
                if (_args != null)
                {
                    _args.HasContinued = value;
                }
                _hasContinued = value;
            }
        }

        public DataInterceptedEventArgs(DataInterceptedEventArgs args)
        {
            _args = args;
            _ogData = args._ogData;
            _ogString = args._ogString;
            _relayer = args._relayer;
            _continuation = args._continuation;
            _continueLock = args._continueLock;

            Step = args.Step;
            Timestamp = args.Timestamp;
            IsOutgoing = args.IsOutgoing;
        }
        public DataInterceptedEventArgs(HMessage packet, int step, bool isOutgoing)
        {
            _ogData = packet.ToBytes();
            _ogString = packet.ToString();
            _ogDestination = packet.Destination;

            Step = step;
            Packet = packet;
            IsOutgoing = isOutgoing;
            Timestamp = DateTime.Now;
            Executions = new List<HMessage>();
        }
        public DataInterceptedEventArgs(HMessage packet, int step, bool isOutgoing, Func<DataInterceptedEventArgs, Task> continuation)
            : this(packet, step, isOutgoing)
        {
            _continueLock = new object();
            _continuation = continuation;
        }
        public DataInterceptedEventArgs(HMessage packet, int step, bool isOutgoing, Func<DataInterceptedEventArgs, Task> continuation, Func<DataInterceptedEventArgs, Task<int>> relayer)
            : this(packet, step, isOutgoing, continuation)
        {
            _relayer = relayer;
        }

        public void Continue()
        {
            Continue(false);
        }
        public void Continue(bool relay)
        {
            if (IsContinuable)
            {
                lock (_continueLock)
                {
                    if (relay && !IsBlocked)
                    {
                        WasRelayed = true;
                        _relayer?.Invoke(this);
                    }

                    HasContinued = true;
                    _continuation(this);
                }
            }
        }

        public void Restore()
        {
            if (!IsOriginal)
            {
                Packet = new HMessage(_ogData, _ogDestination);
            }
        }
        public byte[] GetOriginalData()
        {
            return _ogData;
        }
    }
}