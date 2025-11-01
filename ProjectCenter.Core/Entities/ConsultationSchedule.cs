using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public partial class ConsultationSchedule
{
    public int Id { get; set; }

    public int TeacherId { get; set; }

    public int DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int CabinetId { get; set; }

    public int BuildingId { get; set; }

    public virtual CollegeBuilding Building { get; set; } = null!;

    public virtual Cabinet Cabinet { get; set; } = null!;

    public virtual DayOfWeekForConsultation DayOfWeekNavigation { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}
