using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Application.DTOs.Grade
{
    public class GradeRequestDto
    {
        public int ProjectId { get; set; }
        public int Value { get; set; }
        public string? Comment { get; set; }
    }
}
