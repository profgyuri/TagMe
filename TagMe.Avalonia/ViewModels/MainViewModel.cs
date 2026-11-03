using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using TagMe.Avalonia.Files;
using TagMe.Avalonia.Models;
using TagMe.Avalonia.TagEditing;

namespace TagMe.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ITrackFetcher  _trackFetcher = null!;
    private readonly ITagFilenameEditor  _tagFilenameEditor = null!;

    public MainViewModel()
    { }

    public MainViewModel(ITrackFetcher trackFetcher, ITagFilenameEditor tagFilenameEditor)
    {
        _trackFetcher = trackFetcher;
        _tagFilenameEditor = tagFilenameEditor;
    }

    private List<Track> _allTracks = new();
    
    [ObservableProperty] public partial ObservableCollection<Track> TrackList { get; set; } = new();

    [ObservableProperty] public partial ObservableCollection<Track> SelectedItems { get; set; } = new();
    
    [ObservableProperty] public partial string FilterText { get; set; } = string.Empty;
    
    public void AddFiles(IEnumerable<string> files)
    {
        FilterText = string.Empty;
        var tracks = _trackFetcher.GetFromFilePaths(files);
        foreach (var track in tracks)
        {
            _allTracks.Add(track);
        }

        TrackList = new(_allTracks);
    }

    public void AddFolder(string folder)
    {
        FilterText = string.Empty;
        var  tracks = _trackFetcher.GetFromFolderPath(folder);
        foreach (var track in tracks)
        {
            _allTracks.Add(track);
        }
        
        TrackList = new(_allTracks);
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
        FilterText = string.Empty;
        _allTracks.Clear();
        TrackList.Clear();
    }

    private CancellationTokenSource _filterDebounceToken = new();
    
    async partial void OnFilterTextChanged(string value)
    {
        await _filterDebounceToken?.CancelAsync()!;
        _filterDebounceToken?.Dispose();
        _filterDebounceToken = new();
        
        try
        {
            await Task.Delay(300, _filterDebounceToken.Token);
            var filtered = string.IsNullOrEmpty(value)
                ? _allTracks
                : _allTracks.FilterByValue(value);
            TrackList = new(filtered);
        }
        catch (OperationCanceledException)
        { }
    }
}