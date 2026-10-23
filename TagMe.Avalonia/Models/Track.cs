using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TagMe.Avalonia.Models;

public partial class Track : ObservableObject
{
    [ObservableProperty, NotifyPropertyChangedFor(nameof(ArtistsDisplay))] 
    public partial string[] Artists { get; set; } = [];
    public string ArtistsDisplay 
    {
        get => string.Join("; ", Artists);
        set => Artists = value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
    
    [ObservableProperty] 
    public partial string Title { get; set; }
    [ObservableProperty, NotifyPropertyChangedFor(nameof(GenresDisplay))] 
    public partial string[] Genres { get; set; } = [];
    public string GenresDisplay
    {
        get => string.Join("; ", Genres);
        set
        {
            Genres = value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            Console.WriteLine(string.Join("; ", Genres));
        }
    }

    [ObservableProperty] 
    public partial double Bpm { get; set; }
    [ObservableProperty] 
    public partial double SampleRate { get; set; }
    [ObservableProperty] 
    public partial double BitRate { get; set; }
    [ObservableProperty] 
    public partial string Path { get; set; }
    [ObservableProperty] 
    public partial string FileName { get; set; }
}