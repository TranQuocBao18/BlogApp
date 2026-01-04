using System;
using Blog.Domain.Shared.Common;
using Blog.Infrastructure.Communication.Contexts.Configurations;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Communication.Contexts;

public class CommunicationDbContext : BaseDbContext<CommunicationDbContext>
{
    public const string DefaultSchema = "communication";
    private readonly IDateTimeService _dateTime;
    private readonly IAuthenticatedUserService _authenticatedUser;

    public CommunicationDbContext(DbContextOptions<CommunicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options, dateTime, authenticatedUser)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _dateTime = dateTime;
        _authenticatedUser = authenticatedUser;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.ApplyConfiguration(new NotificationUserConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationMessageConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommunicationDbContext).Assembly);
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
                    entry.Entity.IsDeleted = false;
                    break;
                case EntityState.Deleted:
                    entry.Entity.LastModified = _dateTime.NowUtc;
                    entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
