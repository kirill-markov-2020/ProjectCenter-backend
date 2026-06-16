using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Core.Entities
{
    public class Grade
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }
        public int TeacherId { get; set; }

        public int Value { get; set; }
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Project Project { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
