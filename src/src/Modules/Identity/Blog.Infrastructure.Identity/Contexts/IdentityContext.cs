using System;
using Blog.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Identity.Contexts;

public class IdentityContext : IdentityDbContext<ApplicationUser, SystemRole, string>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Identity");
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("User", "Identity");
        });

        builder.Entity<SystemRole>(entity =>
        {
            entity.ToTable("Role", "Identity");
        });
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles", "Identity");
        });

        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims", "Identity");
        });

        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins", "Identity");
        });

        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims", "Identity");
        });

        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens", "Identity");
        });

        // builder.Entity<UserSetting>(entity =>
        // {
        //     entity.ToTable("UserSetting", "Identity");
        // });
        // builder.Entity<ApplicationUser>()
        //        .HasMany(b => b.UserSettings)
        //        .WithOne();

        // builder.Entity<ApplicationSetting>(entity =>
        // {
        //     entity.ToTable("ApplicationSetting", "Identity");
        // });

        builder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });
    }
}
