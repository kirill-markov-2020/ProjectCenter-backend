using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Application.DTOs
{
    public class StudentShortDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? GroupName { get; set; }
    }
}
