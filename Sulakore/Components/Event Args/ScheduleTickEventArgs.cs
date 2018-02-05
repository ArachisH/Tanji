using System.ComponentModel;

using Sulakore.Protocol;

namespace Sulakore.Components
{
    public class ScheduleTickEventArgs : CancelEventArgs
    {
        private readonly SKoreScheduleView.HSchedule _schedule;

        public int CurrentCycle { get; }
        public int Cycles => _schedule.Cycles;

        public double Interval
        {
            get => _schedule.Interval;
            set => _schedule.Interval = value;
        }
        public HMessage Packet
        {
            get => _schedule.Packet;
            set => _schedule.Packet = value;
        }

        public ScheduleTickEventArgs(SKoreScheduleView.HSchedule schedule, int currentCycle)
        {
            _schedule = schedule;

            CurrentCycle = currentCycle;
        }
    }
}