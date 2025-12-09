using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Api.Controllers;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Application.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ProjectCenter.Test.Api.Controllers
{
    public class ProjectsControllerTests
    {
        [Fact]
        public async Task GetProjects_ShouldReturnAllProjects_ForAdminUser()
        {
            var mockProjectService = new Mock<IProjectService>();

            var expectedProjects = new List<ProjectDto>
            {
                new ProjectDto { Id = 1, Title = "Проект 1" },
                new ProjectDto { Id = 2, Title = "Проект 2" }
            };

            mockProjectService.Setup(service =>
                service.GetProjectsForUserAsync(1, true))
                .ReturnsAsync(expectedProjects);
            var controller = new ProjectsController(mockProjectService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.HttpContext.Items["UserId"] = 1;
            controller.HttpContext.Items["UserRole"] = "Admin";

            var result = await controller.GetProjects();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProjects = Assert.IsType<List<ProjectDto>>(okResult.Value);
            Assert.Equal(2, returnedProjects.Count);
            mockProjectService.Verify(service =>
                service.GetProjectsForUserAsync(1, true), Times.Once);
        }
    }
}