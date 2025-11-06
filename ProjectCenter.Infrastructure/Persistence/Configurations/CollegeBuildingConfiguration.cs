using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class CollegeBuildingConfiguration : IEntityTypeConfiguration<CollegeBuilding>
    {
        public void Configure(EntityTypeBuilder<CollegeBuilding> builder)
        {
            builder.ToTable("CollegeBuilding");
            builder.Property(b => b.Name).HasMaxLength(50).IsRequired();
        }
    }
}
