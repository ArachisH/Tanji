using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sulakore.Network.Protocol;

namespace Tanji.Services.Injection
{
    public class HSchedule
    {
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
            else
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
        }
        public void RemoveFromChain(HSchedule schedule)
        {
            schedule.IsChainLink = false;
            _chain.Remove(schedule);
        }

        private async Task ScheduleAndForgetAsync(int cycles, CancellationTokenSource cancelSource)
        {
            do
            {
                try
                {
#if INTERFACEDEBUG
                    System.Diagnostics.Debug.WriteLine($"Schedule Triggered: +{Interval}ms");
#else
                    await Program.Master.SendAsync(Packet, ToServer).ConfigureAwait(false);
#endif

                    foreach (HSchedule chainedSchedule in _chain)
                    {
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