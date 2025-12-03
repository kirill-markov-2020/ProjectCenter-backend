using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Exceptions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProjectCenter.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private const string ImagesFolder = "Images";
        private const string ProjectsFolder = "Projects";
        private const string DocumentationFolder = "Documentation";

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
        public async Task<string> SaveProjectFileAsync(IFormFile file)
        {
            return await SaveFileAsync(file, ProjectsFolder, new[] { ".zip", ".rar", ".7z" }, 50 * 1024 * 1024);
        }

        public async Task<string> SaveDocumentationFileAsync(IFormFile file)
        {
            return await SaveFileAsync(file, DocumentationFolder, new[] { ".pdf", ".doc", ".docx", ".txt" }, 10 * 1024 * 1024);
        }

        private async Task<string> SaveFileAsync(IFormFile file, string folder, string[] allowedExtensions, long maxSize)
        {
            if (file == null || file.Length == 0)
                return null;

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new FileValidationException($"Недопустимый формат файла. Разрешены: {string.Join(", ", allowedExtensions)}");

            if (file.Length > maxSize)
                throw new FileValidationException($"Размер файла не должен превышать {maxSize / 1024 / 1024}MB.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var folderPath = Path.Combine(_environment.WebRootPath, folder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{folder}/{fileName}";
        }

        public void DeleteProjectFile(string filePath)
        {
            DeleteFile(filePath, ProjectsFolder);
        }

        public void DeleteDocumentationFile(string filePath)
        {
            DeleteFile(filePath, DocumentationFolder);
        }

        private void DeleteFile(string filePath, string expectedFolder)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                var cleanPath = filePath.TrimStart('/');
                var fullPath = Path.Combine(_environment.WebRootPath, cleanPath);

                if (File.Exists(fullPath) && cleanPath.StartsWith($"{expectedFolder}/"))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении файла {filePath}: {ex.Message}");
            }
        }

        public async Task<FileStreamResult> GetFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new FileNotFoundException("Файл не найден");

            var cleanPath = filePath.TrimStart('/');
            var fullPath = Path.Combine(_environment.WebRootPath, cleanPath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Файл не найден");

            var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var contentType = GetContentType(fullPath);
            var fileName = Path.GetFileName(fullPath);

            return new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileName
            };
        }

        private string GetContentType(string path)
        {
            var extension = Path.GetExtension(path).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".zip" => "application/zip",
                ".rar" => "application/x-rar-compressed",
                ".7z" => "application/x-7z-compressed",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }
    }
}