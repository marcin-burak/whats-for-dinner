using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace WhatsForDinner.SqlServer.Entities;

public sealed class Effort
{
	public string Id { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public List<Meal> Meals { get; set; } = [];



	public static Effort Low => new()
	{
		Id = "LOW",
		Name = "Niski"
	};

	public static Effort Medium => new()
	{
		Id = "MEDIUM",
		Name = "Średni"
	};

	public static Effort High => new()
	{
		Id = "HIGH",
		Name = "Wysoki"
	};

	public static IReadOnlyCollection<Effort> All => typeof(Effort)
		.GetProperties(BindingFlags.Public | BindingFlags.Static)
		.Where(property => property.PropertyType == typeof(Effort))
		.Select(property => property.GetValue(null))
		.Cast<Effort>()
		.ToArray();
}

internal sealed class EffortConfiguration : IEntityTypeConfiguration<Effort>
{
	public void Configure(EntityTypeBuilder<Effort> builder)
	{
		builder.Property(effort => effort.Id)
			.HasMaxLength(6);

		builder.Property(effort => effort.Name)
			.HasMaxLength(6);

		builder.HasData(Effort.All);
	}
}