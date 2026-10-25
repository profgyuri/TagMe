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
            _ => throw new NotSupportedException($"Given formula '{formula}' is invalid"),
        };
    }
}