using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using TagLib;
using File = System.IO.File;

namespace TagMe.Avalonia.Models;

public partial class Track
{
    private Dictionary<string, string> _oldProperties = new();
    
    protected override void OnPropertyChanging(PropertyChangingEventArgs e)
    {
        _oldProperties[e.PropertyName!] = GetValue(e.PropertyName!);
        _oldProperties[nameof(Path)] = Path;
        
        base.OnPropertyChanging(e);
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (e.PropertyName is null 
            || string.IsNullOrEmpty(Path) || string.IsNullOrEmpty(FileName)
            || _oldProperties[e.PropertyName] == GetValue(e.PropertyName))
        {
            return;
        }
        
        if (e.PropertyName is nameof(Cover) && _pendingCover is null)
        {
            base.OnPropertyChanged(e);
            return;
        }
        
        if (e.PropertyName is nameof(Path) or nameof(FileName))
        {
            try
            {
                var containingDirectory = System.IO.Path.GetDirectoryName(Path)!;
                var newPath = System.IO.Path.Combine(containingDirectory, FileName);
                if (_oldProperties[nameof(Path)] != newPath 
                    && !string.IsNullOrEmpty(_oldProperties[nameof(Path)]) 
                    && !string.IsNullOrEmpty(newPath))
                {
                    if (e.PropertyName is not nameof(Path))
                    {
                        Path = newPath;
                    }
                    else
                    {
                        File.Move(_oldProperties[nameof(Path)], newPath);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Path = _oldProperties[nameof(Path)];
                throw;
            }
            
            base.OnPropertyChanged(e);
            return;
        }
        
        if (e.PropertyName is nameof(FileName) or nameof(Path) || string.IsNullOrEmpty(Path))
            return;
        
        using var file = TagLib.File.Create(Path);
        var extension = System.IO.Path.GetExtension(Path);
        var tags = new List<TagLib.Tag> { file.Tag, file.GetTag(TagLib.TagTypes.Id3v2, create: true) };
        
        if (extension.Equals(".wav", StringComparison.OrdinalIgnoreCase))
        {
            // Removing, then recreating the RIFF tags is necessary to properly write the metadata.
            file.RemoveTags(TagLib.TagTypes.RiffInfo);
        }
        
        if (extension.Equals(".wav", StringComparison.OrdinalIgnoreCase))
        {
            tags.Add(file.GetTag(TagLib.TagTypes.RiffInfo, create: true));
        }
        
        foreach (var tag in tags)
        {
            ApplyTagUpdate(tag, this);
        }

        file.Save();
        
        base.OnPropertyChanged(e);
    }
    
    private void ApplyTagUpdate(TagLib.Tag tag, Track track)
    {
        tag.Performers = track.Artists;

        if (tag is TagLib.Riff.InfoTag riffInfoTag)
        {
            riffInfoTag.Performers = track.Artists;
        }

        tag.Title = track.Title;
        tag.Genres = track.Genres;
        tag.BeatsPerMinute = (uint)track.Bpm;
        tag.PerformersRole = track.ArtistsRoles;
        tag.AlbumArtists = track.AlbumArtists;
        tag.Composers = track.Composers;
        tag.InitialKey = track.InitialKey;
        tag.RemixedBy = track.RemixedBy;
        tag.Publisher = track.Publisher;
        tag.ISRC = track.ISRC;
        tag.Length = track.Length;
        tag.Album = track.Album;
        tag.Comment = track.Comment;
        tag.Year = track.Year;
        tag.Track = track.TrackNumber;
        tag.Disc = track.DiscNumber;
        tag.Lyrics = track.Lyrics;
        tag.Grouping = track.Grouping;
        tag.Conductor = track.Conductor;
        tag.Copyright = track.Copyright;
        tag.MusicBrainzArtistId = track.MusicBrainzArtistId;
        tag.MusicBrainzReleaseGroupId = track.MusicBrainzReleaseGroupId;
        tag.MusicBrainzReleaseId = track.MusicBrainzReleaseId;
        tag.MusicBrainzReleaseArtistId = track.MusicBrainzReleaseArtistId;
        tag.MusicBrainzTrackId = track.MusicBrainzTrackId;
        tag.MusicBrainzDiscId = track.MusicBrainzDiscId;
        tag.MusicIpId = track.MusicIpId;
        tag.AmazonId = track.AmazonId;
        tag.MusicBrainzReleaseStatus = track.MusicBrainzReleaseStatus;
        tag.MusicBrainzReleaseType = track.MusicBrainzReleaseType;
        tag.MusicBrainzReleaseCountry = track.MusicBrainzReleaseCountry;
        tag.ReplayGainTrackGain = track.ReplayGainTrackGain;
        tag.ReplayGainTrackPeak = track.ReplayGainTrackPeak;
        tag.ReplayGainAlbumGain = track.ReplayGainAlbumGain;
        tag.ReplayGainAlbumPeak = track.ReplayGainAlbumPeak;
        tag.Length = track.Length;
        
        // RiffInfoTag doesn't support pictures
        if (_pendingCover is not null && tag is not TagLib.Riff.InfoTag)
        {
            tag.Pictures = new[] { _pendingCover };
        }
    }
    
    private IPicture? _pendingCover;
    
    public void SetCover(IPicture picture, Bitmap thumbnail)
    {
        _pendingCover = picture;
        Cover = thumbnail;
    }
    
    private string GetValue(string propertyName)
    {
        return propertyName switch
        {
            nameof(Artists) or nameof(ArtistsDisplay) => ArtistsDisplay,
            nameof(Title) => Title,
            nameof(Genres) or nameof(GenresDisplay) => string.Join(", ", Genres),
            nameof(Bpm) => Bpm.ToString(CultureInfo.InvariantCulture),
            nameof(SampleRate) => SampleRate.ToString(CultureInfo.InvariantCulture),
            nameof(BitRate) => BitRate.ToString(CultureInfo.InvariantCulture),
            nameof(FileName) => FileName,
            nameof(Path) => Path,
            nameof(Cover) => Cover is null ? "" : RuntimeHelpers.GetHashCode(Cover).ToString(CultureInfo.InvariantCulture),
            nameof(ArtistsRoles) or nameof(ArtistsRoleDisplay) => ArtistsRoleDisplay,
            nameof(AlbumArtists) or nameof(AlbumArtistsDisplay) => AlbumArtistsDisplay,
            nameof(Composers) or nameof(ComposersDisplay) => ComposersDisplay,
            nameof(Album) => Album,
            nameof(Comment) => Comment,
            nameof(Year) => Year.ToString(CultureInfo.InvariantCulture),
            nameof(TrackNumber) => TrackNumber.ToString(CultureInfo.InvariantCulture),
            nameof(DiscNumber) => DiscNumber.ToString(CultureInfo.InvariantCulture),
            nameof(Lyrics) => Lyrics,
            nameof(Grouping) => Grouping,
            nameof(Conductor) => Conductor,
            nameof(Copyright) => Copyright,
            nameof(MusicBrainzArtistId) => MusicBrainzArtistId,
            nameof(MusicBrainzReleaseGroupId) => MusicBrainzReleaseGroupId,
            nameof(MusicBrainzReleaseId) => MusicBrainzReleaseId,
            nameof(MusicBrainzReleaseArtistId) => MusicBrainzReleaseArtistId,
            nameof(MusicBrainzTrackId) => MusicBrainzTrackId,
            nameof(MusicBrainzDiscId) => MusicBrainzDiscId,
            nameof(MusicIpId) => MusicIpId,
            nameof(AmazonId) => AmazonId,
            nameof(MusicBrainzReleaseStatus) => MusicBrainzReleaseStatus,
            nameof(MusicBrainzReleaseType) => MusicBrainzReleaseType,
            nameof(MusicBrainzReleaseCountry) => MusicBrainzReleaseCountry,
            nameof(ReplayGainTrackGain) => ReplayGainTrackGain.ToString(CultureInfo.InvariantCulture),
            nameof(ReplayGainTrackPeak) => ReplayGainTrackPeak.ToString(CultureInfo.InvariantCulture),
            nameof(ReplayGainAlbumGain) => ReplayGainAlbumGain.ToString(CultureInfo.InvariantCulture),
            nameof(ReplayGainAlbumPeak) => ReplayGainAlbumPeak.ToString(CultureInfo.InvariantCulture),
            nameof(InitialKey) => InitialKey,
            nameof(RemixedBy) => RemixedBy,
            nameof(Publisher) => Publisher,
            nameof(ISRC) => ISRC,
            nameof(Length) => Length,
            _ => ""
        };
    }
}