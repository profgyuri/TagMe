using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagEditor.Avalonia.Models;

namespace TagEditor.Avalonia.Files;

public class TrackFetcher : ITrackFetcher
{
    public IEnumerable<Track> GetFromFilePaths(IEnumerable<string> files)
    {
        return files.Select(AnalyzeFilePath);
    }

    public IEnumerable<Track> GetFromFolderPath(string path)
    {
        return new DirectoryInfo(path)
            .EnumerateFiles()
            .Select(fi => AnalyzeFilePath(fi.FullName));
    }

    private Track AnalyzeFilePath(string path)
    {
        var file = TagLib.File.Create(path);
        return new Track()
        {
            Artists = AsSeparated(file.Tag.Performers),
            BitRate = file.Properties.AudioBitrate,
            Bpm = file.Tag.BeatsPerMinute,
            Genres = AsSeparated(file.Tag.Genres),
            Path = path,
            FileName = new FileInfo(path).Name,
            SampleRate = file.Properties.AudioSampleRate,
            Title = file.Tag.Title,
        };

        string[] AsSeparated(string[] array)
        {
            char[] separators = [';', '/'];
            
            return array
                .SelectMany(p => p.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                .Select(a => a.Trim())
                .Where(a => a.Length > 0)
                .ToArray();
        }
    }
}