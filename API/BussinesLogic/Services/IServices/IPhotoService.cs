
using CloudinaryDotNet.Actions;

namespace API.BussinesLogic.Services.IServices
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}