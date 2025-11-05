using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public class CollegeBuilding
{
    public int Id { get; set; }
    public string Name { get; set; }


    public virtual ICollection<ConsultationSchedule> ConsultationSchedules { get; set; } = new List<ConsultationSchedule>();
}
