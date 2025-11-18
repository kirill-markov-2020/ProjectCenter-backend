using AutoMapper;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Exceptions;

namespace ProjectCenter.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> GetProjectsForUserAsync(int userId, bool isAdmin)
        {
            var projects = isAdmin
                ? await _projectRepository.GetAllProjectsAsync()
                : await _projectRepository.GetPublicProjectsAsync();

            return _mapper.Map<List<ProjectDto>>(projects);
        }
        public async Task<ProjectDto> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);

            if (project == null)
                throw new ProjectNotFoundException(id);

            return _mapper.Map<ProjectDto>(project);
        }
    }
}
