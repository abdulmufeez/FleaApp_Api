using fleaApi.Data;
using FleaApp_Api.Dtos;
using FleaApp_Api.Entities;
using FleaApp_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FleaApp_Api.Repositories
{
    public class PhotoRepo : IPhotoRepo
    {
        private readonly DataContext _context;
        public PhotoRepo(DataContext context)
        {
            _context = context;
        }

        public async Task<Photo> GetPhotoById(int id)
        {
            return await _context.Photos
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public Task<IEnumerable<PhotoDto>> GetPhotos()
        {
            throw new NotImplementedException();
        }

        public void RemovePhoto(Photo photo)
        {
            _context.Photos.Remove(photo);
        }
    }
}