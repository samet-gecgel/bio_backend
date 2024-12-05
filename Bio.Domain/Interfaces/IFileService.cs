using Microsoft.AspNetCore.Http;

namespace Bio.Domain.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string directoryName, string fileName);
        void DeleteFile(string filePath);
    }
}
