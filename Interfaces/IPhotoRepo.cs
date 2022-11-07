using FleaApp_Api.Entities;

namespace FleaApp_Api.Interfaces
{
    public interface IPhotoRepo
    {
        Task<Photo> GetPhotoById(int id);
        void RemovePhoto(Photo photo);
    }
}