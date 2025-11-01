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
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title).IsRequired().HasMaxLength(50);
            builder.Property(p => p.FileProject).IsRequired().HasMaxLength(50);
            builder.Property(p => p.FileDocumentation).IsRequired().HasMaxLength(50);
            builder.Property(p => p.DateDeadline).IsRequired();
            builder.Property(p => p.CreatedDate).IsRequired();

            builder.HasOne(p => p.Student)
                   .WithMany()
                   .HasForeignKey(p => p.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Teacher)
                   .WithMany()
                   .HasForeignKey(p => p.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Status)
                   .WithMany()
                   .HasForeignKey(p => p.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Type)
                   .WithMany()
                   .HasForeignKey(p => p.TypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Subject)
                   .WithMany()
                   .HasForeignKey(p => p.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Comment)
                   .WithMany()
                   .HasForeignKey(p => p.CommentId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
