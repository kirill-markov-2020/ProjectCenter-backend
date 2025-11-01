//using System;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;

//namespace ProjectCenter.Core.Entities;

//public partial class DemKursDbContext : DbContext
//{
//    public DemKursDbContext()
//    {
//    }

//    public DemKursDbContext(DbContextOptions<DemKursDbContext> options)
//        : base(options)
//    {
//    }

//    public virtual DbSet<Cabinet> Cabinets { get; set; }

//    public virtual DbSet<CollegeBuilding> CollegeBuildings { get; set; }

//    public virtual DbSet<Comment> Comments { get; set; }

//    public virtual DbSet<ConsultationSchedule> ConsultationSchedules { get; set; }

//    public virtual DbSet<DayOfWeekForConsultation> DayOfWeekForConsultations { get; set; }

//    public virtual DbSet<Group> Groups { get; set; }

//    public virtual DbSet<Notification> Notifications { get; set; }

//    public virtual DbSet<Project> Projects { get; set; }

//    public virtual DbSet<StatusProject> StatusProjects { get; set; }

//    public virtual DbSet<Student> Students { get; set; }

//    public virtual DbSet<Subject> Subjects { get; set; }

//    public virtual DbSet<Teacher> Teachers { get; set; }

//    public virtual DbSet<TypeProject> TypeProjects { get; set; }

