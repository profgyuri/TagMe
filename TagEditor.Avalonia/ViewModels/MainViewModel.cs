using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
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
    
    public void TagsToFilename(string formula)
    {
        _tagFilenameEditor.SetFilenameFromTags(SelectedItems, formula);
    }

    public void FilenameToTags(string formula)
    {
        _tagFilenameEditor.SetTagsFromFilename(SelectedItems, formula);
    }
    
    public async Task MoveFilesAsync(string folder)
    {
        foreach (var track in SelectedItems)
        {
            try
            {
                track.Path = Path.Combine(folder, track.FileName);
            }
            catch (Exception ex)
            {
                var message = MessageBoxManager.GetMessageBoxStandard("Error", $"Couldn't move file: {ex.Message}");
                await message.ShowAsync();
            }
        }
    }

    [RelayCommand]
    private void ClearGrid()
    {
        TrackList.Clear();
    }
}