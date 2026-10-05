using System.Collections.Generic;
using TagEditor.Avalonia.Models;

namespace TagEditor.Avalonia.Files;

public class TrackFetcher : ITrackFetcher
{
    public IEnumerable<Track> GetFromFilePaths(IEnumerable<string> files)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Track> GetFromFolderPath(string path)
    {
        throw new System.NotImplementedException();
    }
}