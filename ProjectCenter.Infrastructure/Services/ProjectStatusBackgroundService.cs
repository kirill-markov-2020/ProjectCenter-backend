using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectCenter.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ProjectCenter.Infrastructure.Services
{
    public class ProjectStatusBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProjectStatusBackgroundService> _logger;

        public ProjectStatusBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<ProjectStatusBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var scheduledTime = new TimeSpan(8, 30, 0); 
                var targetTime = now.Date.Add(scheduledTime);

                if (now >= targetTime)
                    targetTime = targetTime.AddDays(1);

                var delay = targetTime - now;
                _logger.LogInformation("Следующая проверка статусов проектов в {TargetTime}", targetTime);

                await Task.Delay(delay, stoppingToken);

                await UpdateProjectStatuses(stoppingToken);
            }
        }

        private async Task UpdateProjectStatuses(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Начинаем обновление статусов проектов по дедлайну");

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var today = DateTime.Today;
            var targetStatusIds = new[] { 4, 5, 6, 7 }; 

            var projectsToUpdate = await context.Projects
                .Where(p => p.DateDeadline.Date <= today && !targetStatusIds.Contains(p.StatusId))
                .ToListAsync(stoppingToken);

            if (!projectsToUpdate.Any())
            {
                _logger.LogInformation("Нет проектов для обновления статуса");
                return;
            }

            foreach (var project in projectsToUpdate)
            {
                var oldStatus = project.StatusId;
                project.StatusId = 4; 
                _logger.LogInformation("Проект ID {ProjectId} '{Title}' переведён из статуса {OldStatus} в статус 4 (На защите)",
                    project.Id, project.Title, oldStatus);
            }

            await context.SaveChangesAsync(stoppingToken);
            _logger.LogInformation("Обновлено {Count} проектов", projectsToUpdate.Count);
        }
    }
}