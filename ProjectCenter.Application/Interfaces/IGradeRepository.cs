using ProjectCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Application.Interfaces
{
    public interface IGradeRepository
    {
        Task<Grade?> GetByProjectIdAsync(int projectId);
        Task AddAsync(Grade grade);
        Task UpdateAsync(Grade grade);
    }
}
