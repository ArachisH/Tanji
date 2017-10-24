using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Tanji.Controls
{
    [DesignerCategory("Code")]
    public class BindableToolStripMenuItem : ToolStripMenuItem, IBindableComponent
    {
        private BindingContext _context;
        [Browsable(false)]
        public BindingContext BindingContext
        {
            get => (_context ?? (_context = new BindingContext()));
            set => _context = value;
        }

        private ControlBindingsCollection _bindings;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ControlBindingsCollection DataBindings
        {
            get
            {
                return (_bindings ??
                    (_bindings = new ControlBindingsCollection(this)));
            }
        }

        public BindableToolStripMenuItem()
        { }
        public BindableToolStripMenuItem(string text)
            : base(text)
        { }
        public BindableToolStripMenuItem(Image image)
            : base(image)
        { }
        public BindableToolStripMenuItem(string text, Image image)
            : base(text, image)
        { }
        public BindableToolStripMenuItem(string text, Image image, EventHandler onClick)
            : base(text, image, onClick)
        { }
        public BindableToolStripMenuItem(string text, Image image, EventHandler onClick, string name)
            : base(text, image, onClick, name)
        { }
        public BindableToolStripMenuItem(string text, Image image, params ToolStripItem[] dropDownItems)
            : base(text, image, dropDownItems)
        { }
        public BindableToolStripMenuItem(string text, Image image, EventHandler onClick, Keys shortcutKeys)
            : base(text, image, onClick, shortcutKeys)
        { }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            UpdateBindings();
        }

        protected void UpdateBindings()
        {
            foreach (Binding binding in DataBindings)
            {
                binding.WriteValue();
            }
        }
    }
}