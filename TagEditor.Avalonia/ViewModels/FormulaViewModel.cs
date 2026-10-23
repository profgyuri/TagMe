using System.Collections.Generic;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using TagEditor.Avalonia.Models;
using TagEditor.Avalonia.TagEditing;

namespace TagEditor.Avalonia.ViewModels;

public partial class FormulaViewModel : ViewModelBase
{
    [ObservableProperty] public partial string Formula { get; set; } = "";
    [ObservableProperty] public partial Dictionary<string, string> ReadTags { get; set; } = new();

    public bool IsTagsToFilename { get; set; }
    public Track? PreviewTrack { get; set; }

    partial void OnFormulaChanged(string value)
    {
        if (PreviewTrack is null)
            return;
        
        if (IsTagsToFilename)
        {
            ReadTags = new()
            {
                ["Filename"] = FilenameParser.ParseFilename(PreviewTrack, Formula) + Path.GetExtension(PreviewTrack.FileName)
            };
            return;
        }
        
        ReadTags = TagParser.ParseTags(PreviewTrack.FileName, Formula) ?? new();
        
        OnPropertyChanged(nameof(ReadTags));
    }
}