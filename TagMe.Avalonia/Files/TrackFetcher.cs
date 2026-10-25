using System.Collections.Generic;
using System.IO;
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
            .Select(info => AnalyzeFilePath(info.FullName));
    }

    private Track AnalyzeFilePath(string path)
    {
        var file = TagLib.File.Create(path);
        
        return new Track
        {
            Artists = file.Tag.Performers,
            Title = file.Tag.Title,
            Genres = file.Tag.Genres,
            Bpm = file.Tag.BeatsPerMinute,
            SampleRate = file.Properties.AudioSampleRate,
            BitRate = file.Properties.AudioBitrate,
            ArtistsRoles = file.Tag.PerformersRole,
            AlbumArtists = file.Tag.AlbumArtists,
            Composers = file.Tag.Composers,
            Album = file.Tag.Album,
            Comment = file.Tag.Comment,
            Year = file.Tag.Year,
            TrackNumber = file.Tag.Track,
            DiscNumber = file.Tag.Disc,
            Lyrics = file.Tag.Lyrics,
            Grouping = file.Tag.Grouping,
            Conductor = file.Tag.Conductor,
            Copyright = file.Tag.Copyright,
            MusicBrainzArtistId = file.Tag.MusicBrainzArtistId,
            MusicBrainzDiscId = file.Tag.MusicBrainzDiscId,
            MusicBrainzReleaseArtistId = file.Tag.MusicBrainzReleaseArtistId,
            MusicBrainzReleaseCountry = file.Tag.MusicBrainzReleaseCountry,
            MusicBrainzReleaseGroupId = file.Tag.MusicBrainzReleaseGroupId,
            MusicBrainzReleaseId = file.Tag.MusicBrainzReleaseId,
            MusicBrainzReleaseStatus = file.Tag.MusicBrainzReleaseStatus,
            MusicBrainzReleaseType = file.Tag.MusicBrainzReleaseType,
            MusicBrainzTrackId = file.Tag.MusicBrainzTrackId,
            MusicIpId = file.Tag.MusicIpId,
            AmazonId = file.Tag.AmazonId,
            ReplayGainAlbumGain = file.Tag.ReplayGainAlbumGain,
            ReplayGainAlbumPeak = file.Tag.ReplayGainAlbumPeak,
            ReplayGainTrackGain = file.Tag.ReplayGainTrackGain,
            ReplayGainTrackPeak = file.Tag.ReplayGainTrackPeak,
            InitialKey = file.Tag.InitialKey,
            RemixedBy = file.Tag.RemixedBy,
            Publisher = file.Tag.Publisher,
            ISRC = file.Tag.ISRC,
            Length = file.Tag.Length,
            Duration = file.Properties.Duration,
            Cover = file.Tag.Pictures.LoadCover(),
            BitsPerSample = file.Properties.BitsPerSample,
            AudioChannels = file.Properties.AudioChannels,
            FileName = new FileInfo(path).Name,
            Path = path,
        };
    }
}