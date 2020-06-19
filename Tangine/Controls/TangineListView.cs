using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace Tangine.Controls
{
    [DesignerCategory("Code")]
    public class TangineListView : ListView
    {
        private bool _lastSelectionState;
        private ListViewItem _previouslySelectedItem, _expectedSelection, _grabbedItem;

        /// <summary>
        /// Occurs when the user finishes dragging an item.
        /// </summary>
        public event EventHandler ItemDragged;
        protected virtual void OnItemDragged(EventArgs e)
        {
            ItemDragged?.Invoke(this, e);
        }

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
        public bool HasSelectedItem => SelectedItems.Count > 0;

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

        private int _fillColumnIndex = -1;
        [DefaultValue(-1)]
        public int FillColumnIndex
        {
            get => _fillColumnIndex;
            set
            {
                _fillColumnIndex = value;
                Invalidate();
            }
        }

        private bool _isAutoResizingColumns = true;
        [DefaultValue(true)]
        public bool IsAutoResizingColumns
        {
            get => _isAutoResizingColumns;
            set
            {
                _isAutoResizingColumns = value;
                Invalidate();
            }
        }

        public TangineListView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            OwnerDraw = true;
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
                {
                    RemoveItem(item, false, false);
                }
            }
            finally
            {
                EndUpdate();
                AutoResizeColumn();
            }
        }

        public void RemoveSelectedItem()
        {
            if (HasSelectedItem)
            {
                RemoveItem(SelectedItem, true);
            }
        }
        public void RemoveItem(ListViewItem item)
        {
            RemoveItem(item, true);
        }
        protected virtual void RemoveItem(ListViewItem item, bool selectNext, bool resizeColumns = true)
        {
            int index = item.Index;
            selectNext = Items.Count - 1 > 0 && selectNext;

            Items.RemoveAt(index);
            if (selectNext)
            {
                if (index >= Items.Count)
                    index = Items.Count - 1;

                item = Items[index];
                item.Selected = true;
                EnsureVisible(item.Index);
            }

            if (resizeColumns)
            {
                AutoResizeColumn();
            }
            OnItemSelectionStateChanged(EventArgs.Empty);
        }

        public void MoveSelectedItemUp()
        {
            if (HasSelectedItem)
            {
                MoveItemUp(SelectedItem);
            }
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

        public void AddSelectedItem(ListViewItem item)
        {
            AddItem(item);
            item.Selected = true;
            OnItemSelectionStateChanged(EventArgs.Empty);
        }
        public ListViewItem AddSelectedItem(params string[] subItems)
        {
            ListViewItem item = AddItem(subItems);
            item.Selected = true;

            OnItemSelectionStateChanged(EventArgs.Empty);
            return item;
        }

        public void AddItem(ListViewItem item)
        {
            Items.Add(item);
            item.EnsureVisible();
            AutoResizeColumn();
        }
        public ListViewItem AddItem(params string[] subItems)
        {
            var item = new ListViewItem(subItems);
            AddItem(item);
            return item;
        }

        private void AutoResizeColumn()
        {
            if (DesignMode || !IsAutoResizingColumns || Columns.Count == 0) return;

            int totalWidth = 0;
            for (int i = 0; i < Columns.Count; i++)
            {
                totalWidth += Columns[i].Width;
            }

            int difference = ClientSize.Width - totalWidth;
            Columns[FillColumnIndex == -1 ? ^1 : FillColumnIndex].Width += difference;
        }
        private void RemoveFocusRectangles()
        {
            var msg = Message.Create(Handle, 0x127, new IntPtr(0x10001), new IntPtr(0));
            WndProc(ref msg);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!ClientRectangle.Contains(PointToClient(MousePosition)))
            {
                _grabbedItem = null;
            }
            base.OnMouseLeave(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_grabbedItem != null)
            {
                ListViewItem pushedItem = GetItemAt(e.X, e.Y);
                if (pushedItem != null)
                {
                    int newIndex = pushedItem.Index;

                    BeginUpdate();
                    Items.Remove(_grabbedItem);
                    Items.Insert(newIndex, _grabbedItem);
                    EndUpdate();

                    _grabbedItem.Selected = true;
                    OnItemSelectionStateChanged(EventArgs.Empty);
                    _grabbedItem = null;

                    EnsureVisible(newIndex);
                    OnItemDragged(EventArgs.Empty);
                }
            }
            base.OnMouseUp(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _expectedSelection = GetItemAt(e.X, e.Y);
            base.OnMouseDown(e);
        }

        protected override void OnCreateControl()
        {
            AutoResizeColumn();
            base.OnCreateControl();
        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            RemoveFocusRectangles();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (SelectedItem != null)
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete: SelectedItem.Checked = false; break;
                    case Keys.Enter: SelectedItem.Checked = !SelectedItem.Checked; break;
                }
            }
            base.OnKeyDown(e);
        }
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (_expectedSelection == SelectedItem || SelectedItem != null)
            {
                OnItemSelectionStateChanged(EventArgs.Empty);
            }
            base.OnSelectedIndexChanged(e);
            RemoveFocusRectangles();
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_grabbedItem != null)
            {
                ListViewItem itemToPush = HitTest(e.Location).Item;
                if (itemToPush != null)
                {
                    InsertionMark.Index = itemToPush.Index;
                    InsertionMark.Color = Color.FromArgb(243, 63, 63);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            _grabbedItem = (ListViewItem)e.Item;
            base.OnItemDrag(e);
        }
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawItem(e);
        }
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
            base.OnDrawSubItem(e);
        }
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            e.Graphics.DrawLine(SystemPens.GradientInactiveCaption, e.Bounds.X, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            if (e.ColumnIndex > 0)
            {
                e.Graphics.DrawLine(SystemPens.GradientInactiveCaption, e.Bounds.X, 0, e.Bounds.X, e.Bounds.Bottom);
            }

            TextFormatFlags format = Columns[e.ColumnIndex].TextAlign switch
            {
                HorizontalAlignment.Left => TextFormatFlags.Left,
                HorizontalAlignment.Right => TextFormatFlags.Right,
                HorizontalAlignment.Center => TextFormatFlags.HorizontalCenter,
                _ => TextFormatFlags.Default
            };
            format |= TextFormatFlags.VerticalCenter;

            TextRenderer.DrawText(e.Graphics, e.Header.Text, Font, e.Bounds, Color.Black, format);
            base.OnDrawColumnHeader(e);
        }
    }
}