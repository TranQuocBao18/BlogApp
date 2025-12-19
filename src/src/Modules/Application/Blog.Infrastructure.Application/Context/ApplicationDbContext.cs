using System;
using Blog.Domain.Shared.Common;
using Blog.Infrastructure.Application.Context.Configurations;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Application.Context;

public class ApplicationDbContext : BaseDbContext<ApplicationDbContext>
{
    public const string DefaultSchema = "application";
    private readonly IDateTimeService _dateTime;
    private readonly IAuthenticatedUserService _authenticatedUser;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options, dateTime, authenticatedUser)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _dateTime = dateTime;
        _authenticatedUser = authenticatedUser;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.ApplyConfiguration(new BannerConfiguration());
        modelBuilder.ApplyConfiguration(new BlogConfiguration());
        modelBuilder.ApplyConfiguration(new BlogTagConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new LikeConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        foreach (var property in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,6)");
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = _dateTime.NowUtc;
                    entry.Entity.CreatedBy = _authenticatedUser.UserId;
                    entry.Entity.IsDeleted = false;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModified = _dateTime.NowUtc;
                    entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
