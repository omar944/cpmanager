using CloudinaryDotNet.Actions;

namespace API.Interfaces;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile imageFile);

    Task<DeletionResult> DeletePhotoAsync(string publicId);
}