using Microsoft.AspNetCore.Http;

namespace ProjectCenter.Application.DTOs.UpdateProject
{
    public class UpdateStudentProjectRequestDto
    {
        public IFormFile? NewProjectFile { get; set; }          // Архив проекта
        public IFormFile? NewDocumentationFile { get; set; }    // Текстовая документация
        public bool? IsPublic { get; set; }                     // Видимость проекта
        public bool? RemoveProjectFile { get; set; }           // Удалить файл проекта
        public bool? RemoveDocumentationFile { get; set; }     // Удалить документацию
    }
}