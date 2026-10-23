using Autofac;
using TagMe.Avalonia.Files;
using TagMe.Avalonia.TagEditing;
using TagMe.Avalonia.ViewModels;
using TagMe.Avalonia.Views;

namespace TagMe.Avalonia;

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