using Microsoft.EntityFrameworkCore;
using ProjectCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Infrastructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Group> Groups { get; set; }
      
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }         
        public DbSet<TypeNotification> TypeNotifications { get; set; }
        public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TypeProject> TypeProjects { get; set; }
        public DbSet<StatusProject> StatusProjects { get; set; }
        public DbSet<DataStorageCategory> DataStorageCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
