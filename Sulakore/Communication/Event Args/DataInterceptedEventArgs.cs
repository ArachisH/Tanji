using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Communication
{
    /// <summary>
    /// Represents an intercepted message that will be returned to the caller with blocking/replacing information.
    /// </summary>
    public class DataInterceptedEventArgs : ContinuableEventArgs
    {
        private readonly byte[] _ogData;
        private readonly string _ogString;
        private readonly HDestination _ogDestination;

        public int Step { get; }
        public HMessage Packet { get; set; }
        public List<HMessage> Executions { get; }

        public bool IsOriginal
        {
            get
            {
                return (Packet.ToString().Equals(_ogString) &&
                    Packet.Destination == _ogDestination);
            }
        }
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataInterceptedEventArgs"/> class.
        /// </summary>
        /// <param name="continuation">The <see cref="Func{TResult}"/> of type <see cref="Task"/> that will be invoked when <see cref="ContinueRead"/> is called.</param>
        /// <param name="step">The current count/step/order from which this data was intercepted.</param>
        /// <param name="packet">The intercepted data to read/write from.</param>
        public DataInterceptedEventArgs(HMessage packet, int step, Func<Task> continuation)
            : base(continuation)
        {
            _ogData = packet.ToBytes();
            _ogString = packet.ToString();
            _ogDestination = packet.Destination;

            Step = step;
            Packet = packet;
            Executions = new List<HMessage>();
        }

        /// <summary>
        /// Restores the intercepted data to its initial form, before it was replaced/modified.
        /// </summary>
        public void Restore()
        {
            if (IsOriginal) return;
            Packet = new HMessage(_ogData, _ogDestination);
        }
    }
}