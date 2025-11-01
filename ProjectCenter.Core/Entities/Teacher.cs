using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public partial class Teacher
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<ConsultationSchedule> ConsultationSchedules { get; set; } = new List<ConsultationSchedule>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    public virtual User User { get; set; } = null!;
}
