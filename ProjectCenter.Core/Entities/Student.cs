using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public class Student
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GroupId { get; set; }
    public int TeacherId { get; set; }


    public virtual User User { get; set; }
    public virtual Group Group { get; set; }
    public virtual Teacher Teacher { get; set; }
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
