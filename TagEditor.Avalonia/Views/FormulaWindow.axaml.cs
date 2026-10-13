using Avalonia.Controls;
using Avalonia.Interactivity;
using TagEditor.Avalonia.ViewModels;

namespace TagEditor.Avalonia.Views;

public partial class FormulaWindow : Window
{
    public FormulaWindow()
    {
        InitializeComponent();
    }

    private void OkButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not FormulaViewModel vm) return;

        Close(vm.Formula);
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(null);
    }
}