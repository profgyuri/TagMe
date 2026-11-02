using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using TagMe.Avalonia.Models;

namespace TagMe.Avalonia.Files;

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
            .Where(info => SupportedFileTypes.Audio.Patterns!.Any(pattern => 
                FileSystemName.MatchesSimpleExpression(pattern, info.Name, ignoreCase: true)))
            .Select(info => AnalyzeFilePath(info.FullName));
    }

    private Track AnalyzeFilePath(string path)
    {
        var file = TagLib.File.Create(path);
        
        return new Track
        {
            Artists = file.Tag.Performers,
            Title = file.Tag.Title ?? string.Empty,
            Genres = file.Tag.Genres,
            Bpm = file.Tag.BeatsPerMinute,
            SampleRate = file.Properties.AudioSampleRate,
            BitRate = file.Properties.AudioBitrate,
            ArtistsRoles = file.Tag.PerformersRole,
            AlbumArtists = file.Tag.AlbumArtists,
            Composers = file.Tag.Composers,
            Album = file.Tag.Album ?? string.Empty,
            Comment = file.Tag.Comment ?? string.Empty,
            Year = file.Tag.Year,
            TrackNumber = file.Tag.Track,
            DiscNumber = file.Tag.Disc,
            Lyrics = file.Tag.Lyrics ?? string.Empty,
            Grouping = file.Tag.Grouping ?? string.Empty,
            Conductor = file.Tag.Conductor ?? string.Empty,
            Copyright = file.Tag.Copyright ?? string.Empty,
            MusicBrainzArtistId = file.Tag.MusicBrainzArtistId ?? string.Empty,
            MusicBrainzDiscId = file.Tag.MusicBrainzDiscId ?? string.Empty,
            MusicBrainzReleaseArtistId = file.Tag.MusicBrainzReleaseArtistId ?? string.Empty,
            MusicBrainzReleaseCountry = file.Tag.MusicBrainzReleaseCountry ?? string.Empty,
            MusicBrainzReleaseGroupId = file.Tag.MusicBrainzReleaseGroupId ?? string.Empty,
            MusicBrainzReleaseId = file.Tag.MusicBrainzReleaseId ?? string.Empty,
            MusicBrainzReleaseStatus = file.Tag.MusicBrainzReleaseStatus ?? string.Empty,
            MusicBrainzReleaseType = file.Tag.MusicBrainzReleaseType ?? string.Empty,
            MusicBrainzTrackId = file.Tag.MusicBrainzTrackId ?? string.Empty,
            MusicIpId = file.Tag.MusicIpId ?? string.Empty,
            AmazonId = file.Tag.AmazonId ?? string.Empty,
            ReplayGainAlbumGain = file.Tag.ReplayGainAlbumGain,
            ReplayGainAlbumPeak = file.Tag.ReplayGainAlbumPeak,
            ReplayGainTrackGain = file.Tag.ReplayGainTrackGain,
            ReplayGainTrackPeak = file.Tag.ReplayGainTrackPeak,
            InitialKey = file.Tag.InitialKey ?? string.Empty,
            RemixedBy = file.Tag.RemixedBy ?? string.Empty,
            Publisher = file.Tag.Publisher ?? string.Empty,
            ISRC = file.Tag.ISRC ?? string.Empty,
            Length = file.Tag.Length ?? string.Empty,
            Duration = file.Properties.Duration,
            Cover = file.Tag.Pictures.LoadCover(),
            BitsPerSample = file.Properties.BitsPerSample,
            AudioChannels = file.Properties.AudioChannels,
            FileName = new FileInfo(path).Name,
            Path = path,
        };
    }
}