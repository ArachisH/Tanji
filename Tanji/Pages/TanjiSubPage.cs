using System.Windows.Forms;

namespace Tanji.Pages
{
    public abstract class TanjiSubPage<T> : TanjiPage where T : TanjiPage
    {
        public T Parent { get; }

        public TanjiSubPage(T parent, TabPage tab)
            : base(parent.UI, tab)
        {
            Parent = parent;
        }

        public override void Select()
        {
            Parent.Select();
            base.Select();
        }
    }
}