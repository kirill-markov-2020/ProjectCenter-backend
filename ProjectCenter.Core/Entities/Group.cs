using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public class Group
{
    public int Id { get; set; }
    public string SpecialtyCode { get; set; } 
    public string BaseName { get; set; }


    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
