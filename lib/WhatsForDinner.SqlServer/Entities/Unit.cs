using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace WhatsForDinner.SqlServer.Entities;

public sealed class Unit
{
	public string Id { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public List<MealIngredient> MealIngredients { get; set; } = [];



	public static Unit Gram => new()
	{
		Id = "GRAM",
		Name = "g"
	};

	public static Unit Mililiter => new()
	{
		Id = "MILILITER",
		Name = "ml"
	};

	public static Unit TableSpoon => new()
	{
		Id = "TABLE_SPOON",
		Name = "łyżka stołowa"
	};

	public static Unit Spoon => new()
	{
		Id = "SPOON",
		Name = "łyżeczka"
	};

	public static IReadOnlyCollection<Unit> All => typeof(Unit)
		.GetProperties(BindingFlags.Public | BindingFlags.Static)
		.Where(property => property.PropertyType == typeof(Unit))
		.Select(property => property.GetValue(null))
		.Cast<Unit>()
		.ToArray();
}

internal sealed class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
	public void Configure(EntityTypeBuilder<Unit> builder)
	{
		builder.Property(unit => unit.Id)
			.HasMaxLength(11);

		builder.Property(unit => unit.Name)
			.HasMaxLength(13);

		builder.HasData(Unit.All);
	}
}