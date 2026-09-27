using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TagEditor.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] public partial string Greeting { get; set; } = "Welcome to Avalonia!";

    [ObservableProperty] public partial ObservableCollection<TestItem> TestStrings { get; set; } = new()
    {
        new("Line 1"),
        new("Line 2"),
        new("Line 3"),
        new("Line 4"),
    };
}

public partial class TestItem : ObservableObject
{
    [ObservableProperty] public partial string Value { get; set; }

    public TestItem(string value) => Value = value;
}