using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public partial class CollegeBuilding
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ConsultationSchedule> ConsultationSchedules { get; set; } = new List<ConsultationSchedule>();
}
