using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TagEditor.Avalonia.ViewModels;

public partial class FormulaViewModel : ViewModelBase
{
    [ObservableProperty] public partial string Formula { get; set; } = "";
    [ObservableProperty] public partial Dictionary<string, string> ReadTags { get; set; } = new();
}