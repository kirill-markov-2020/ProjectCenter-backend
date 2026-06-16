using ProjectCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Application.Interfaces
{
    public interface IGroupRepository
    {
        Task<List<Group>> GetByIdsAsync(IEnumerable<int> ids);
        Task<Group?> GetByIdAsync(int id);
        Task<List<Group>> GetAllAsync();
        Task AddAsync(Group group);
    }
}
