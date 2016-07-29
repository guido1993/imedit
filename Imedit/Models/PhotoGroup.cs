using System.Collections.ObjectModel;

namespace Imedit.Models
{
    public class PhotoGroup
    {
        public string Header { get; set; }
        public ObservableCollection<Photo> Photos { get; set; }
        public bool HasPhotos { get { return this.Photos.Count > 0; } }

        public PhotoGroup(string header, ObservableCollection<Photo> photos)
        {
            this.Header = header;
            this.Photos = photos;
        }
    }
}