using Autofac;
using TagEditor.Avalonia.Files;
using TagEditor.Avalonia.TagEditing;
using TagEditor.Avalonia.ViewModels;
using TagEditor.Avalonia.Views;

namespace TagEditor.Avalonia;

public static class DiContainer
{
    private static IComponentContext? _context;
    
    public static IComponentContext GetContext()
    {
        if (_context is not null) return _context;
        
        var builder = new ContainerBuilder();
        
        builder.RegisterType<ViewLocator>().AsSelf().SingleInstance();
        builder.RegisterType<MainWindow>().AsSelf();
        builder.RegisterType<MainViewModel>().AsSelf();
        
        builder.RegisterType<FormulaWindow>().AsSelf();
        builder.RegisterType<FormulaViewModel>().AsSelf();
        
        builder.RegisterType<TrackFetcher>().As<ITrackFetcher>();
        builder.RegisterType<TagFilenameEditor>().As<ITagFilenameEditor>();
        
        return _context = builder.Build();
    }
}