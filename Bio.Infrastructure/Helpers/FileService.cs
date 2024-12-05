using Microsoft.AspNetCore.Http;
using Bio.Domain.Interfaces;
using Bio.Domain.Constant;

namespace Bio.Infrastructure.Helpers
{
    public class FileService(string basePath) : IFileService
    {
        private readonly string _basePath = basePath;

        public async Task<string> SaveFileAsync(IFormFile file, string directoryName, string fileName)
        {
            if (file == null || file.Length == 0)
                return null;

            var extension = Path.GetExtension(file.FileName);

            fileName = $"{fileName}{extension}";

            var directoryPath = Path.Combine(_basePath, directoryName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{Url.BaseUrl}/Files/{directoryName}/{fileName}";
        }


        public void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(_basePath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
