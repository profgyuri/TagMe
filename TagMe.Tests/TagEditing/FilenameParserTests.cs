using TagMe.Avalonia.Models;
using TagMe.Avalonia.TagEditing;

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
        
        var formula = "%Artist% separator %tiTle% (%genrE%)"; // casing shouldn't matter
        var expectedFilename = "TestArtist separator TestTitle (TestGenre)";
        
        var filename = FilenameParser.ParseFilename(testTrack, formula);
        Assert.Equal(expectedFilename, filename);
    }

    [Fact]
    public void SetFilenameFromTags_Throws_WhenFormulaContainsBadMaskString()
    {
        var testTrack = new Track()
        {
            Artists = ["TestArtist"],
            Title = "TestTitle",
            Genres = ["TestGenre"]
        };
        
        var formula = "%notexistingmask% separator %title% (%genre%)";
        
        Assert.Throws<NotSupportedException>(() => FilenameParser.ParseFilename(testTrack, formula));
    }
}