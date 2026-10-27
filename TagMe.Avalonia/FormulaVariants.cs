using System;
using System.Globalization;
using TagMe.Avalonia.Models;

namespace TagMe.Avalonia;

public static class FormulaVariants
{
    public static string GetFormulaVariantForField(string fieldName)
    {
        return $"%{fieldName.ToLowerInvariant()}%";
    }
    
    public static string GetFieldValueForFormula(Track track, string formula)
    {
        return formula.ToLower() switch
        {
            "%artists%" => track.ArtistsDisplay,
            "%title%" => track.Title,
            "%genres%" => track.GenresDisplay,
            "%bitrate%" => track.BitRate.ToString(CultureInfo.InvariantCulture),
            "%samplerate%" => track.SampleRate.ToString(CultureInfo.InvariantCulture),
            "%bpm%" => track.Bpm.ToString(CultureInfo.InvariantCulture),
            "%filename%" => track.FileName,
            "%artistsroles%" => track.ArtistsRoleDisplay,
            "%albumartists%" => track.AlbumArtistsDisplay,
            "%composers%" => track.ComposersDisplay,
            "%album%" => track.Album,
            "%comment%" => track.Comment,
            "%year%" => track.Year.ToString(CultureInfo.InvariantCulture),
            "%discnumber%" => track.DiscNumber.ToString(CultureInfo.InvariantCulture),
            "%lyrics%" => track.Lyrics,
            "%grouping%" => track.Grouping,
            "%conductor%" => track.Conductor,
            "%copyright%" => track.Copyright,
            "%musicbrainzartistid%" => track.MusicBrainzArtistId,
            "%musicbrainzreleasegroupid%" => track.MusicBrainzReleaseGroupId,
            "%musicbrainzreleaseid%" => track.MusicBrainzReleaseId,
            "%musicbrainzreleaseartistid%" => track.MusicBrainzReleaseArtistId,
            "%musicbrainztrackid%" => track.MusicBrainzTrackId,
            "%musicbrainzdiscid%" => track.MusicBrainzDiscId,
            "%musicbrainzreleasestatus%" => track.MusicBrainzReleaseStatus,
            "%musicbrainzreleasetype%" => track.MusicBrainzReleaseType,
            "%musicbrainzreleasecountry%" => track.MusicBrainzReleaseCountry,
            "%musicipid%" => track.MusicIpId,
            "%amazonid%" => track.AmazonId,
            "%replaygaintrackgain%" => track.ReplayGainTrackGain.ToString(CultureInfo.InvariantCulture),
            "%replaygaintrackpeak%" => track.ReplayGainTrackPeak.ToString(CultureInfo.InvariantCulture),
            "%replaygainalbumpeak%" => track.ReplayGainAlbumPeak.ToString(CultureInfo.InvariantCulture),
            "%replaygainalbumgain%" => track.ReplayGainAlbumGain.ToString(CultureInfo.InvariantCulture),
            "%initialkey%" => track.InitialKey,
            "%remixedby%" => track.RemixedBy,
            "%publisher%" => track.Publisher,
            "%isrc%" => track.ISRC,
            "%length%" => track.Length,
            "%bitspersample%" => track.BitsPerSample.ToString(CultureInfo.InvariantCulture),
            "%audiochannels%" => track.AudioChannels.ToString(CultureInfo.InvariantCulture),
            _ => throw new NotSupportedException($"Given formula '{formula}' is invalid"),
        };
    }
}