using System;
using System.Collections.Generic;

namespace ProjectCenter.Core.Entities;

public class ConsultationSchedule
{
    public int Id { get; set; }
    public int TeacherId { get; set; }
    public int DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int CabinetId { get; set; }
    public int BuildingId { get; set; }


    public virtual Teacher Teacher { get; set; }
    public virtual DayOfWeekForConsultation DayOfWeekForConsultation { get; set; }
    public virtual Cabinet Cabinet { get; set; }
    public virtual CollegeBuilding Building { get; set; }
}
