using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public class StatusProject
{
    public int Id { get; set; }
    public string Name { get; set; }


    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
