using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(u => u.Surname).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Name).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Patronymic).HasMaxLength(50);
            builder.Property(u => u.Login).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Password).HasMaxLength(150).IsRequired();
            builder.Property(u => u.Phone).HasMaxLength(20).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(50).IsRequired();
            builder.Property(u => u.Photo).HasMaxLength(150);

            builder.HasOne(u => u.Student)
                   .WithOne(s => s.User)
                   .HasForeignKey<Student>(s => s.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Teacher)
                   .WithOne(t => t.User)
                   .HasForeignKey<Teacher>(t => t.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
