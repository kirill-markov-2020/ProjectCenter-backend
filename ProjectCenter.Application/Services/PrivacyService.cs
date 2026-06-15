using ProjectCenter.Application.DTOs.Privacy;
using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.Application.Services
{
    public class PrivacyService : IPrivacyService
    {
        private readonly IDataStorageCategoryRepository _repository;

        private static readonly List<DataStorageCategoryDto> _defaultCategories =
        [
            new() { Name = "Сайт", Purpose = "", RetentionPeriod = "" },
            new() { Name = "Пользователи", Purpose = "", RetentionPeriod = "" },
            new() { Name = "Проекты", Purpose = "", RetentionPeriod = "" },
            new() { Name = "Заявки", Purpose = "", RetentionPeriod = "" },
            new() { Name = "Комментарии", Purpose = "", RetentionPeriod = "" },
            new() { Name = "Уведомления", Purpose = "", RetentionPeriod = "" },
            new() { Name = "Файлы", Purpose = "", RetentionPeriod = "" },
        ];

        public PrivacyService(IDataStorageCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DataStorageCategoryDto>> GetDataStorageSummaryAsync()
        {
            var categories = await _repository.GetAllAsync();

            if (categories.Count == 0)
                return _defaultCategories;

            return categories.Select(c => new DataStorageCategoryDto
            {
                Name = c.Name,
                Purpose = c.Purpose ?? "",
                RetentionPeriod = c.RetentionPeriod ?? ""
            }).ToList();
        }
    }
}
