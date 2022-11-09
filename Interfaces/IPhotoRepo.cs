using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;

namespace FleaApp_Api.Interfaces
{
    public interface IPhotoRepo
    {
        void RemovePhoto(Photo photo);
        Task<Photo> GetPhotoById(int id);
        Task<IEnumerable<PhotoDto>> GetPhotos();
    }
}