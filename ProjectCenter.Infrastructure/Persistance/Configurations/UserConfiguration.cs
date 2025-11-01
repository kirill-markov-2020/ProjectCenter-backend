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
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Surname).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Name).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Patronymic).HasMaxLength(50);
            builder.Property(u => u.Login).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Phone).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Photo).HasMaxLength(50);

            builder.Property(u => u.IsAdmin).IsRequired();
        }
    }
}
