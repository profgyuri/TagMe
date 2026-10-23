using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TagMe.Avalonia.ViewModels;
using TagMe.Avalonia.Views;

namespace TagMe.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var context = DiContainer.GetContext();
        
        DataTemplates.Add(context.Resolve<ViewLocator>());

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = context.Resolve<MainWindow>();
            window.DataContext = context.Resolve<MainViewModel>();
            desktop.MainWindow = window;
        }

        base.OnFrameworkInitializationCompleted();
    }
}