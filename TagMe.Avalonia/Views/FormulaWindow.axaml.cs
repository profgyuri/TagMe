using Avalonia.Controls;
using Avalonia.Interactivity;
using TagMe.Avalonia.ViewModels;

namespace TagMe.Avalonia.Views;

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

    private void ContextMenuItem_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem item) return;
        
        var caretPosition = FormulaTextBox.CaretIndex;
        var oldText = FormulaTextBox.Text;
        FormulaTextBox.Text = oldText?.Insert(caretPosition, FormulaVariants.GetFormulaVariantForField((string)item.Header!));
    }
}