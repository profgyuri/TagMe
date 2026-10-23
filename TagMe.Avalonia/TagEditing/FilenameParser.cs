using System.Text;
using TagMe.Avalonia.Models;

namespace TagMe.Avalonia.TagEditing;

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
                
                result.Append(FormulaVariants.GetFieldValueForFormula(track, mask));
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
}