using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.EntityType;

public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(nameof(Notification));

        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id);
        
        builder.Property(n => n.CustomerId).IsRequired();
        builder.Property(n => n.Title).IsRequired();
        builder.Property(n => n.Description).IsRequired();
        builder.Property(n => n.SendAt).IsRequired();
        builder.Property(n => n.Status).IsRequired();
        builder.Property(n => n.Type).IsRequired();
    }
}