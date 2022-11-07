using CloudinaryDotNet.Actions;

namespace FleaApp_Api.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);        
    }
}