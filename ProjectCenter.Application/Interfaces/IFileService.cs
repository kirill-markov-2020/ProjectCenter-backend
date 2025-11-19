using Microsoft.AspNetCore.Http;

namespace ProjectCenter.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile imageFile);
        void DeleteImage(string imagePath);
    }
}