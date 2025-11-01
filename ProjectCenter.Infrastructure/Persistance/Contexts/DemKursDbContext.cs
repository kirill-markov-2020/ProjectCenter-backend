using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjectCenter.Core;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistance.Contexts;

public partial class DemKursDbContext : DbContext
{
    public DemKursDbContext()
    {
    }

    public DemKursDbContext(DbContextOptions<DemKursDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cabinet> Cabinets { get; set; }

    public virtual DbSet<CollegeBuilding> CollegeBuildings { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<ConsultationSchedule> ConsultationSchedules { get; set; }

    public virtual DbSet<DayOfWeekForConsultation> DayOfWeekForConsultations { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<StatusProject> StatusProjects { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TypeProject> TypeProjects { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DemKursDbContext).Assembly);
    }
}
