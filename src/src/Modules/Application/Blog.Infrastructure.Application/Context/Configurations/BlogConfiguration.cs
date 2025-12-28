using System;
using Microsoft.EntityFrameworkCore;
using Blog.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Application.Context.Configurations;

public class BlogConfiguration : IEntityTypeConfiguration<BlogEntity>
{
    public void Configure(EntityTypeBuilder<BlogEntity> builder)
    {
        builder.ToTable("Blog", ApplicationDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
                .HasConversion(v => v.ToString(), v => Guid.Parse(v))
                .IsRequired();
        // Foreign Keys conversions
        builder.Property(x => x.CategoryId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(x => x.BannerId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // Relationships
        builder.HasOne(x => x.Category)
            .WithMany(c => c.Blogs)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Comments)
            .WithOne(c => c.Blog)
            .HasForeignKey(c => c.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.BlogTags)
            .WithOne(bt => bt.Blog)
            .HasForeignKey(bt => bt.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Likes)
            .WithOne(l => l.Blog)
            .HasForeignKey(l => l.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
