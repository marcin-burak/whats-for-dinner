using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WhatsForDinner.SqlServer.Entities;

public sealed class User
{
	public Guid Id { get; set; }

	public string Email { get; set; } = string.Empty;

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

	public string FullName { get; set; } = string.Empty;

	public List<Group> Groups { get; } = [];

	public List<Membership> Memberships { get; } = [];

	public List<Meal> CreatedMeals { get; } = [];

	public List<Meal> ModifiedMeals { get; } = [];
}

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(user => user.Id)
			.IsClustered(false);

		builder.Property(user => user.Email)
			.HasMaxLength(200);

		builder.Property(user => user.FirstName)
			.HasMaxLength(100);

		builder.Property(user => user.LastName)
			.HasMaxLength(100);

		builder.Property(user => user.FullName)
			.HasMaxLength(201);
	}
}