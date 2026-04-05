using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Infrastructure.Persistence.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly AppDbContext _context;

        public GradeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Grade?> GetByProjectIdAsync(int projectId)
        {
            return await _context.Grade
                .FirstOrDefaultAsync(g => g.ProjectId == projectId);
        }

        public async Task AddAsync(Grade grade)
        {
            _context.Grade.Add(grade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Grade grade)
        {
            _context.Grade.Update(grade);
            await _context.SaveChangesAsync();
        }
    }
}
