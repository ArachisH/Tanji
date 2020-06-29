using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Network.Protocol;

namespace Tanji.Services.Injection
{
    public class HSchedule
    {
        private int _callsToAlign;
        private readonly List<HSchedule> _chain;
        private CancellationTokenSource _cancelSource;

        public int Cycles { get; }
        public int Interval { get; }
        public Keys Hotkeys { get; }
        public bool ToServer { get; }
        public HPacket Packet { get; }

        public bool IsLinkActivated { get; set; }
        public bool IsChainLink { get; private set; }

        public HSchedule(HPacket packet, bool toServer, int interval, int cycles, Keys hotkeys)
        {
            Packet = packet;
            Cycles = cycles;
            Hotkeys = hotkeys;
            ToServer = toServer;
            Interval = interval;
            _chain = new List<HSchedule>();
        }

        public Task ToggleAsync(bool enable)
        {
            if (enable)
            {
                _cancelSource ??= new CancellationTokenSource();
                return ScheduleAndForgetAsync(Cycles, _cancelSource);
            }
            else if (_cancelSource != null)
            {
                _cancelSource.Cancel(false);
                _cancelSource = null;
            }
            return Task.CompletedTask;
        }
        public void AddToChain(HSchedule schedule)
        {
            schedule.IsChainLink = true;
            _chain.Add(schedule);
            AlignChainedSchedule(schedule, 0);
        }
        public void RemoveFromChain(HSchedule schedule)
        {
            schedule.IsChainLink = false;
            _chain.Remove(schedule);
            AlignChainedSchedule(schedule, 0);
        }
        public void AlignChainedSchedule(HSchedule schedule, int deviation)
        {

            int currentIndex = _chain.IndexOf(schedule);
            _chain.RemoveAt(currentIndex);
            _chain.Insert(currentIndex + deviation, schedule);
            _callsToAlign++;
        }

        private async Task ScheduleAndForgetAsync(int cycles, CancellationTokenSource cancelSource)
        {
            HSchedule[] chain = _chain.ToArray();
            do
            {
                try
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Schedule Triggered: +{Interval}ms");
#endif
#if !INTERFACEDEBUG
                    await Program.Master.SendAsync(Packet, ToServer).ConfigureAwait(false);
#endif
                    for (int i = 0; i < chain.Length || _callsToAlign > 0; i++)
                    {
                        if (_callsToAlign > 0)
                        {
                            i--;
                            int currentCallsToAlign = _callsToAlign;
                            chain = _chain.ToArray();

                            _callsToAlign -= currentCallsToAlign; // Big brain synchronized thread-safe schedule alignment.
#if DEBUG
                            if (_callsToAlign > 0) // This is fine, but let me know when it happens, I'm curious.
                            {
                                System.Diagnostics.Debugger.Break();
                            }
#endif
                            continue;
                        }

                        HSchedule chainedSchedule = chain[i];
                        if (!chainedSchedule.IsLinkActivated) continue;

                        await Task.Delay(chainedSchedule.Interval, cancelSource.Token).ConfigureAwait(false);
                        await chainedSchedule.ScheduleAndForgetAsync(chainedSchedule.Cycles == 0 ? 1 : chainedSchedule.Cycles, cancelSource).ConfigureAwait(false);
                    }

                    if ((Cycles > 0 || IsChainLink) && --cycles == 0) break;
                    await Task.Delay(Interval, cancelSource.Token).ConfigureAwait(false);
                }
                catch (TaskCanceledException) { /* Schedule was cancelled.  */ }
            }
            while ((Cycles == 0 || cycles > 0) && !cancelSource.IsCancellationRequested);
        }
    }
}