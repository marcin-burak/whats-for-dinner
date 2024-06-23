using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WhatsForDinner.SqlServer.Entities;

public sealed class MealIngredient
{
	public Guid Id { get; set; }

	public Guid MealId { get; set; }

	public Meal Meal { get; set; } = null!;

	public string IngredientId { get; set; } = string.Empty;

	public Ingredient Ingredient { get; set; } = null!;

	public string UnitId { get; set; } = string.Empty;

	public Unit Unit { get; set; } = null!;

	public int Amount { get; set; }

	public int Order { get; set; }
}

internal sealed class MealIngredientConfiguration : IEntityTypeConfiguration<MealIngredient>
{
	public void Configure(EntityTypeBuilder<MealIngredient> builder)
	{
		builder.HasKey(mealIngredient => mealIngredient.Id).IsClustered(false);
		builder.HasAlternateKey(mealIngredient => new { mealIngredient.MealId, mealIngredient.IngredientId, mealIngredient.UnitId });
	}
}