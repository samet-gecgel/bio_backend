using Microsoft.AspNetCore.Http;

namespace Bio.Domain.Interfaces
{
    public interface IPhotoService
    {
        Task<string> SavePhotoAsync(IFormFile photo, string directoryName, string fileName);
        void DeletePhoto(string filePath);
    }
}
