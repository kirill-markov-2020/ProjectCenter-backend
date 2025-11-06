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

            builder.HasOne(cs => cs.Teacher)
                   .WithMany(t => t.ConsultationSchedules)
                   .HasForeignKey(cs => cs.TeacherId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cs => cs.DayOfWeekForConsultation)
                   .WithMany(d => d.ConsultationSchedules)
                   .HasForeignKey(cs => cs.DayOfWeek)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cs => cs.Cabinet)
                   .WithMany(c => c.ConsultationSchedules)
                   .HasForeignKey(cs => cs.CabinetId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cs => cs.Building)
                   .WithMany(b => b.ConsultationSchedules)
                   .HasForeignKey(cs => cs.BuildingId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
