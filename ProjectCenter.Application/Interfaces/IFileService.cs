using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectCenter.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile imageFile);
        void DeleteImage(string imagePath);
        Task<string> SaveProjectFileAsync(IFormFile file);
        Task<string> SaveDocumentationFileAsync(IFormFile file);
        void DeleteProjectFile(string filePath);
        void DeleteDocumentationFile(string filePath);
        Task<FileStreamResult> GetFileAsync(string filePath);
    }
}