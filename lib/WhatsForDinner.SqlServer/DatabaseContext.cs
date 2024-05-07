using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WhatsForDinner.SqlServer.Entities;

namespace WhatsForDinner.SqlServer;

public sealed class DatabaseContext(DbContextOptions<DatabaseContext> options) : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        builder.Entity<User>().ToTable("User");
        builder.Entity<IdentityRole<int>>().ToTable("Role");
        builder.Entity<IdentityUserRole<int>>().ToTable("User role");
        builder.Entity<IdentityUserClaim<int>>().ToTable("User claim");
        builder.Entity<IdentityUserLogin<int>>().ToTable("Login");
        builder.Entity<IdentityUserToken<int>>().ToTable("Token");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("Role claim");
    }
}
