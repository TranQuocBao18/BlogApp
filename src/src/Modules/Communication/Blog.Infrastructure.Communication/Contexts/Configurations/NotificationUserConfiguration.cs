using System;
using Blog.Domain.Communication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Communication.Contexts.Configurations;

public class NotificationUserConfiguration : IEntityTypeConfiguration<NotificationUser>
{
    public void Configure(EntityTypeBuilder<NotificationUser> builder)
    {
        builder.ToTable("NotificationUsers", CommunicationDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v))
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v))
            .IsRequired();

        builder.Property(x => x.NotificationId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v))
            .IsRequired();

        builder.HasOne(x => x.PushMultiNotificationMessage)
            .WithMany(x => x.NotificationUsers)
            .HasForeignKey(x => x.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