//    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-38F8P07\\SQLEXPRESS;Database=DemKursDb;Trusted_Connection=True;TrustServerCertificate=True;");

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Cabinet>(entity =>
//        {
//            entity.ToTable("Cabinet");

//            entity.Property(e => e.Name).HasMaxLength(50);
//        });

//        modelBuilder.Entity<CollegeBuilding>(entity =>
//        {
//            entity.ToTable("CollegeBuilding");

//            entity.Property(e => e.Name).HasMaxLength(50);
//        });

//        modelBuilder.Entity<Comment>(entity =>
//        {
//            entity.ToTable("Comment");

//            entity.Property(e => e.Date).HasColumnType("datetime");
//            entity.Property(e => e.Text).HasMaxLength(50);

//            entity.HasOne(d => d.User).WithMany(p => p.Comments)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Comment_User");
//        });

//        modelBuilder.Entity<ConsultationSchedule>(entity =>
//        {
//            entity.ToTable("ConsultationSchedule");

//            entity.Property(e => e.EndTime).HasPrecision(0);
//            entity.Property(e => e.StartTime).HasPrecision(0);

//            entity.HasOne(d => d.Building).WithMany(p => p.ConsultationSchedules)
//                .HasForeignKey(d => d.BuildingId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_ConsultationSchedule_CollegeBuilding");

//            entity.HasOne(d => d.Cabinet).WithMany(p => p.ConsultationSchedules)
//                .HasForeignKey(d => d.CabinetId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_ConsultationSchedule_Cabinet");

//            entity.HasOne(d => d.DayOfWeekNavigation).WithMany(p => p.ConsultationSchedules)
//                .HasForeignKey(d => d.DayOfWeek)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_ConsultationSchedule_DayOfWeekForConsultation");

//            entity.HasOne(d => d.Teacher).WithMany(p => p.ConsultationSchedules)
//                .HasForeignKey(d => d.TeacherId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_ConsultationSchedule_Teacher");
//        });

//        modelBuilder.Entity<DayOfWeekForConsultation>(entity =>
//        {
//            entity.ToTable("DayOfWeekForConsultation");

//            entity.Property(e => e.Name).HasMaxLength(10);
//        });

//        modelBuilder.Entity<Group>(entity =>
//        {
//            entity.ToTable("Group");

//            entity.Property(e => e.Name).HasMaxLength(50);
//        });

//        modelBuilder.Entity<Notification>(entity =>
//        {
//            entity.ToTable("Notification");

//            entity.Property(e => e.Text).HasMaxLength(50);

//            entity.HasOne(d => d.Recipient).WithMany(p => p.NotificationRecipients)
//                .HasForeignKey(d => d.RecipientId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Notification_User");

//            entity.HasOne(d => d.Sender).WithMany(p => p.NotificationSenders)
//                .HasForeignKey(d => d.SenderId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Notification_UserId");
//        });

//        modelBuilder.Entity<Project>(entity =>
//        {
//            entity.ToTable("Project");

//            entity.Property(e => e.FileDocumentation).HasMaxLength(50);
//            entity.Property(e => e.FileProject).HasMaxLength(50);
//            entity.Property(e => e.Title).HasMaxLength(50);

//            entity.HasOne(d => d.Comment).WithMany(p => p.Projects)
//                .HasForeignKey(d => d.CommentId)
//                .HasConstraintName("FK_Project_Comment");

//            entity.HasOne(d => d.Status).WithMany(p => p.Projects)
//                .HasForeignKey(d => d.StatusId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Project_StatusProject");

//            entity.HasOne(d => d.Student).WithMany(p => p.Projects)
//                .HasForeignKey(d => d.StudentId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Project_Student");

//            entity.HasOne(d => d.Subject).WithMany(p => p.Projects)
//                .HasForeignKey(d => d.SubjectId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Project_Subject");

//            entity.HasOne(d => d.Teacher).WithMany(p => p.Projects)
//                .HasForeignKey(d => d.TeacherId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Project_Teacher");

//            entity.HasOne(d => d.Type).WithMany(p => p.Projects)
//                .HasForeignKey(d => d.TypeId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Project_TypeProject");
//        });

//        modelBuilder.Entity<StatusProject>(entity =>
//        {
//            entity.ToTable("StatusProject");

//            entity.Property(e => e.Name).HasMaxLength(50);
//        });

//        modelBuilder.Entity<Student>(entity =>
//        {
//            entity.ToTable("Student");

//            entity.HasOne(d => d.Group).WithMany(p => p.Students)
//                .HasForeignKey(d => d.GroupId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Student_Group");

//            entity.HasOne(d => d.Teacher).WithMany(p => p.Students)
//                .HasForeignKey(d => d.TeacherId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Student_Teacher");

//            entity.HasOne(d => d.User).WithMany(p => p.Students)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Student_UserId");
//        });

//        modelBuilder.Entity<Subject>(entity =>
//        {
//            entity.ToTable("Subject");

//            entity.Property(e => e.Name).HasMaxLength(50);
//        });

//        modelBuilder.Entity<Teacher>(entity =>
//        {
//            entity.ToTable("Teacher");

//            entity.HasOne(d => d.User).WithMany(p => p.Teachers)
//                .HasForeignKey(d => d.UserId)
//                .OnDelete(DeleteBehavior.ClientSetNull)
//                .HasConstraintName("FK_Teacher_UserId");
//        });

//        modelBuilder.Entity<TypeProject>(entity =>
//        {
//            entity.ToTable("TypeProject");

//            entity.Property(e => e.Name).HasMaxLength(50);
//        });

//        modelBuilder.Entity<User>(entity =>
//        {
//            entity.HasKey(e => e.Id).HasName("PK_UserId");

//            entity.ToTable("User");

//            entity.Property(e => e.Email).HasMaxLength(50);
//            entity.Property(e => e.Login).HasMaxLength(50);
//            entity.Property(e => e.Name).HasMaxLength(50);
//            entity.Property(e => e.Password).HasMaxLength(50);
//            entity.Property(e => e.Patronymic).HasMaxLength(50);
//            entity.Property(e => e.Phone).HasMaxLength(50);
//            entity.Property(e => e.Photo).HasMaxLength(50);
//            entity.Property(e => e.Surname).HasMaxLength(50);
//        });

//        OnModelCreatingPartial(modelBuilder);
//    }

//    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
//}
