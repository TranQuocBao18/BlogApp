using System;
using Blog.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Application.Context.Configurations;

public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
{
    public void Configure(EntityTypeBuilder<CommentLike> builder)
    {
        builder.ToTable("CommentLike", ApplicationDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
                .HasConversion(v => v.ToString(), v => Guid.Parse(v))
                .IsRequired();

        builder.Property(x => x.CommentId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(x => x.UserId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // Relationships
        builder.HasOne(x => x.Comment)
            .WithMany(c => c.Likes)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
