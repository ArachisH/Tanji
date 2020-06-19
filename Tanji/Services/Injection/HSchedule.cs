using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Sulakore.Network.Protocol;

namespace Tanji.Services.Injection
{
    public class HSchedule
    {
        private CancellationTokenSource _cancelSource;

        public int Cycles { get; }
        public int Interval { get; }
        public Keys Hotkeys { get; }
        public bool ToServer { get; }
        public HPacket Packet { get; }

        public HSchedule(HPacket packet, bool toServer, int interval, int cycles, Keys hotkeys)
        {
            Packet = packet;
            Cycles = cycles;
            Hotkeys = hotkeys;
            ToServer = toServer;
            Interval = interval;
        }

        public Task ToggleAsync(bool enable)
        {
            if (enable)
            {
                _cancelSource ??= new CancellationTokenSource();
                return ScheduleAndForgetAsync(_cancelSource);
            }
            else
            {
                _cancelSource.Cancel(false);
                _cancelSource = null;
            }
            return Task.CompletedTask;
        }

        private async Task ScheduleAndForgetAsync(CancellationTokenSource cancelSource)
        {
            int cycles = Cycles;
            do
            {
                try
                {
                    await Program.Master.SendAsync(Packet, ToServer).ConfigureAwait(false);
                    if (--cycles == 0) break;
                    await Task.Delay(Interval, cancelSource.Token).ConfigureAwait(false);
                }
                catch (TaskCanceledException) { /* Schedule was cancelled.  */ }
            }
            while ((Cycles == 0 || cycles > 0) && !cancelSource.IsCancellationRequested);
        }
    }
}