using Bio.Domain.Constant;
using Bio.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bio.Infrastructure.Helpers
{
    public class PhotoService(string photosPath) : IPhotoService
    {
        private readonly string _photosPath = photosPath;

        public async Task<string> SavePhotoAsync(IFormFile photo, string directoryName, string fileName)
        {
            if (photo == null || photo.Length == 0)
                return null;

            var extension = Path.GetExtension(photo.FileName);
            fileName = $"{fileName}{extension}";

            var directoryPath = Path.Combine(_photosPath, directoryName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            return $"{Url.BaseUrl}/Photos/{directoryName}/{fileName}";
        }


        public void DeletePhoto(string filePath)
        {
            var fullPath = Path.Combine(_photosPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
