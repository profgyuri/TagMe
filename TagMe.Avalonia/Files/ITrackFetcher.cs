using System.Collections.Generic;
using TagMe.Avalonia.Models;

namespace TagMe.Avalonia.Files;

/// <summary>
/// Contract to retrieve tracks from the file system.
/// </summary>
public interface ITrackFetcher
{
    /// <summary>
    /// Retrieve tracks using the file browser results
    /// </summary>
    /// <param name="files">The selected files that should be analyzed</param>
    IEnumerable<Track> GetFromFilePaths(IEnumerable<string> files);
    
    /// <summary>
    /// Retrieve tracks using the folder browser results
    /// </summary>
    /// <param name="path">The folder that contains the music files</param>
    IEnumerable<Track> GetFromFolderPath(string path);
}