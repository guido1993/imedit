using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Imedit.Helpers;
using Imedit.Models;

namespace Imedit.ViewModels
{
    public class PhotoProviderReal : IPhotoProvider
    {
        public async Task<ObservableCollection<PhotoGroup>> LoadPhotos()
        {
            var collection = new ObservableCollection<PhotoGroup>();
            var folders = await KnownFolders.PicturesLibrary.GetFoldersAsync();
            
            foreach (var folder in folders)
            {
                var photos = new ObservableCollection<Photo>();
                var files = await folder.GetFilesAsync();

                foreach (var image in files)
                {
                    //Debug.WriteLine(image.Name);

                    if (!image.Name.Contains(".jpg"))
                        continue;
                    
                    var photo = new Photo() { PhotoUri = new Uri(image.Path), ImageName = image.Name, FolderName = folder.Name };
                    photos.Add(photo);
                };
                
                var group = new PhotoGroup(folder.Name, photos);
                collection.Add(group);
            }

            var sample = LoadFakePhotos();
            collection.Add(sample);

            return collection;
        }

        public PhotoGroup LoadFakePhotos()
        {
            var photos = new ObservableCollection<Photo>();

            for (var i = 1; i <= 10; i++)
                photos.Add(new Photo { PhotoUri = new Uri("ms-appx:///Assets/FakePhotos/" + i + ".jpg"), ImageName = i + ".jpg" });
            
            var group = new PhotoGroup(ResourceHelper.GetTranslation("PhotoPivot1"), photos);
            return group;
        }
    }
}