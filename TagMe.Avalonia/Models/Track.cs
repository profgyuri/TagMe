using System;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using TagLib;

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
        set => Genres = value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
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
    [ObservableProperty] 
    public partial string[] ArtistsRoles { get; set; } = [];
    public string ArtistsRoleDisplay
    {
        get => string.Join("; ", ArtistsRoles);
        set => ArtistsRoles = value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
    [ObservableProperty] 
    public partial string[] AlbumArtists { get; set; } = [];
    public string AlbumArtistsDisplay
    {
        get => string.Join("; ", AlbumArtists);
        set => AlbumArtists = value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
    [ObservableProperty] 
    public partial string[] Composers { get; set; } = [];
    public string ComposersDisplay
    {
        get => string.Join("; ", Composers);
        set => Composers = value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
    [ObservableProperty] 
    public partial string Album { get; set; }
    [ObservableProperty] 
    public partial string Comment { get; set; }
    [ObservableProperty] 
    public partial uint Year { get; set; }
    [ObservableProperty] 
    public partial uint TrackNumber { get; set; }
    [ObservableProperty] 
    public partial uint DiscNumber { get; set; }
    [ObservableProperty] 
    public partial string Lyrics { get; set; }
    [ObservableProperty] 
    public partial string Grouping { get; set; }
    [ObservableProperty] 
    public partial string Conductor { get; set; }
    [ObservableProperty] 
    public partial string Copyright { get; set; }
    [ObservableProperty] 
    public partial string MusicBrainzArtistId { get; set; }
    [ObservableProperty] 
    public partial string MusicBrainzReleaseGroupId { get; set; }
    [ObservableProperty] 
    public partial string MusicBrainzReleaseId { get; set; }
    [ObservableProperty] 
    public partial string MusicBrainzReleaseArtistId { get; set; }
    [ObservableProperty] 
    public partial string MusicBrainzTrackId { get; set; }
    [ObservableProperty] 
    public partial string MusicBrainzDiscId { get; set; }
    [ObservableProperty] 
    public partial string MusicIpId { get; set; }
    [ObservableProperty] 
    public partial string AmazonId { get; set; }
    [ObservableProperty] 
    public partial string MusicBrainzReleaseStatus { get; set; }
    [ObservableProperty] 
    public partial string MusicBrainzReleaseType { get; set; }
    [ObservableProperty] 
    public partial string MusicBrainzReleaseCountry { get; set; }
    [ObservableProperty] 
    public partial double ReplayGainTrackGain { get; set; }
    [ObservableProperty] 
    public partial double ReplayGainTrackPeak { get; set; }
    [ObservableProperty] 
    public partial double ReplayGainAlbumGain { get; set; }
    [ObservableProperty] 
    public partial double ReplayGainAlbumPeak { get; set; }
    [ObservableProperty] 
    public partial string InitialKey { get; set; }
    [ObservableProperty] 
    public partial string RemixedBy { get; set; }
    [ObservableProperty] 
    public partial string Publisher { get; set; }
    [ObservableProperty] 
    public partial string ISRC { get; set; }
    [ObservableProperty] 
    public partial string Length { get; set; }
    [ObservableProperty] 
    public partial Bitmap? Cover { get; set; }
    [ObservableProperty] 
    public partial TimeSpan Duration { get; set; }
    [ObservableProperty] 
    public partial int BitsPerSample { get; set; }
    [ObservableProperty] 
    public partial int AudioChannels { get; set; }
}