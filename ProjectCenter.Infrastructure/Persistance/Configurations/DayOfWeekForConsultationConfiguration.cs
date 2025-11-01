using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class DayOfWeekForConsultationConfiguration : IEntityTypeConfiguration<DayOfWeekForConsultation>
    {
        public void Configure(EntityTypeBuilder<DayOfWeekForConsultation> builder)
        {
            builder.ToTable("DayOfWeekForConsultation");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(10);
        }
    }
}
