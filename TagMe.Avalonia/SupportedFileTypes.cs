using Avalonia.Platform.Storage;

namespace TagMe.Avalonia;

public static class SupportedFileTypes
{
    public static readonly FilePickerFileType Audio = new("Audio Files")
    {
        Patterns =
        [
            "*.aa", "*.aax", "*.aac", "*.aiff", "*.ape",
            "*.dsf",
            "*.flac",
            "*.m4a", "*.m4b", "*.m4p", "*.mp3", "*.mpc", "*.mpp",
            "*.ogg", "*.oga",
            "*.wav", "*.wma", "*.wv", "*.webm"
        ],
        MimeTypes = ["audio/*"]
    };
}