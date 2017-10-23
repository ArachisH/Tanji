using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

using Sulakore.Protocol;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreInjectionMenu : ContextMenuStrip
    {
        private TextBox _inputBox;
        [Browsable(false)]
        public TextBox InputBox
        {
            get { return _inputBox; }
            set
            {
                if (value != null)
                {
                    _inputBox = value;
                    _inputBox.ContextMenuStrip = this;
                }
            }
        }

        #region Menu Items
        protected ToolStripMenuItem RealignBtn { get; }
        protected ToolStripMenuItem InsertParametersBtn { get; }

        protected ToolStripMenuItem StringBtn { get; }
        protected ToolStripMenuItem IntegerBtn { get; }
        protected ToolStripMenuItem ByteBooleanBtn { get; }

        protected ToolStripSeparator TopSeparator { get; }

        protected ToolStripMenuItem CutBtn { get; }
        protected ToolStripMenuItem CopyBtn { get; }
        protected ToolStripMenuItem PasteBtn { get; }

        protected ToolStripSeparator BottomSeparator { get; }

        protected ToolStripMenuItem UndoBtn { get; }
        protected ToolStripMenuItem SelectAllBtn { get; }
        #endregion

        public SKoreInjectionMenu(IContainer container)
            : base(container)
        {
            RealignBtn = CreateItem("Realign", Keys.R);
            RealignBtn.Click += RealignBtn_Click;

            InsertParametersBtn = CreateItem("Insert Parameter(s)");
            TopSeparator = new ToolStripSeparator();

            StringBtn = CreateItem("String", Keys.S);
            StringBtn.Click += StringBtn_Click;

            IntegerBtn = CreateItem("Integer", Keys.I);
            IntegerBtn.Click += IntegerBtn_Click;

            ByteBooleanBtn = CreateItem("Byte/Boolean", Keys.B);
            ByteBooleanBtn.Click += ByteBooleanBtn_Click;

            InsertParametersBtn.DropDownItems.Add(StringBtn);
            InsertParametersBtn.DropDownItems.Add(IntegerBtn);
            InsertParametersBtn.DropDownItems.Add(ByteBooleanBtn);

            CutBtn = CreateItem("Cut", Keys.X);
            CutBtn.Click += CutBtn_Click;

            CopyBtn = CreateItem("Copy", Keys.C);
            CopyBtn.Click += CopyBtn_Click;

            PasteBtn = CreateItem("Paste", Keys.V);
            PasteBtn.Click += PasteBtn_Click;

            BottomSeparator = new ToolStripSeparator();

            UndoBtn = CreateItem("Undo", Keys.Z);
            UndoBtn.Click += UndoBtn_Click;

            SelectAllBtn = CreateItem("Select All", Keys.A);
            SelectAllBtn.Click += SelectAllBtn_Click;

            Items.AddRange(new ToolStripItem[]
            {
                RealignBtn,
                InsertParametersBtn,
                TopSeparator,
                CutBtn,
                CopyBtn,
                PasteBtn,
                BottomSeparator,
                UndoBtn,
                SelectAllBtn
            });
        }

        public void InsertString()
        {
            InsertParameter("s");
        }
        public void InsertInteger()
        {
            InsertParameter("i");
        }
        public void InsertByteBoolean()
        {
            InsertParameter("b");
        }

        protected void InsertParameter(string parameterName)
        {
            if (InputBox == null) return;
            string selectedValue = InputBox.SelectedText;

            object parameterValue = null;
            byte[] parameterData = HMessage.ToBytes(selectedValue);

            switch (parameterName)
            {
                case "i":
                {
                    if (parameterData.Length == 4)
                    {
                        parameterValue = BigEndian.ToInt32(parameterData, 0);
                    }
                    break;
                }
                case "s":
                {
                    if (parameterData.Length >= 2 &&
                        (BigEndian.ToUInt16(parameterData, 0) == (parameterData.Length - 2)))
                    {
                        parameterValue = Encoding.UTF8.GetString(
                            parameterData, 2, parameterData.Length - 2);
                    }
                    break;
                }
                case "b":
                {
                    if (parameterData.Length == 1)
                    {
                        parameterValue = BigEndian.ToBoolean(parameterData, 0);
                    }
                    break;
                }
            }
            InsertParameter(parameterName, parameterValue);
        }
        protected virtual void InsertParameter(string parameterName, object parameterValue)
        {
            string formattedParameter = $"{{{parameterName}:{parameterValue}}}";

            int parameterStart = (parameterValue == null ?
                InputBox.TextLength : InputBox.SelectionStart);

            if (parameterValue != null)
            {
                InputBox.Text = InputBox.Text.Remove(
                    parameterStart, InputBox.SelectionLength);
            }

            InputBox.Text = InputBox.Text.Insert(
                parameterStart, formattedParameter);

            InputBox.Select(parameterStart + 3,
                (parameterValue?.ToString() ?? string.Empty).Length);
        }

        private void RealignBtn_Click(object sender, EventArgs e)
        {
            if (InputBox == null) return;

            int packetHeaderEnd = 0;
            if (!InputBox.Text.StartsWith("{l}{u:"))
            {
                ushort packetHeader = 0;
                byte[] packetData = HMessage.ToBytes(InputBox.Text);
                if (packetData.Length >= 6)
                {
                    int packetLength = BigEndian.ToInt32(packetData, 0);
                    packetHeader = BigEndian.ToUInt16(packetData, 4);

                    byte[] headerData = new byte[6];
                    Buffer.BlockCopy(packetData, 0, headerData, 0, 6);

                    if (packetLength == (packetData.Length - 4))
                    {
                        string formattedHeader = HMessage.ToString(headerData);
                        InputBox.Text = InputBox.Text.Remove(0, formattedHeader.Length);
                    }
                }
                InputBox.Text = InputBox.Text.Insert(0, $"{{l}}{{u:{packetHeader}}}");
                packetHeaderEnd = packetHeader.ToString().Length;
            }
            else
            {
                string formattedHeader = InputBox.Text
                    .GetChild("{l}{u:").GetParent("}");

                packetHeaderEnd = formattedHeader.Length;
            }
            InputBox.Select(6, packetHeaderEnd);
            InputBox.ScrollToCaret();
        }

        private void StringBtn_Click(object sender, EventArgs e)
        {
            InsertString();
        }
        private void IntegerBtn_Click(object sender, EventArgs e)
        {
            InsertInteger();
        }
        private void ByteBooleanBtn_Click(object sender, EventArgs e)
        {
            InsertByteBoolean();
        }

        private void CutBtn_Click(object sender, EventArgs e)
        {
            InputBox?.Cut();
        }
        private void CopyBtn_Click(object sender, EventArgs e)
        {
            InputBox?.Copy();
        }
        private void PasteBtn_Click(object sender, EventArgs e)
        {
            InputBox?.Paste();
        }

        private void UndoBtn_Click(object sender, EventArgs e)
        {
            InputBox?.Undo();
        }
        private void SelectAllBtn_Click(object sender, EventArgs e)
        {
            InputBox?.SelectAll();
        }

        protected ToolStripMenuItem CreateItem(string text)
        {
            return CreateItem(text, Keys.None);
        }
        protected virtual ToolStripMenuItem CreateItem(string text, Keys ctrlModifier)
        {
            var item = new ToolStripMenuItem(text);

            if (ctrlModifier != Keys.None && ctrlModifier != Keys.Control)
                item.ShortcutKeys = (Keys.Control | ctrlModifier);

            return item;
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            UndoBtn.Enabled = (InputBox?.CanUndo ?? false);
            base.OnOpening(e);
        }
    }
}