using System;
using Blog.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Application.Context.Configurations;

public class BlogLikeConfiguration : IEntityTypeConfiguration<BlogLike>
{
    public void Configure(EntityTypeBuilder<BlogLike> builder)
    {
        builder.ToTable("BlogLike", ApplicationDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
                .HasConversion(v => v.ToString(), v => Guid.Parse(v))
                .IsRequired();

        builder.Property(x => x.BlogId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(x => x.UserId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // Relationships
        builder.HasOne(x => x.Blog)
            .WithMany(b => b.Likes)
            .HasForeignKey(x => x.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
