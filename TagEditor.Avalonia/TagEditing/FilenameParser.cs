using System;
using System.Globalization;
using TagEditor.Avalonia.Models;

namespace TagEditor.Avalonia.TagEditing;

public class FilenameParser
{
    public static string ParseFilename(Track track, string formula)
    {
        var sb = new System.Text.StringBuilder();
        var result = new System.Text.StringBuilder();

        foreach (var c in formula)
        {
            sb.Append(c);
            
            if (sb.Length > 1 && sb[0] == '%' && sb[^1] == '%')
            {
                var property = sb.ToString().ToLower() switch
                {
                    "%title%" => track.Title,
                    "%artist%" => track.ArtistsDisplay,
                    "%genre%" => track.GenresDisplay,
                    "%bitrate%" => track.BitRate.ToString(CultureInfo.InvariantCulture),
                    "%samplerate%" => track.SampleRate.ToString(CultureInfo.InvariantCulture),
                    "%bpm%" => track.Bpm.ToString(CultureInfo.InvariantCulture),
                    _ => throw new NotImplementedException(),
                };
                result.Append(property);
            
                sb.Clear();
            }
            else if (sb.Length > 0 && sb[0] != '%')
            {
                if (c == ' ')
                {
                    result.Append(sb);
                    
                    sb.Clear();
                }
            }
        }
        
        return result.ToString().Trim();
    }
}