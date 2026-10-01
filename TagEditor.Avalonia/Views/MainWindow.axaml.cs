using System.Linq;
using Avalonia.Controls;
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
}