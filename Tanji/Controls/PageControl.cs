using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Tanji.Controls;

[ToolboxItem(false)]
[DesignerCategory("Code")]
public class PageControl : UserControl
{
    public PageControl()
    {
        ApplyDefault();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyDefault();
    }

    private void ApplyDefault()
    {
#if DEBUG
        BackColor = Color.DarkSlateGray;
#else
        BackColor = Color.White;
#endif
        Size = new Size(536, 387);
    }
}