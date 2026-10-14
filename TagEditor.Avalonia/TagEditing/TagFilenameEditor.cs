using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using TagEditor.Avalonia.Models;

namespace TagEditor.Avalonia.TagEditing;

public class TagFilenameEditor : ITagFilenameEditor
{
    public IEnumerable<Track> SetFilenameFromTags(ObservableCollection<Track> tracks, string formula)
    {
        foreach (var track in tracks)
        {
            var filename = FilenameParser.ParseFilename(track, formula);
            track.FileName = filename + Path.GetExtension(track.FileName);
        }
        
        return tracks;
    }

    public IEnumerable<Track> SetTagsFromFilename(ObservableCollection<Track> tracks, string formula)
    {
        foreach (var track in tracks)
        {
            TagParser.ApplyTagsFromFilename(track, formula);
        }
        
        return tracks;
    }
}