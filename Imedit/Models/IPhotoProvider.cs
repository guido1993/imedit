using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Imedit.Models
{
    public interface IPhotoProvider
    {
        Task<ObservableCollection<PhotoGroup>> LoadPhotos();
    }
}