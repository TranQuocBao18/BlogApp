using System;
using Blog.Domain.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Application.Context.Configurations;

public class BannerConfiguration : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.ToTable("Banner", ApplicationDbContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder .Property(x => x.Id)
                .HasConversion(v => v.ToString(), v => Guid.Parse(v))
                .IsRequired();
    }
}
