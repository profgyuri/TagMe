using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using TagLib;
using TagMe.Avalonia.Models;

namespace TagMe.Avalonia;

public static class Extensions
{
    // This just looks cool
    extension(IPicture[] pictures)
    {
        public Bitmap? LoadCover()
        {
            var picture = pictures.FirstOrDefault(p => p.Type == PictureType.FrontCover)
                          ?? pictures.FirstOrDefault();
            
            if (picture is null || picture.Data.Count == 0)
                return null;

            using var stream = new MemoryStream(picture.Data.Data);

            try
            {
                return Bitmap.DecodeToWidth(stream, 26);
            }
            catch
            {
                return null;
            }
        }
    }

    extension(List<Track> tracks)
    {
        public IEnumerable<Track> FilterByValue(string value)
        {
            foreach (var track in tracks)
            {
                if (track.Album.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.AlbumArtists.Any(a => a?.Contains(value, StringComparison.OrdinalIgnoreCase) == true) ||
                    track.AmazonId.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.Artists.Any(a => a?.Contains(value, StringComparison.OrdinalIgnoreCase) == true) ||
                    track.ArtistsRoles.Any(a => a?.Contains(value, StringComparison.OrdinalIgnoreCase) == true) ||
                    track.Comment.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.Genres.Any(g => g?.Contains(value, StringComparison.OrdinalIgnoreCase) == true) ||
                    track.Title.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.Composers.Any(c => c?.Contains(value, StringComparison.OrdinalIgnoreCase) == true) ||
                    track.Conductor.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.Copyright.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.Lyrics.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.Genres.Any(g => g?.Contains(value, StringComparison.OrdinalIgnoreCase) == true) ||
                    track.Grouping.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.InitialKey.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.ISRC.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.RemixedBy.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.Publisher.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                    track.Path.Contains(value, StringComparison.OrdinalIgnoreCase))
                    yield return track;
            }
        }
    }
}