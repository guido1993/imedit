using System;
using Windows.Storage;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Imedit.Models;

namespace Imedit.Converters
{
    public class ImagePathToSourceConverter : IValueConverter
    {
        private int i = 0;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var image = (Photo) value;
            var folder = KnownFolders.PicturesLibrary;
            StorageFile file;

            if (image.HasSubFolder)
                folder = folder.GetFolderAsync(image.FolderName).AsTask().Result;

            if (image.PhotoUri.OriginalString.StartsWith("ms-appx"))
                return new BitmapImage(image.PhotoUri);

            if (image.HasSubFolder && image.FolderName == "Camera Roll")
            {
                i++;

                if (i > 10)
                    return null;
            }


            file = (StorageFile)folder.TryGetItemAsync(image.ImageName).AsTask().Result;
            var t = file.GetThumbnailAsync(Windows.Storage.FileProperties.ThumbnailMode.PicturesView).AsTask().Result;
            //var stream = .OpenReadAsync().AsTask().Result;
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(t.CloneStream());

            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
