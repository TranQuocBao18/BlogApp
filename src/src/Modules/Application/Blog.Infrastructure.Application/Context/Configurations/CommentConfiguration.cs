using System;
using Blog.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Application.Context.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comment", ApplicationDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
                .HasConversion(v => v.ToString(), v => Guid.Parse(v))
                .IsRequired();

        builder.Property(x => x.BlogId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(x => x.UserId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(x => x.ParentId)
            .HasConversion(
                v => v.HasValue ? v.Value.ToString() : null,
                v => string.IsNullOrEmpty(v) ? (Guid?)null : Guid.Parse(v)
            );

        // Self-referencing relationship
        builder.HasOne(x => x.ParentComment)
            .WithMany(c => c.ChildComments)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.NoAction); // ⚠️ NO ACTION như trong script

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Likes)
            .WithOne(l => l.Comment)
            .HasForeignKey(l => l.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
