using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using TagLib;

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

            return Bitmap.DecodeToWidth(stream, 48);
        }
    }
}