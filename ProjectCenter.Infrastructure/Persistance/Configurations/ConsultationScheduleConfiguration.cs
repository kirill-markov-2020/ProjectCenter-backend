using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class ConsultationScheduleConfiguration : IEntityTypeConfiguration<ConsultationSchedule>
    {
        public void Configure(EntityTypeBuilder<ConsultationSchedule> builder)
        {
            builder.ToTable("ConsultationSchedule");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.StartTime).IsRequired();
            builder.Property(c => c.EndTime).IsRequired();

            builder.HasOne(c => c.Teacher)
                   .WithMany()
                   .HasForeignKey(c => c.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Cabinet)
                   .WithMany()
                   .HasForeignKey(c => c.CabinetId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Building)
                   .WithMany()
                   .HasForeignKey(c => c.BuildingId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.DayOfWeekNavigation)
                   .WithMany()
                   .HasForeignKey(c => c.DayOfWeek)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
