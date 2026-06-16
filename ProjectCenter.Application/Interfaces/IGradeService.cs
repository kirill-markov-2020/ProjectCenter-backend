using ProjectCenter.Application.DTOs.Grade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Application.Interfaces
{
 
    public interface IGradeService
    {
        Task<GradeDto> SetGradeAsync(int teacherUserId, GradeRequestDto dto);
        Task<GradeDto> UpdateGradeAsync(int teacherUserId, int projectId, GradeRequestDto dto);
        Task<GradeDto> GetGradeByProjectIdAsync(int projectId);
        Task<bool> HasGradeAsync(int projectId);
    }
}
