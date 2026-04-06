using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Text)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(n => n.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(n => n.IsRead)
                .IsRequired()
                .HasDefaultValue(false);

 
            builder.HasIndex(n => n.RecipientId)
                .HasDatabaseName("IX_Notification_RecipientId");

            builder.HasIndex(n => n.TypeId)
                .HasDatabaseName("IX_Notification_TypeId");

            builder.HasIndex(n => n.IsRead)
                .HasDatabaseName("IX_Notification_IsRead");

            builder.HasIndex(n => n.CreatedAt)
                .HasDatabaseName("IX_Notification_CreatedAt");

       
            builder.HasOne(n => n.Recipient)
                .WithMany(u => u.ReceivedNotifications)
                .HasForeignKey(n => n.RecipientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(n => n.Type)
                .WithMany(t => t.Notifications)
                .HasForeignKey(n => n.TypeId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}