using TagEditor.Avalonia.Models;
using TagEditor.Avalonia.TagEditing;

namespace TagEditor.Tests.TagEditing;

public class FilenameParserTests
{
    [Fact]
    public void SetFilenameFromTags_ReturnsGoodNewPaths_ForCorrectFormula()
    {
        var testTrack = new Track()
        {
            Artists = ["TestArtist"],
            Title = "TestTitle",
            Genres = ["TestGenre"]
        };
        
        var formula = "%artist% separator %title% (%genre%)";
        var expectedFilename = "TestArtist separator TestTitle (TestGenre)";
        
        var filename = FilenameParser.ParseFilename(testTrack, formula);
        Assert.Equal(expectedFilename, filename);
    }
}