using System;
using System.Globalization;
using TagEditor.Avalonia.Models;

namespace TagEditor.Avalonia;

public static class FormulaVariants
{
    public static string GetFormulaVariantForField(string fieldName)
    {
        return fieldName switch
        {
            nameof(Track.Artists) => "%artist%",
            nameof(Track.Genres) => "%genre%",
            nameof(Track.Title) 
                or nameof(Track.Bpm) 
                or nameof(Track.SampleRate) 
                or nameof(Track.FileName) => $"%{fieldName.ToLowerInvariant()}%",
            _ => throw new NotSupportedException($"Given field name '{fieldName}' is invalid"),
        };
    }
    
    public static string GetFieldValueForFormula(Track track, string formula)
    {
        return formula.ToLower() switch
        {
            "%artist%" => track.ArtistsDisplay,
            "%title%" => track.Title,
            "%genre%" => track.GenresDisplay,
            "%bitrate%" => track.BitRate.ToString(CultureInfo.InvariantCulture),
            "%samplerate%" => track.SampleRate.ToString(CultureInfo.InvariantCulture),
            "%bpm%" => track.Bpm.ToString(CultureInfo.InvariantCulture),
            "%filename%" => track.FileName,
            _ => throw new NotSupportedException($"Given formula '{formula}' is invalid"),
        };
    }
}