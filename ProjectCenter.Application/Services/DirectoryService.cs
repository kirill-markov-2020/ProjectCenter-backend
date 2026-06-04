using ProjectCenter.Application.DTOs.Directory;
using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.Application.Services
{
    public class DirectoryService : IDirectoryService
    {
        private readonly IDirectoryRepository _repo;

        public DirectoryService(IDirectoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<StatusProjectDto>> GetStatusesAsync()
            => (await _repo.GetStatusesAsync())
                .Select(s => new StatusProjectDto { Id = s.Id, Name = s.Name })
                .ToList();

        public async Task<List<TypeProjectDto>> GetTypesAsync()
            => (await _repo.GetTypesAsync())
                .Select(t => new TypeProjectDto { Id = t.Id, Name = t.Name })
                .ToList();

        public async Task<List<SubjectDto>> GetSubjectsAsync()
            => (await _repo.GetSubjectsAsync())
                .Select(s => new SubjectDto { Id = s.Id, Name = s.Name })
                .ToList();

        public async Task<List<GroupDto>> GetGroupsAsync()
        {
            var groups = await _repo.GetGroupsAsync();
            return groups.Select(g => new GroupDto
            {
                Id = g.Id,
                SpecialtyCode = g.SpecialtyCode,
                BaseName = g.BaseName,
                FullName = $"{g.SpecialtyCode}-{g.BaseName}"  
            }).ToList();
        }
    }
}
