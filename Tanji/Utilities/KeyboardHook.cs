using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Tanji.Utilities
{
    public class KeyboardHook : NativeWindow, IDisposable
    {
        private const int WM_HOTKEY = 0x312;

        private readonly Keys[] _keyModifiers;
        private readonly Modifiers[] _modifiers;
        private readonly Dictionary<Modifiers, Dictionary<Keys, int>> _registeredKeys;

        public event EventHandler<KeyEventArgs> HotkeyActivated;
        protected virtual void OnKeyPressed(KeyEventArgs e)
        {
            HotkeyActivated?.Invoke(this, e);
        }

        public bool IsDisposed { get; private set; }

        public KeyboardHook()
        {
            _keyModifiers = new Keys[3] { Keys.Alt, Keys.Shift, Keys.Control };
            _modifiers = new Modifiers[3] { Modifiers.Alt, Modifiers.Shift, Modifiers.Control };
            _registeredKeys = new Dictionary<Modifiers, Dictionary<Keys, int>>();

            CreateHandle(new CreateParams());
        }

        public void RegisterHotkey(Keys keyData)
        {
            lock (_registeredKeys)
            {
                Keys keyCode = Keys.None;
                Modifiers modifiers = GetModifiers(keyData, out keyCode);

                if (!_registeredKeys.ContainsKey(modifiers))
                    _registeredKeys[modifiers] = new Dictionary<Keys, int>();

                var id = (int)DateTime.Now.Ticks;
                if (!_registeredKeys[modifiers].ContainsKey(keyCode))
                {
                    _registeredKeys[modifiers][keyCode] = id;
                    NativeMethods.RegisterHotKey(Handle,
                        id, (uint)modifiers, (uint)keyCode);
                }
            }
        }
        public void UnregisterHotkey(Keys keyData)
        {
            lock (_registeredKeys)
            {
                Keys keyCode = Keys.None;
                Modifiers modifiers = GetModifiers(keyData, out keyCode);

                if (!_registeredKeys.ContainsKey(modifiers)) return;
                if (!_registeredKeys[modifiers].ContainsKey(keyCode)) return;

                NativeMethods.UnregisterHotKey(Handle,
                    _registeredKeys[modifiers][keyCode]);

                _registeredKeys[modifiers].Remove(keyCode);
            }
        }

        private Keys GetModifierKeys(Modifiers modifiers)
        {
            var modifierKeys = Keys.None;
            foreach (Modifiers modifier in _modifiers)
            {
                if (!modifiers.HasFlag(modifier)) continue;
                switch (modifier)
                {
                    case Modifiers.Alt:
                    modifierKeys |= Keys.Alt;
                    break;

                    case Modifiers.Shift:
                    modifierKeys |= Keys.Shift;
                    break;

                    case Modifiers.Control:
                    modifierKeys |= Keys.Control;
                    break;

                    default:
                    throw new ArgumentException("Invalid Modifier(s): " + modifiers);
                }
            }
            return modifierKeys;
        }
        private Modifiers GetModifiers(Keys keyData, out Keys keyCode)
        {
            Keys keyModifiers = (keyData & Keys.Modifiers);
            keyCode = (keyData & ~keyModifiers);

            var modifiers = Modifiers.None;
            foreach (Keys modifier in _keyModifiers)
            {
                if (!keyModifiers.HasFlag(modifier)) continue;
                switch (modifier)
                {
                    case Keys.Alt:
                    modifiers |= Modifiers.Alt;
                    break;

                    case Keys.Shift:
                    modifiers |= Modifiers.Shift;
                    break;

                    case Keys.Control:
                    modifiers |= Modifiers.Control;
                    break;

                    default:
                    throw new ArgumentException("Invalid Modifier(s): " + keyModifiers);
                }
            }
            return modifiers;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                var param = (int)m.LParam;
                var modifier = (Modifiers)(param & 0xFFFF);

                Keys keyData = (GetModifierKeys(modifier) |
                    (Keys)((param >> 16) & 0xFFFF));

                OnKeyPressed(new KeyEventArgs(keyData));
            }
            base.WndProc(ref m);
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
                foreach (Dictionary<Keys, int> keys in _registeredKeys.Values)
                {
                    foreach (int identifier in keys.Values)
                    {
                        NativeMethods.UnregisterHotKey(
                            Handle, identifier);
                    }
                }
                _registeredKeys.Clear();
                DestroyHandle();
            }
            IsDisposed = true;
        }

        [Flags]
        private enum Modifiers : uint
        {
            None = 0x0,
            Alt = 0x1,
            Control = 0x2,
            Shift = 0x4
        }
    }
}