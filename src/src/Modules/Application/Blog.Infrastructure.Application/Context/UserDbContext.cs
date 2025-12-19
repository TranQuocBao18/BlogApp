using System;
using Blog.Infrastructure.Shared.Interfaces;
using Blog.Infrastructure.Shared.Persistences.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Application.Context;

public class UserDbContext : BaseDbContext<UserDbContext>
{
    public const string DefaultSchema = "identity";
    public UserDbContext(DbContextOptions<UserDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options, dateTime, authenticatedUser)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
    }
}
