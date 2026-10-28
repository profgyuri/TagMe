using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using TagLib;
using TagMe.Avalonia.Models;
using TagMe.Avalonia.ViewModels;

namespace TagMe.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += (sender, args) =>
        {
            Settings.LoadColumnVisibility().ToList()
                .ForEach(c => 
                    Grid.Columns.First(x => x.Header.ToString() == c.Key).IsVisible = c.Value);
        };
        ColumnHeaderMenu.ItemsSource = Grid.Columns;
        ColumnHeaderMenu.Closed += ColumnHeaderMenuOnClosed;
    }

    private void ColumnHeaderMenuOnClosed(object? sender, RoutedEventArgs e)
    {
        Settings.SaveColumnVisibility(Grid.Columns.ToDictionary(c => c.Header.ToString()!, c => c.IsVisible));
    }

    private void Grid_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not MainViewModel vm) return;
        if (e.AddedItems.Count > 0)
        {
            var items = e.AddedItems.Cast<Track>();
            foreach (var item in items)
                vm.SelectedItems.Add(item);
        }
        
        if (e.RemovedItems.Count > 0)
        {
            var items = e.RemovedItems.Cast<Track>();
            foreach (var item in items)
                vm.SelectedItems.Remove(item);
        }
    }

    private async void AddFiles_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = GetTopLevel(this);
        if (topLevel is null) return;
        if (DataContext is not MainViewModel vm) return;
        
        var dialogResult = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Choose Music Files",
            AllowMultiple = true,
            FileTypeFilter = [SupportedFileTypes.Audio]
        });
        
        if (dialogResult.Count == 0) return;
        
        vm.AddFiles(dialogResult.Select(file => file.Path.LocalPath));
    }

    private async void AddFolder_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = GetTopLevel(this);
        if (topLevel is null) return;
        if (DataContext is not MainViewModel vm) return;

        var dialogResult = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Choose a Music Folder",
            AllowMultiple = false
        });
        
        if (dialogResult.Count == 0) return;
        
        vm.AddFolder(dialogResult[0].Path.LocalPath);
    }
    
    private async void TagsToFilename_OnClick(object? sender, RoutedEventArgs e)
    {
        if (await GetFormula(isTagsToFilename: true) is not string formula) return;
        if (DataContext is not MainViewModel vm) return;
        
        vm.TagsToFilename(formula);
    }

    private async void FilenameToTags_OnClick(object? sender, RoutedEventArgs e)
    {
        if (await GetFormula(isTagsToFilename: false) is not string formula) return;
        if (DataContext is not MainViewModel vm) return;
        
        vm.FilenameToTags(formula);
    }
    
    private async Task<string?> GetFormula(bool isTagsToFilename)
    {
        if (DataContext is not MainViewModel {SelectedItems.Count: > 0 } vm) return null;
        
        var context = DiContainer.GetContext();
        var formulaWindow = context.Resolve<FormulaWindow>();
        formulaWindow.DataContext = context.Resolve<FormulaViewModel>();

        var formulaWindowDataContext = (FormulaViewModel)formulaWindow.DataContext;
        formulaWindowDataContext.IsTagsToFilename = isTagsToFilename;
        formulaWindowDataContext.PreviewTrack = vm.SelectedItems[0];
        
        return await formulaWindow.ShowDialog<string?>(this);
    }

    private async void MoveFiles_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = GetTopLevel(this);
        if (topLevel is null) return;
        if (DataContext is not MainViewModel vm) return;
        
        var dialogResult = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Choose a destination folder",
            AllowMultiple = false
        });
        
        if (dialogResult.Count == 0) return;
        
        await vm.MoveFilesAsync(dialogResult[0].Path.LocalPath);
    }

    private async void ChangeCoverButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Control { DataContext: Track row })
            return;

        var topLevel = GetTopLevel(this);
        if (topLevel is null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select cover image",
            AllowMultiple = false,
            FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
        });

        var path = files.Count > 0 ? files[0].TryGetLocalPath() : null;
        if (path is null) return;

        var picture = new Picture(path) { Type = PictureType.FrontCover };

        using var stream = new MemoryStream(picture.Data.Data);
        row.SetCover(picture, Bitmap.DecodeToWidth(stream, 24));
    }
}