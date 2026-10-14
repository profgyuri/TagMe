using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using TagEditor.Avalonia.Models;
using TagEditor.Avalonia.ViewModels;

namespace TagEditor.Avalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ColumnHeaderMenu.ItemsSource = Grid.Columns;
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
        if (DataContext is not MainViewModel vm) return null;
        
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
}