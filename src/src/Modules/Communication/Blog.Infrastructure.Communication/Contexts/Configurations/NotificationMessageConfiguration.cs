using System;
using Blog.Domain.Communication.Entities;
using Blog.Domain.Communication.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Communication.Contexts.Configurations;

public class NotificationMessageConfiguration : IEntityTypeConfiguration<NotificationMessage>
{
    public void Configure(EntityTypeBuilder<NotificationMessage> builder)
    {
        builder.ToTable("NotificationMessages", CommunicationDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v))
            .IsRequired();

        builder.Property(x => x.Title).HasMaxLength(500);
        builder.Property(x => x.ContentNotify).HasMaxLength(500);
        builder.Property(x => x.ReferenceData).HasMaxLength(700);
        builder
            .Property(o => o.NotificationType)
            .HasConversion(v => v.ToString(), v => (NotificationType)Enum.Parse(typeof(NotificationType), v))
            .HasMaxLength(100);
    }
}
