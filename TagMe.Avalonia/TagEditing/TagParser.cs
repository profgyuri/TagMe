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
    private static readonly HashSet<string> ParsableMasks =
        new(StringComparer.OrdinalIgnoreCase) { "artist", "title", "genre", "bpm" };

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

        if (tags.TryGetValue("artist", out var artist) && artist.Length > 0)
            track.ArtistsDisplay = artist;

        if (tags.TryGetValue("title", out var title) && title.Length > 0)
            track.Title = title;

        if (tags.TryGetValue("genre", out var genre) && genre.Length > 0)
            track.GenresDisplay = genre;

        if (tags.TryGetValue("bpm", out var bpm)
            && double.TryParse(bpm, NumberStyles.Float, CultureInfo.InvariantCulture, out var bpmValue))
            track.Bpm = bpmValue;

        return true;
    }
}