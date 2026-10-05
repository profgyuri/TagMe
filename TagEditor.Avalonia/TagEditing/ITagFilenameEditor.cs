using System.Collections.ObjectModel;
using TagEditor.Avalonia.Models;

namespace TagEditor.Avalonia.TagEditing;

/// <summary>
/// Opens up the tag editor window to supply the formula used for getting the tags or the filename
/// </summary>
public interface ITagFilenameEditor
{
    /// <summary>
    /// Using the tags of the track and the formula from the popup window, sets the new filename
    /// </summary>
    /// <param name="tracks">These tracks' files should be renamed</param>
    void SetFilenameFromTags(ObservableCollection<Track> tracks);
    
    /// <summary>
    /// Using the filename of the track and the formula from the popup window, sets the tags for the track
    /// </summary>
    /// <param name="tracks">These tracks' tags should be extracted from their respective filename</param>
    void SetTagsFromFilename(ObservableCollection<Track> tracks);
}