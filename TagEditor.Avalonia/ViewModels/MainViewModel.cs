using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TagEditor.Avalonia.Files;
using TagEditor.Avalonia.Models;
using TagEditor.Avalonia.TagEditing;

namespace TagEditor.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ITrackFetcher  _trackFetcher;
    private readonly ITagFilenameEditor  _tagFilenameEditor;

    public MainViewModel()
    { }

    public MainViewModel(ITrackFetcher trackFetcher, ITagFilenameEditor tagFilenameEditor)
    {
        _trackFetcher = trackFetcher;
        _tagFilenameEditor = tagFilenameEditor;
    }

    [ObservableProperty] public partial ObservableCollection<Track> TrackList { get; set; } = new();

    [ObservableProperty] public partial ObservableCollection<Track> SelectedItems { get; set; } = new();
    
    public void AddFiles(IEnumerable<string> files)
    {
        var tracks = _trackFetcher.GetFromFilePaths(files);
        foreach (var track in tracks)
        {
            TrackList.Add(track);
        }
    }

    public void AddFolder(string folder)
    {
        var  tracks = _trackFetcher.GetFromFolderPath(folder);
        foreach (var track in tracks)
        {
            TrackList.Add(track);
        }
    }

    [RelayCommand]
    private void ClearGrid()
    {
        TrackList.Clear();
    }

    [RelayCommand]
    private void TagsToFilename()
    {
        _tagFilenameEditor.SetFilenameFromTags(SelectedItems);
    }

    [RelayCommand]
    private void FilenameToTags()
    {
        _tagFilenameEditor.SetTagsFromFilename(SelectedItems);
    }
}