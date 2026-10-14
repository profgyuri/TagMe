using System;
using System.Globalization;
using System.Text;
using TagEditor.Avalonia.Models;

namespace TagEditor.Avalonia.TagEditing;

public static class FilenameParser
{
    /// <summary>
    /// Gets the filename from the tags
    /// </summary>
    /// <returns>The filename without the extension</returns>
    public static string ParseFilename(Track track, string formula)
    {
        var pos = 0;
        var result = new StringBuilder();
        var sb = new StringBuilder();

        while (formula.Length > pos)
        {
            var c = formula[pos];
            pos++;

            if (c == '%' && sb.Length > 1 && sb[0] == '%')
            {
                // here comes the mask
                sb.Append(c);
                var mask = sb.ToString();
                sb.Clear();
                
                result.Append(ParseMask(track, mask));
            }
            else if (c == '%')
            {
                // here comes everything before the mask
                result.Append(sb);
                sb.Clear();
                sb.Append(c);
            }
            else
            {
                sb.Append(c);
            }
        }
        
        result.Append(sb);
        
        return result.ToString().Trim();
    }
    
    static string ParseMask(Track track, string mask)
    {
        return mask.ToLower() switch
        {
            "%artist%" => track.ArtistsDisplay,
            "%title%" => track.Title,
            "%genre%" => track.GenresDisplay,
            "%bitrate%" => track.BitRate.ToString(CultureInfo.InvariantCulture),
            "%samplerate%" => track.SampleRate.ToString(CultureInfo.InvariantCulture),
            "%bpm%" => track.Bpm.ToString(CultureInfo.InvariantCulture),
            _ => throw new NotSupportedException(),
        };
    }
}