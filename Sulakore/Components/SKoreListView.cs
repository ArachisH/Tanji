using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreListView : ListView
    {
        private bool _lastSelectionState;
        private ListViewItem _previouslySelectedItem;

        /// <summary>
        /// Occurs when an item's selection state differs from the previous state.
        /// </summary>
        public event EventHandler ItemSelectionStateChanged;
        protected virtual void OnItemSelectionStateChanged(EventArgs e)
        {
            if (_lastSelectionState != HasSelectedItem)
            {
                _lastSelectionState = HasSelectedItem;
                ItemSelectionStateChanged?.Invoke(this, e);
            }

            if (!_lastSelectionState)
                _previouslySelectedItem = null;

            OnItemSelected(e);
        }

        /// <summary>
        /// Occurs when an item has been selected for the first time.
        /// </summary>
        public event EventHandler ItemSelected;
        protected virtual void OnItemSelected(EventArgs e)
        {
            if (HasSelectedItem &&
                (SelectedItem != _previouslySelectedItem))
            {
                _previouslySelectedItem = SelectedItem;
                ItemSelected?.Invoke(this, e);
            }
        }

        [DefaultValue(true)]
        public bool LockColumnWidth { get; set; }

        [Browsable(false)]
        public bool HasSelectedItem => (SelectedItems.Count > 0);

        [Browsable(false)]
        public ListViewItem SelectedItem =>
            (HasSelectedItem ? SelectedItems[0] : null);

        [Browsable(false)]
        public bool CanMoveSelectedItemUp
        {
            get
            {
                if (!HasSelectedItem) return false;
                return (SelectedItem.Index >= 1);
            }
        }

        [Browsable(false)]
        public bool CanMoveSelectedItemDown
        {
            get
            {
                if (!HasSelectedItem) return false;
                return (SelectedItem.Index != (Items.Count - 1));
            }
        }

        public SKoreListView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            GridLines = true;
            View = View.Details;
            MultiSelect = false;
            FullRowSelect = true;
            HideSelection = false;
            LockColumnWidth = true;
            ShowItemToolTips = true;
            UseCompatibleStateImageBehavior = false;
            HeaderStyle = ColumnHeaderStyle.Nonclickable;
        }

        public void ClearItems()
        {
            var items = new ListViewItem[Items.Count];
            Items.CopyTo(items, 0);
            ClearItems(items);
        }
        protected virtual void ClearItems(IEnumerable<ListViewItem> items)
        {
            try
            {
                BeginUpdate();
                foreach (ListViewItem item in items)
                    RemoveItem(item, false);
            }
            finally { EndUpdate(); }
        }

        public void RemoveSelectedItem()
        {
            if (HasSelectedItem)
                RemoveItem(SelectedItem, true);
        }
        public void RemoveItem(ListViewItem item)
        {
            RemoveItem(item, true);
        }
        protected virtual void RemoveItem(ListViewItem item, bool selectNext)
        {
            int index = item.Index;
            selectNext = (Items.Count - 1 > 0 && selectNext);

            Items.RemoveAt(index);
            if (selectNext)
            {
                if (index >= Items.Count)
                    index = Items.Count - 1;

                item = Items[index];
                item.Selected = true;
                EnsureVisible(item.Index);
            }
            OnItemSelectionStateChanged(EventArgs.Empty);
        }

        public void MoveSelectedItemUp()
        {
            if (HasSelectedItem)
                MoveItemUp(SelectedItem);
        }
        protected virtual void MoveItemUp(ListViewItem item)
        {
            int oldIndex = item.Index;
            if (oldIndex < 1) return;
            _previouslySelectedItem = null;

            BeginUpdate();
            Items.RemoveAt(oldIndex);
            Items.Insert(oldIndex - 1, item);
            EndUpdate();

            item.Selected = true;
            OnItemSelectionStateChanged(EventArgs.Empty);

            int index = item.Index;
            EnsureVisible(index <= 4 ? 0 : index - 4);
        }

        public void MoveSelectedItemDown()
        {
            if (HasSelectedItem)
                MoveItemDown(SelectedItem);
        }
        protected virtual void MoveItemDown(ListViewItem item)
        {
            int oldIndex = item.Index;
            if (oldIndex == (Items.Count - 1)) return;
            _previouslySelectedItem = null;

            BeginUpdate();
            Items.RemoveAt(oldIndex);
            Items.Insert(oldIndex + 1, item);
            EndUpdate();

            item.Selected = true;
            OnItemSelectionStateChanged(EventArgs.Empty);

            int index = item.Index;
            EnsureVisible(index + 4 >= Items.Count ? Items.Count - 1 : index + 4);
        }

        public void AddFocusedItem(ListViewItem item)
        {
            AddItem(item);
            item.Selected = true;
            OnItemSelectionStateChanged(EventArgs.Empty);
        }
        public ListViewItem AddFocusedItem(params object[] items)
        {
            ListViewItem item = AddItem(items);
            item.Selected = true;

            OnItemSelectionStateChanged(EventArgs.Empty);
            return item;
        }

        public void AddItem(ListViewItem item)
        {
            Focus();
            Items.Add(item);
            item.EnsureVisible();
        }
        public ListViewItem AddItem(params object[] items)
        {
            string[] stringItems = items
                .Select(i => i.ToString()).ToArray();

            var item = new ListViewItem(stringItems);
            AddItem(item);

            return item;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            OnItemSelectionStateChanged(EventArgs.Empty);
            base.OnMouseUp(e);
        }
        protected override void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
        {
            if (LockColumnWidth && !DesignMode)
            {
                e.Cancel = true;
                e.NewWidth = Columns[e.ColumnIndex].Width;
            }
            base.OnColumnWidthChanging(e);
        }
    }
}