using System.Diagnostics.CodeAnalysis;
using Autofac;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using TagMe.Avalonia.ViewModels;

namespace TagMe.Avalonia;

/// <summary>
/// Given a view model, returns the corresponding view if possible.
/// </summary>
[RequiresUnreferencedCode(
    "Default implementation of ViewLocator is changed from reflection to DI.",
    Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : IDataTemplate
{
    private readonly IComponentContext _componentContext;

    public ViewLocator(IComponentContext componentContext)
    {
        _componentContext = componentContext;
    }

    public Control Build(object? data)
    {
        return data switch
        {
            //here go the data templates of content controls like so (windows don't qualify):
            //MainViewModel => _componentContext.Resolve<MainWindow>(),
            _ => new TextBlock { Text = $"No view for {data?.GetType().Name ?? "null"}" }
        };
    }

    public bool Match(object? data) => data is ViewModelBase;
}