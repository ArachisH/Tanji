﻿using System.Threading;
using System.ComponentModel;

using Sulakore.Protocol;

namespace Tanji.Controls;

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
    public CancellationToken Cancellation => _schedule.CancellationSource.Token;

    public ScheduleTickEventArgs(SKoreScheduleView.HSchedule schedule, int currentCycle)
    {
        _schedule = schedule;

        CurrentCycle = currentCycle;
    }
}