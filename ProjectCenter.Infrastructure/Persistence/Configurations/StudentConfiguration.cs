using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Student");

            builder.HasOne(s => s.Group)
                   .WithMany(g => g.Students)
                   .HasForeignKey(s => s.GroupId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.Property(s => s.DateEnrolled)
                    .IsRequired();

            builder.Property(s => s.DateGraduated)
                    .IsRequired(false);
            builder.HasOne(s => s.Teacher)
                   .WithMany(t => t.Students)
                   .HasForeignKey(s => s.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
