using CommunityToolkit.Mvvm.ComponentModel;

namespace TagEditor.Avalonia.Models;

public partial class Track : ObservableObject
{
    [ObservableProperty] public partial string Artist { get; set; }
    [ObservableProperty] public partial string Title { get; set; }
    [ObservableProperty] public partial string Genre { get; set; }
    [ObservableProperty] public partial double Bpm { get; set; }
    [ObservableProperty] public partial double SampleRate { get; set; }
    [ObservableProperty] public partial double BitRate { get; set; }
    [ObservableProperty] public partial string Path { get; set; }
}