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
            builder.HasKey(s => s.Id);

            builder.Property(s => s.UserId).IsRequired();
            builder.Property(s => s.GroupId).IsRequired();
            builder.Property(s => s.TeacherId).IsRequired();

            builder.HasOne(s => s.User)
                   .WithMany()
                   .HasForeignKey(s => s.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Group)
                   .WithMany()
                   .HasForeignKey(s => s.GroupId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Teacher)
                   .WithMany()
                   .HasForeignKey(s => s.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
