using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Application.DTOs.Project
{
    public class AdminCreateProjectRequestDto
    {
        public string Title { get; set; }
        public int TypeId { get; set; }
        public int SubjectId { get; set; }
        public bool IsPublic { get; set; }
        public int StudentUserId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime DateDeadline { get; set; }
    }
}
