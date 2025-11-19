using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProjectCenter.Application.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProjectCenter.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private const string ImagesFolder = "Images";

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

           
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Недопустимый формат файла. Разрешены только изображения.");

          
            if (imageFile.Length > 5 * 1024 * 1024)
                throw new ArgumentException("Размер файла не должен превышать 5MB.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var imagesPath = Path.Combine(_environment.WebRootPath, ImagesFolder);

            if (!Directory.Exists(imagesPath))
                Directory.CreateDirectory(imagesPath);

            var fullPath = Path.Combine(imagesPath, fileName);

           
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/{ImagesFolder}/{fileName}";
        }

        public void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;

            try
            {
              
                var cleanPath = imagePath.TrimStart('/');
                var fullPath = Path.Combine(_environment.WebRootPath, cleanPath);

               
                if (File.Exists(fullPath) && cleanPath.StartsWith("Images/"))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
          
                Console.WriteLine($"Ошибка при удалении файла {imagePath}: {ex.Message}");
            }
        }
    }
}