using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using TagEditor.Avalonia.Models;

namespace TagEditor.Avalonia.TagEditing;

public class TagFilenameEditor : ITagFilenameEditor
{
    public IEnumerable<Track> SetFilenameFromTags(ObservableCollection<Track> tracks, string formula)
    {
        foreach (var track in tracks)
        {
            var filename = FilenameParser.ParseFilename(track, formula);
            track.FileName = filename;
        }
        
        return tracks;
    }

    public IEnumerable<Track> SetTagsFromFilename(ObservableCollection<Track> tracks, string formula)
    {
        throw new System.NotImplementedException();
    }
}