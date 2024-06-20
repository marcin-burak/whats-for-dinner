using Microsoft.EntityFrameworkCore;
using WhatsForDinner.SqlServer.Entities;

namespace WhatsForDinner.SqlServer;

public sealed class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
	public DbSet<User> User { get; set; }

	public DbSet<Group> Group { get; set; }

	public DbSet<Membership> Membership { get; set; }

	public DbSet<Meal> Meal { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.ApplyConfigurationsFromAssembly(GetType().Assembly);



		builder.Entity<User>()
			.HasMany(user => user.CreatedMeals)
			.WithOne(meal => meal.CreatedBy);

		builder.Entity<User>()
			.HasMany(user => user.ModifiedMeals)
			.WithOne(meal => meal.ModifiedBy);



		builder.Entity<User>()
			.HasMany(user => user.Groups)
			.WithMany(group => group.Users)
			.UsingEntity<Membership>();
	}
}
