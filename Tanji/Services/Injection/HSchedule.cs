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

        public ListViewItem PhysicalItem { get; }
        public bool IsLinkActivated { get; set; }
        public bool IsChainLink { get; private set; }
        public HSchedule Parent { get; private set; }
        public IReadOnlyList<HSchedule> Chain { get; }

        public HSchedule(HPacket packet, bool toServer, int interval, int cycles, Keys hotkeys, ListViewItem item)
        {
            Packet = packet;
            Cycles = cycles;
            Hotkeys = hotkeys;
            ToServer = toServer;
            Interval = interval;
            PhysicalItem = item;

            _chain = new List<HSchedule>();
            Chain = _chain.AsReadOnly();
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
            AddToChain(schedule, _chain.Count);
        }
        public void AddToChain(HSchedule schedule, int index)
        {
            schedule.Parent = this;
            schedule.IsChainLink = true;

            _chain.Insert(index, schedule);
            _callsToAlign++;
        }

        public int GetChainedScheduleIndex(HSchedule schedule) => _chain.IndexOf(schedule);

        public void RemoveFromChain(HSchedule schedule)
        {
            schedule.Parent = null;
            schedule.IsChainLink = false;

            _chain.Remove(schedule);
            _callsToAlign++;
        }
        public void MoveChainedSchedule(HSchedule schedule, int index)
        {
            _chain.Remove(schedule);
            _chain.Insert(index, schedule);
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
                    System.Diagnostics.Debug.WriteLine("Packet Sent...");
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
#if DEBUG
                        System.Diagnostics.Debug.WriteLine($"[Chain:{i}]Waiting {chainedSchedule.Interval}ms... ");
#endif
                        await Task.Delay(chainedSchedule.Interval, cancelSource.Token).ConfigureAwait(false);
#if DEBUG
                        System.Diagnostics.Debug.WriteLine($"[Chain: {i}]Packet Sent...");
#endif
#if !INTERFACEDEBUG
                        await Program.Master.SendAsync(chainedSchedule.Packet, chainedSchedule.ToServer).ConfigureAwait(false);
#endif
                    }

                    if (Cycles > 0 && --cycles == 0) break;
#if DEBUG
                    System.Diagnostics.Debug.WriteLine($"Waiting {Interval}ms... ");
#endif
                    await Task.Delay(Interval, cancelSource.Token).ConfigureAwait(false);
                }
                catch (TaskCanceledException) { /* Schedule was cancelled.  */ }
            }
            while ((Cycles == 0 || cycles > 0) && !cancelSource.IsCancellationRequested);
        }
    }
}