using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TagEditor.Avalonia.Models;

namespace TagEditor.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] public partial ObservableCollection<Track> TrackList { get; set; } = new()
    {
        new()
        {
            Artist = "Artist 1",
            Title = "Title 1",
            Genre = "Genre 1",
            Bpm = 128,
            SampleRate = 44100,
            BitRate = 1411,
            Path = "/mnt/hdd/Music/Sample 1.wav"
        },
        new()
        {
            Artist = "Artist 2",
            Title = "Title 2",
            Genre = "Genre 2",
            Bpm = 150,
            SampleRate = 44100,
            BitRate = 320,
            Path = "/mnt/hdd/Music/Sample 2.mp3"
        },
        new()
        {
            Artist = "Artist 3",
            Title = "Title 3",
            Genre = "Genre 3",
            Bpm = 115,
            SampleRate = 44100,
            BitRate = 2117,
            Path = "/mnt/hdd/Music/Sample 3.wav"
        },
    };

    [ObservableProperty] public partial ObservableCollection<Track> SelectedItems { get; set; } = new();
    
    #region Commands

    [RelayCommand]
    private void AddFiles()
    {
        
    }

    [RelayCommand]
    private void AddFolder()
    {
        
    }

    [RelayCommand]
    private void ClearGrid()
    {
        
    }

    [RelayCommand]
    private void TagsToFilename()
    {
        Console.WriteLine($"Tags to filename {SelectedItems.Count}");
    }

    [RelayCommand]
    private void FilenameToTags()
    {
        Console.WriteLine($"Filename to filename {SelectedItems.Count}");
    }

    #endregion
}