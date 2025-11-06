using AutoMapper;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.Interfaces;

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
    }
}
