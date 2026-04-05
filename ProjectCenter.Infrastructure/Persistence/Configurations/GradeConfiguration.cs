using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    using global::ProjectCenter.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace ProjectCenter.Infrastructure.Persistence.Configurations
    {
        public class GradeConfiguration : IEntityTypeConfiguration<Grade>
        {
            public void Configure(EntityTypeBuilder<Grade> builder)
            {
                builder.ToTable("Grade");

                builder.HasKey(g => g.Id);

                builder.Property(g => g.Value)
                    .IsRequired();

                builder.Property(g => g.Comment)
                    .HasMaxLength(1000); // комментарий необязательный

                builder.Property(g => g.CreatedAt)
                    .IsRequired();

                // Связь с Project (1 к 1)
                builder.HasOne(g => g.Project)
                    .WithOne(p => p.Grade)
                    .HasForeignKey<Grade>(g => g.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Связь с Teacher
                builder.HasOne(g => g.Teacher)
                    .WithMany(t => t.Grade)
                    .HasForeignKey(g => g.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        }
    }
}
