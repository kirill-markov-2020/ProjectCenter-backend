using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Project");

            builder.Property(p => p.Title).HasMaxLength(500).IsRequired();
            builder.Property(p => p.FileProject).HasMaxLength(500);
            builder.Property(p => p.FileDocumentation).HasMaxLength(500);

            builder.HasOne(p => p.Student)
                    .WithMany(s => s.Projects)
                    .HasForeignKey(p => p.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Teacher)
                   .WithMany(t => t.Projects)
                   .HasForeignKey(p => p.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Status)
                   .WithMany(s => s.Projects)
                   .HasForeignKey(p => p.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Type)
                   .WithMany(t => t.Projects)
                   .HasForeignKey(p => p.TypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Subject)
                   .WithMany(s => s.Projects)
                   .HasForeignKey(p => p.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Comment)
                   .WithMany(c => c.Projects)
                   .HasForeignKey(p => p.CommentId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
