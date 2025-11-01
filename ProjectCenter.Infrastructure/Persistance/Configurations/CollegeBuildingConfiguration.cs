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
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Name).IsRequired().HasMaxLength(50);
        }
    }
}
