using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Sulakore.Protocol;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreConstructView : SKoreListView
    {
        private HMessage _cachedPacket;

        private readonly List<object> _values;
        public IReadOnlyList<object> Values => _values;

        public SKoreConstructView()
        {
            _values = new List<object>();
        }

        public void WriteInteger(int value)
        {
            WriteInteger(value, 1);
        }
        public void WriteInteger(int value, int amount)
        {
            WriteValue("Integer", value,
                BigEndian.GetBytes(value), amount);
        }

        public void WriteBoolean(bool value)
        {
            WriteBoolean(value, 1);
        }
        public void WriteBoolean(bool value, int amount)
        {
            WriteValue("Boolean", value,
                BigEndian.GetBytes(value), amount);
        }

        public void WriteString(string value)
        {
            WriteString(value, 1);
        }
        public void WriteString(string value, int amount)
        {
            WriteValue("String", value,
                BigEndian.GetBytes(value), amount);
        }

        protected virtual void MoveValue(int oldIndex, int newIndex)
        {
            lock (_values)
            {
                object value = _values[oldIndex];
                _values.RemoveAt(oldIndex);
                _values.Insert(newIndex, value);
                _cachedPacket = null;
            }
        }
        protected virtual void WriteValue(string type, object value, byte[] data, int amount)
        {
            lock (_values)
            {
                BeginUpdate();
                string encoded = HMessage.ToString(data);
                for (int i = 0; i < amount; i++)
                {
                    _values.Add(value);
                    ListViewItem item = null;
                    if (i != (amount - 1))
                    {
                        item = AddItem(type, value, encoded);
                    }
                    else item = AddFocusedItem(type, value, encoded);
                }
                EndUpdate();
                _cachedPacket = null;
            }
        }
        
        protected override void MoveItemUp(ListViewItem item)
        {
            int oldIndex = item.Index;
            base.MoveItemUp(item);
            MoveValue(oldIndex, item.Index);
        }
        protected override void MoveItemDown(ListViewItem item)
        {
            int oldIndex = item.Index;
            base.MoveItemDown(item);
            MoveValue(oldIndex, item.Index);
        }
        protected override void RemoveItem(ListViewItem item, bool selectNext)
        {
            lock (_values)
            {
                _values.RemoveAt(item.Index);
                base.RemoveItem(item, selectNext);
                _cachedPacket = null;
            }
        }

        public void UpdateSelectedValue(object value)
        {
            if (HasSelectedItem)
                UpdateValue(SelectedItem, value);
        }
        protected virtual void UpdateValue(ListViewItem item, object value)
        {
            lock (_values)
            {
                byte[] data = null;
                switch (item.SubItems[0].Text)
                {
                    case "String": data = BigEndian.GetBytes((string)value); break;
                    case "Integer": data = BigEndian.GetBytes((int)value); break;
                    case "Boolean": data = BigEndian.GetBytes((bool)value); break;
                }

                string encoded = HMessage.ToString(data);
                item.SubItems[1].Text = value.ToString();
                item.SubItems[2].Text = encoded;

                _values[item.Index] = value;
                _cachedPacket = null;
            }
        }

        public HMessage GetPacket(ushort header)
        {
            lock (_values)
            {
                if (_cachedPacket == null)
                {
                    _cachedPacket =
                        new HMessage(header, _values.ToArray());
                }
                _cachedPacket.Header = header;
                return _cachedPacket;
            }
        }
        public string GetStructure(ushort header)
        {
            lock (_values)
            {
                string structure = $"{{l}}{{u:{header}}}";
                foreach (object value in _values)
                {
                    char type = Type.GetTypeCode(
                        value.GetType()).ToString().ToLower()[0];

                    structure += $"{{{type}:{value}}}";
                }
                return structure;
            }
        }
    }
}