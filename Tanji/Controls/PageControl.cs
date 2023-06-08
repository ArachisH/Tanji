using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using Tanji.Core.ViewModels;

using Microsoft.Extensions.DependencyInjection;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Tanji.Controls;

[ToolboxItem(false)]
[DesignerCategory("Code")]
public class PageControl<TObservable> : UserControl where TObservable : ObservableObject, new()
{
    protected virtual BindingSource? Source { get; }
    protected TObservable? Context { get; private set; }

    public PageControl()
    {
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

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (Program.ServiceProvider != null)
        {
            // The parent designer for the custom controls is unable to inject the view model directly into the control, and must be retrieved outside of the constructor.
            DataContext = Program.ServiceProvider.GetRequiredService<TObservable>();
        }
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        if (DataContext is TObservable context)
        {
            Context = context;
            if (Source != null)
            {
                Source.DataSource = context;
            }
        }
        base.OnDataContextChanged(e);
    }
}

// Workaround for generic base types on UserControls/Forms
// Related: https://github.com/dotnet/winforms/issues/3328
public class ConnectionPageControl : PageControl<ConnectionViewModel>;
public class InjectionPageControl : PageControl<InjectionViewModel>;
public class ToolboxPageControl : PageControl<ToolboxViewModel>;
public class ExtensionsPageControl : PageControl<ExtensionsViewModel>;
public class SettingsPageControl : PageControl<SettingsViewModel>;