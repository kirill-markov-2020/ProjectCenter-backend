using Moq;
using Xunit;
using AutoMapper;
using ProjectCenter.Application.Services;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Core.Exceptions;

namespace ProjectCenter.Test.Application.Services
{
    public class ProjectServiceTests
    {
        [Fact]
        public async Task CreateProjectAsync_ShouldPreventCreation_WhenActiveProjectExists()
        {
            var mockProjectRepository = new Mock<IProjectRepository>();
            var mockUserRepository = new Mock<IUserRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockFileService = new Mock<IFileService>();

            var testStudent = new Student
            {
                Id = 1,
                UserId = 100,
                TeacherId = 10
            };

            mockUserRepository.Setup(repo => repo.GetStudentByUserIdAsync(100))
                .ReturnsAsync(testStudent);

            var existingProject = new Project { Id = 5, Title = "Текущий проект" };
            mockProjectRepository.Setup(repo => repo.GetActiveProjectByStudentIdAsync(1))
                .ReturnsAsync(existingProject);

            var projectService = new ProjectService(
                mockProjectRepository.Object,
                mockUserRepository.Object,
                mockMapper.Object,
                mockFileService.Object);

            var newProjectRequest = new CreateProjectRequestDto
            {
                Title = "Новый проект",
                TypeId = 2,
                SubjectId = 3,
                IsPublic = false
            };
            await Assert.ThrowsAsync<ActiveProjectExistsException>(
                () => projectService.CreateProjectAsync(newProjectRequest, 100));
        }
    }
}