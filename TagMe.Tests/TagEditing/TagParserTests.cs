using TagMe.Avalonia.Models;
using TagMe.Avalonia.TagEditing;

namespace TagEditor.Tests.TagEditing;

public class TagParserTests
{
    [Fact]
    public void ParseTags_DoesNothing_WhenFormulaIsInvalid()
    {
        var testTrack = new Track()
        {
            Artists = ["TestArtist"],
            Title = "TestTitle",
            Genres = ["TestGenre"],
            FileName = "TestArtist - TestTitle.wav"
        };
        var formula = "%fartist% - %title%";
        
        TagParser.ApplyTagsFromFilename(testTrack, formula);
        
        Assert.Equal("TestArtist", testTrack.Artists[0]);
        Assert.Equal("TestTitle", testTrack.Title);
        Assert.Equal("TestGenre", testTrack.Genres[0]);
    }

    [Fact]
    public void ParseTags_UpdatesTags_WhenFormulaIsValid()
    {
        var testTrack = new Track()
        {
            Artists = ["TestArtist"],
            Title = "TestTitle",
            Genres = ["TestGenre"],
            FileName = "NewArtist - NewTitle (NewGenre).wav"
        };
        var formula = "%Artist% - %tiTle% (%genrE%)";
        
        TagParser.ApplyTagsFromFilename(testTrack, formula);
        
        Assert.Equal("NewArtist", testTrack.Artists[0]);
        Assert.Equal("NewTitle", testTrack.Title);
        Assert.Equal("NewGenre", testTrack.Genres[0]);
        Assert.Equal("NewArtist - NewTitle (NewGenre).wav", testTrack.FileName);
    }
}