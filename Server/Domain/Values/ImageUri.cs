using Functional;
using System;
using System.Collections.Generic;
using System.IO;

namespace FoodAndMeals.Domain.Values
{
    public class ImageUri
    {
        private static HashSet<string> imageFileExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };

        public Uri Uri { get; }

        private ImageUri(Uri uri) => Uri = uri;

        public static Result<ImageUri> CreateFrom(string uri)
        {
            if (null == uri)
            {
                return new ErrorMessage("uri cannot be null");
            }

            Uri parsedUri;
            try
            {
                parsedUri = new Uri(uri);
            }
            catch
            {
                return new ErrorMessage($"The value \"{uri}\" is not a valid Uri.");
            }

            var fileExtension = Path.GetExtension(parsedUri.LocalPath);
            if (!imageFileExtensions.Contains(fileExtension))
            {
                return new ErrorMessage($"Uri \"{uri}\" is not regonized as a link to a file.");
            }

            return new ImageUri(parsedUri);
        }
    }
}
