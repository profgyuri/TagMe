using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TagMe.Avalonia;

public static class Settings
{
    private static readonly string GridLayoutPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "tagme",
        "grid-layout.json");
    
    public static void SaveColumnVisibility(Dictionary<string, bool> visibleColumns)
    {
        CheckGridLayoutFileExists();
        
        File.WriteAllText(GridLayoutPath, JsonSerializer.Serialize(visibleColumns, AppJsonContext.Default.DictionaryStringBoolean));
    }
    
    public static Dictionary<string, bool> LoadColumnVisibility()
    {
        CheckGridLayoutFileExists();

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, bool>>(File.ReadAllText(GridLayoutPath),
                AppJsonContext.Default.DictionaryStringBoolean) ?? new();
        }
        catch
        {
            return new();
        }
    }
    
    private static void CheckGridLayoutFileExists()
    {
        var dir = new FileInfo(GridLayoutPath).Directory!;
        if (!dir.Exists)
            Directory.CreateDirectory(dir.FullName);
        
        if (!File.Exists(GridLayoutPath))
            File.Create(GridLayoutPath).Dispose();
    }
}

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(Dictionary<string, bool>))]
internal partial class AppJsonContext : JsonSerializerContext
{
}