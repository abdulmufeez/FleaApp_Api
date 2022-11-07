using fleaApi.Data;
using FleaApp_Api.Entities;
using FleaApp_Api.Interfaces;

namespace FleaApp_Api.Repositories
{
    public class PhotoRepo : IPhotoRepo
    {
        private readonly DataContext _context;
        public PhotoRepo(DataContext context)
        {
            _context = context;
        }

        public Task<Photo> GetPhotoById(int id)
        {
            throw new NotImplementedException();
        }

        public void RemovePhoto(Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}