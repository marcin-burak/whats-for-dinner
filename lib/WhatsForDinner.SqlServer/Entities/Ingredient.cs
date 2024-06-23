using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace WhatsForDinner.SqlServer.Entities;

public sealed class Ingredient
{
	public string Id { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public List<MealIngredient> Meals { get; set; } = [];



	public static Ingredient Milk => new()
	{
		Id = "MILK",
		Name = "Mleko"
	};

	public static Ingredient Flour => new()
	{
		Id = "FLOUR",
		Name = "Mąka"
	};

	public static IReadOnlyCollection<Ingredient> All => typeof(Ingredient)
		.GetProperties(BindingFlags.Public | BindingFlags.Static)
		.Where(property => property.PropertyType == typeof(Ingredient))
		.Select(property => property.GetValue(null))
		.Cast<Ingredient>()
		.ToArray();
}

internal sealed class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
	public void Configure(EntityTypeBuilder<Ingredient> builder)
	{
		builder.Property(ingredient => ingredient.Id)
			.HasMaxLength(20);

		builder.Property(ingredient => ingredient.Name)
			.HasMaxLength(200);

		builder.HasData(Ingredient.All);
	}
}