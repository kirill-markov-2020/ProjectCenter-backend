using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public partial class Student
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int GroupId { get; set; }

    public int TeacherId { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual Teacher Teacher { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
