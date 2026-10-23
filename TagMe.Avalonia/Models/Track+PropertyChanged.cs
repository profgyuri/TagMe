using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;

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
        if (track.Artists.Length != 0)
        {
            tag.Performers = track.Artists;

            if (tag is TagLib.Riff.InfoTag riffInfoTag)
            {
                riffInfoTag.Performers = track.Artists;
            }
        }

        if (!string.IsNullOrWhiteSpace(track.Title))
        {
            tag.Title = track.Title;
        }

        if (track.Genres.Length != 0)
        {
            tag.Genres = track.Genres;
        }

        tag.BeatsPerMinute = (uint)track.Bpm;
    }
    
    private string GetValue(string propertyName)
    {
        return propertyName switch
        {
            nameof(Artists) => ArtistsDisplay,
            nameof(Title) => Title,
            nameof(Genres) => string.Join(", ", Genres),
            nameof(Bpm) => Bpm.ToString(CultureInfo.InvariantCulture),
            nameof(SampleRate) => SampleRate.ToString(CultureInfo.InvariantCulture),
            nameof(BitRate) => BitRate.ToString(CultureInfo.InvariantCulture),
            nameof(FileName) => FileName,
            nameof(Path) => Path,
            _ => ""
        };
    }
}