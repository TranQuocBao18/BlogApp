using System;
using Blog.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Application.Context.Configurations;

public class BlogTagConfiguration : IEntityTypeConfiguration<BlogTag>
{
    public void Configure(EntityTypeBuilder<BlogTag> builder)
    {
        builder.ToTable("BlogTag", ApplicationDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
                .HasConversion(v => v.ToString(), v => Guid.Parse(v))
                .IsRequired();

        builder.Property(x => x.BlogId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        builder.Property(x => x.TagId)
            .HasConversion(v => v.ToString(), v => Guid.Parse(v));

        // Configure IsDeleted as bit type
        builder.Property(x => x.IsDeleted)
            .HasColumnType("bit");

        // Relationships
        builder.HasOne(x => x.Blog)
            .WithMany(b => b.BlogTags)
            .HasForeignKey(x => x.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Tag)
            .WithMany(t => t.BlogTags)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
