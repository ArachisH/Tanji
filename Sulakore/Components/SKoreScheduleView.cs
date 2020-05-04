using System;
using System.Timers;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreScheduleView : SKoreListView
    {
        private readonly Action<HSchedule> _updateItem;
        private readonly Dictionary<HSchedule, ListViewItem> _items;

        public event EventHandler<ScheduleTickEventArgs> ScheduleTick;
        protected virtual void OnScheduleTick(object sender, ScheduleTickEventArgs e)
        {
            bool isDirty = false;
            var schedule = (HSchedule)sender;
            try
            {
                string oldpacket = e.Packet.ToString();
                ScheduleTick?.Invoke(this, e);
                string newPacket = e.Packet.ToString();

                if (oldpacket != newPacket ||
                    e.Interval > 0 && schedule.Interval != e.Interval)
                {
                    isDirty = true;
                }
            }
            catch { e.Cancel = true; }
            finally
            {
                schedule.IsRunning = !e.Cancel;
                if (e.Cancel)
                {
                    schedule.CancellationSource.Cancel();
                    schedule.CancellationSource = new CancellationTokenSource();
                }

                if (isDirty || e.Cancel)
                    Invoke(_updateItem, schedule);
            }
        }

        protected HSchedule SelectedSchedule
        {
            get
            {
                if (!HasSelectedItem)
                    return null;

                var schedule =
                    (SelectedItem.Tag as HSchedule);

                return schedule;
            }
        }

        public SKoreScheduleView()
        {
            _updateItem = UpdateItem;
            _items = new Dictionary<HSchedule, ListViewItem>();

            CheckBoxes = true;
        }

        public ListViewItem AddSchedule(HMessage packet, int interval, int cycles, bool autoStart)
        {
            ListViewItem item = AddFocusedItem(packet, packet.Destination, interval, cycles);

            var schedule = new HSchedule(packet, interval, cycles);
            schedule.ScheduleTick += OnScheduleTick;

            _items.Add(schedule, item);
            item.Tag = schedule;
            item.Checked = autoStart;

            return item;
        }

        protected virtual void UpdateItem(HSchedule schedule)
        {
            ListViewItem item = _items[schedule];

            if (item.Checked != schedule.IsRunning)
                item.Checked = schedule.IsRunning;

            item.SubItems[0].Text = schedule.Packet.ToString();
            item.SubItems[1].Text = schedule.Packet.Destination.ToString();
            item.SubItems[2].Text = schedule.Interval.ToString();
            item.SubItems[3].Text = schedule.Cycles.ToString();
        }

        protected override void OnItemChecked(ItemCheckedEventArgs e)
        {
            var schedule = (e.Item.Tag as HSchedule);
            if (schedule != null)
            {
                bool isRunning = e.Item.Checked;
                schedule.IsRunning = isRunning;
                if (!isRunning)
                {
                    schedule.CancellationSource.Cancel();
                    schedule.CancellationSource = new CancellationTokenSource();
                }
            }
            base.OnItemChecked(e);
        }
        protected override void RemoveItem(ListViewItem item, bool selectNext)
        {
            var schedule = (item.Tag as HSchedule);
            if (schedule != null)
            {
                if (_items.ContainsKey(schedule))
                    _items.Remove(schedule);

                schedule.Dispose();
            }
            base.RemoveItem(item, selectNext);
        }

        public class HSchedule : IDisposable
        {
            private int _cycles;
            private int _currentCycle;
            private readonly System.Timers.Timer _ticker;

            public event EventHandler<ScheduleTickEventArgs> ScheduleTick;
            protected virtual void OnScheduleTick(ScheduleTickEventArgs e)
            {
                try
                {
                    if (Cycles != 0 && _currentCycle >= Cycles)
                    {
                        IsRunning = false;
                        e.Cancel = true;
                    }
                    ScheduleTick?.Invoke(this, e);
                }
                catch { e.Cancel = true; }
                finally
                {
                    if (e.Cancel)
                        IsRunning = false;
                }
            }

            public int Cycles
            {
                get => _cycles;
                set
                {
                    if (_cycles >= 0)
                    {
                        _cycles = value;
                    }
                }
            }
            public bool IsRunning
            {
                get => _ticker.Enabled;
                set
                {
                    // The same value is being set, ignore operation
                    if (_ticker.Enabled == value) return;

                    _currentCycle = 0;
                    _ticker.Enabled = value;
                }
            }
            public double Interval
            {
                get => _ticker.Interval;
                set
                {
                    if (value > 0)
                        _ticker.Interval = value;
                }
            }
            public ISynchronizeInvoke SynchronizingObject
            {
                get => _ticker.SynchronizingObject;
                set => _ticker.SynchronizingObject = value;
            }

            public HMessage Packet { get; set; }
            public bool IsDisposed { get; private set; }
            public CancellationTokenSource CancellationSource { get; set; }

            public HSchedule(HMessage packet, int interval, int cycles)
            {
                _ticker = new System.Timers.Timer(interval);
                _ticker.Elapsed += Elapsed;

                Packet = packet;
                Cycles = cycles;
                CancellationSource = new CancellationTokenSource();
            }

            private void Elapsed(object sender, ElapsedEventArgs e)
            {
                lock (_ticker)
                {
                    if (!IsRunning) return;
                    if (Cycles != 0) _currentCycle++;
                    OnScheduleTick(new ScheduleTickEventArgs(this, _currentCycle));
                }
            }

            public void Dispose()
            {
                Dispose(true);
            }
            protected virtual void Dispose(bool disposing)
            {
                if (IsDisposed) return;
                if (disposing)
                {
                    IsRunning = false;
                    _ticker.Dispose();

                    SKore.Unsubscribe(ref ScheduleTick);
                }
                IsDisposed = true;
            }
        }
    }
}