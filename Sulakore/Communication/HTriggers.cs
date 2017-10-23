using System;
using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Communication
{
    public class HTriggers
    {
        private readonly Stack<HMessage> _outPreviousMessages, _inPreviousMessages;
        private readonly Dictionary<ushort, Action<DataInterceptedEventArgs>> _outCallbacks, _inCallbacks;

        public HTriggers()
        {
            _inPreviousMessages = new Stack<HMessage>();
            _outPreviousMessages = new Stack<HMessage>();

            _inCallbacks = new Dictionary<ushort, Action<DataInterceptedEventArgs>>();
            _outCallbacks = new Dictionary<ushort, Action<DataInterceptedEventArgs>>();
        }

        public void OutDetach()
        {
            _outCallbacks.Clear();
        }
        public void OutDetach(ushort header)
        {
            if (_outCallbacks.ContainsKey(header))
                _outCallbacks.Remove(header);
        }
        public void OutAttach(ushort header, Action<DataInterceptedEventArgs> callback)
        {
            _outCallbacks[header] = callback;
        }

        public void InDetach()
        {
            _inCallbacks.Clear();
        }
        public void InDetach(ushort header)
        {
            if (_inCallbacks.ContainsKey(header))
                _inCallbacks.Remove(header);
        }
        public void InAttach(ushort header, Action<DataInterceptedEventArgs> callback)
        {
            _inCallbacks[header] = callback;
        }

        private void HandleMessage(
            DataInterceptedEventArgs e,
            Stack<HMessage> previousMessages,
            Action<HMessage, HMessage> handler,
            IDictionary<ushort, Action<DataInterceptedEventArgs>> callbacks)
        {
            try
            {
                e.Packet.Position = 0;
                if (callbacks.ContainsKey(e.Packet.Header))
                    callbacks[e.Packet.Header](e);

                HMessage previous = null;
                if (previousMessages.Count > 0)
                {
                    previous = previousMessages.Pop();
                    previous.Position = 0;
                }

                e.Packet.Position = 0;
                handler(e.Packet, previous);
            }
            finally
            {
                e.Packet.Position = 0;
                previousMessages.Push(e.Packet);
            }
        }

        public void HandleOutgoing(DataInterceptedEventArgs e)
        {
            HandleMessage(e, _outPreviousMessages, HandleOutgoing, _outCallbacks);
        }
        protected virtual void HandleOutgoing(HMessage current, HMessage previous)
        { }

        public void HandleIncoming(DataInterceptedEventArgs e)
        {
            HandleMessage(e, _inPreviousMessages, HandleIncoming, _inCallbacks);
        }
        protected virtual void HandleIncoming(HMessage current, HMessage previous)
        { }
    }
}