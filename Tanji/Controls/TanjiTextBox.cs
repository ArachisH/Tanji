using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Tanji.Controls;

[ToolboxItem(false)]
public sealed class TanjiTextBox : TextBox
{
    private readonly TanjiLabelBox _lbxParent;

    public bool IsNumbersOnly { get; set; }

    public override bool AutoSize
    {
        get => base.AutoSize;
        set => base.AutoSize = value;
    }

    public TanjiTextBox(TanjiLabelBox lbxParent)
    {
        _lbxParent = lbxParent;

        TabStop = true;
        AutoSize = false;
        Dock = DockStyle.Right;
        ForeColor = Color.Black;
        BackColor = Color.White;
        TextAlign = HorizontalAlignment.Center;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (_lbxParent.IsReadOnly)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        else
        {
            base.OnKeyDown(e);
            _lbxParent.RaiseOnKeyDown(e);
        }
    }
    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        if (_lbxParent.IsNumbersOnly)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        base.OnKeyPress(e);
        _lbxParent.RaiseOnKeyPress(e);
    }
    protected override void OnTextChanged(EventArgs e) => _lbxParent.RaiseOnTextChange(e);
}