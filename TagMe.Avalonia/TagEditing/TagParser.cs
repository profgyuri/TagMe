using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TagMe.Avalonia.Models;

namespace TagMe.Avalonia.TagEditing;

public static class TagParser
{
    private static readonly Regex MaskRegex = new(@"%(\w+)%", RegexOptions.Compiled);

    // lining up only writeable masks
    private static readonly HashSet<string> ParsableMasks = new(StringComparer.OrdinalIgnoreCase)
    {
        "artists", "title", "genres", "bpm", "filename", "artistsroles", "albumartists", "composers", "album",
        "comment", "year", "tracknumber", "discnumber", "lyrics", "grouping", "conductor", "copyright",
        "replaygainalbumgain", "replaygaintrackgain", "replaygainalbumpeak", "replaygaintrackpeak",
        "musicbrainzartistid", "musicbrainztrackid", "musicbrainzdiscid", "musicbrainzreleaseartistid", 
        "musicbrainzreleasegroupid", "musicbrainzreleasecountry", "musicbrainzreleaseid", "musicbrainzreleasestatus", 
        "musicbrainzreleasetype", "musicipid", "amazonid",
    };

    /// <summary>
    /// Gets the tags from the filename
    /// </summary>
    /// <returns>A dictionary of mask names (lowercase, without %) to values, or null if the cannot be parsed with the formula</returns>
    public static Dictionary<string, string>? ParseTags(string filename, string formula)
    {
        var name = Path.GetFileNameWithoutExtension(filename);
        formula = formula.Trim();

        var pattern = new StringBuilder("^");
        var pos = 0;

        foreach (var m in MaskRegex.Matches(formula).Cast<Match>())
        {
            var mask = m.Groups[1].Value.ToLowerInvariant();
            if (!ParsableMasks.Contains(mask))
                return null;

            pattern.Append(Regex.Escape(formula[pos..m.Index]));   // literal text before the mask
            pattern.Append($"(?<{mask}>.*?)");
            pos = m.Index + m.Length;
        }

        pattern.Append(Regex.Escape(formula[pos..])).Append('$');

        var match = Regex.Match(name, pattern.ToString(), RegexOptions.CultureInvariant);
        if (!match.Success)
            return null;

        return MaskRegex.Matches(formula)
            .Select(m => m.Groups[1].Value.ToLowerInvariant())
            .Distinct()
            .ToDictionary(mask => mask, mask => match.Groups[mask].Value.Trim());
    }
    
