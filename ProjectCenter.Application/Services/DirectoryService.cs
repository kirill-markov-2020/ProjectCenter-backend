using ProjectCenter.Application.DTOs.Directory;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Services
{
    public class DirectoryService : IDirectoryService
    {
        private readonly IDirectoryRepository _repo;
        private readonly IGroupRepository _groupRepository;

        public DirectoryService(IDirectoryRepository repo, IGroupRepository groupRepository)
        {
            _repo = repo;
            _groupRepository = groupRepository;
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
                Name = g.Name,
            }).ToList();
        }
        public async Task<GroupDto> CreateGroupAsync(CreateGroupDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Название группы не может быть пустым");

            var group = new Group
            {
                Name = dto.Name.Trim()
            };

            await _groupRepository.AddAsync(group);

            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name
            };
        }
    }
}