    /// <summary>
    /// Reads the tags from the track's filename using the formula and writes them to the track.
    /// </summary>
    /// <returns>False if the filename doesn't match the formula (the track is left untouched)</returns>
    public static bool ApplyTagsFromFilename(Track track, string formula)
    {
        var tags = ParseTags(track.FileName, formula);
        if (tags is null)
            return false;

        if (tags.TryGetValue("artists", out var artist) && artist.Length > 0)
            track.ArtistsDisplay = artist;

        if (tags.TryGetValue("title", out var title) && title.Length > 0)
            track.Title = title;

        if (tags.TryGetValue("genres", out var genre) && genre.Length > 0)
            track.GenresDisplay = genre;

        if (tags.TryGetValue("bpm", out var bpm)
            && double.TryParse(bpm, NumberStyles.Float, CultureInfo.InvariantCulture, out var bpmValue))
            track.Bpm = bpmValue;

        if (tags.TryGetValue("filename", out var filename) && filename.Length > 0)
            track.FileName = filename;
        
        if (tags.TryGetValue("length", out var length) && length.Length > 0)
            track.Length = length;
        
        if (tags.TryGetValue("artistsroles", out var artistRoles) && artistRoles.Length > 0)
            track.ArtistsRoleDisplay = artistRoles;
        
        if (tags.TryGetValue("albumartists", out var albumArtists) && albumArtists.Length > 0)
            track.AlbumArtistsDisplay = albumArtists;
        
        if (tags.TryGetValue("composers", out var composers) && composers.Length > 0)
            track.ComposersDisplay = composers;
        
        if (tags.TryGetValue("album", out var album) && album.Length > 0)
            track.Album = album;
        
        if (tags.TryGetValue("comment", out var comment) && comment.Length > 0)
            track.Comment = comment;
        
        if (tags.TryGetValue("year", out var year) 
            && uint.TryParse(year, NumberStyles.Integer, CultureInfo.InvariantCulture, out var yearValue))
            track.Year = yearValue;
        
        if (tags.TryGetValue("tracknumber", out var trackNo) 
            && uint.TryParse(trackNo, NumberStyles.Integer, CultureInfo.InvariantCulture, out var trackNoValue))
            track.TrackNumber = trackNoValue;
        
        if (tags.TryGetValue("discnumber", out var discNo) 
            && uint.TryParse(discNo, NumberStyles.Integer, CultureInfo.InvariantCulture, out var discNoValue))
            track.DiscNumber = discNoValue;
        
        if (tags.TryGetValue("lyrics", out var lyrics) && lyrics.Length > 0)
            track.Lyrics = lyrics;
        
        if (tags.TryGetValue("grouping", out var grouping) && grouping.Length > 0)
            track.Grouping = grouping;
        
        if (tags.TryGetValue("conductor", out var conductor) && conductor.Length > 0)
            track.Conductor = conductor;
        
        if (tags.TryGetValue("copyright", out var copyright) && copyright.Length > 0)
            track.Copyright = copyright;
        
        if (tags.TryGetValue("replaygainalbumgain", out var albumGain)
            && double.TryParse(albumGain, NumberStyles.Float, CultureInfo.InvariantCulture, out var albumGainValue))
            track.ReplayGainAlbumGain = albumGainValue;

        if (tags.TryGetValue("replaygaintrackgain", out var trackGain)
            && double.TryParse(trackGain, NumberStyles.Float, CultureInfo.InvariantCulture, out var trackGainValue))
            track.ReplayGainTrackGain = trackGainValue;

        if (tags.TryGetValue("replaygainalbumpeak", out var albumPeak)
            && double.TryParse(albumPeak, NumberStyles.Float, CultureInfo.InvariantCulture, out var albumPeakValue))
            track.ReplayGainAlbumPeak = albumPeakValue;

        if (tags.TryGetValue("replaygaintrackpeak", out var trackPeak)
            && double.TryParse(trackPeak, NumberStyles.Float, CultureInfo.InvariantCulture, out var trackPeakValue))
            track.ReplayGainTrackPeak = trackPeakValue;

        if (tags.TryGetValue("musicbrainzartistid", out var mbArtistId) && mbArtistId.Length > 0)
            track.MusicBrainzArtistId = mbArtistId;

        if (tags.TryGetValue("musicbrainztrackid", out var mbTrackId) && mbTrackId.Length > 0)
            track.MusicBrainzTrackId = mbTrackId;

        if (tags.TryGetValue("musicbrainzdiscid", out var mbDiscId) && mbDiscId.Length > 0)
            track.MusicBrainzDiscId = mbDiscId;

        if (tags.TryGetValue("musicbrainzreleaseartistid", out var mbReleaseArtistId) && mbReleaseArtistId.Length > 0)
            track.MusicBrainzReleaseArtistId = mbReleaseArtistId;

        if (tags.TryGetValue("musicbrainzreleasegroupid", out var mbReleaseGroupId) && mbReleaseGroupId.Length > 0)
            track.MusicBrainzReleaseGroupId = mbReleaseGroupId;

        if (tags.TryGetValue("musicbrainzreleasecountry", out var mbReleaseCountry) && mbReleaseCountry.Length > 0)
            track.MusicBrainzReleaseCountry = mbReleaseCountry;

        if (tags.TryGetValue("musicbrainzreleaseid", out var mbReleaseId) && mbReleaseId.Length > 0)
            track.MusicBrainzReleaseId = mbReleaseId;

        if (tags.TryGetValue("musicbrainzreleasestatus", out var mbReleaseStatus) && mbReleaseStatus.Length > 0)
            track.MusicBrainzReleaseStatus = mbReleaseStatus;

        if (tags.TryGetValue("musicbrainzreleasetype", out var mbReleaseType) && mbReleaseType.Length > 0)
            track.MusicBrainzReleaseType = mbReleaseType;

        if (tags.TryGetValue("musicipid", out var musicIpId) && musicIpId.Length > 0)
            track.MusicIpId = musicIpId;

        if (tags.TryGetValue("amazonid", out var amazonId) && amazonId.Length > 0)
            track.AmazonId = amazonId;

        return true;
    }
}