using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WhatsForDinner.SqlServer.Entities;

public sealed class RecipeStep
{
	public Guid Id { get; set; }

	public byte Number { get; set; }

	public string Content { get; set; } = string.Empty;



	public Guid MealId { get; set; }

	public Meal Meal { get; set; } = null!;
}

internal sealed class RecipeStepConfiguration : IEntityTypeConfiguration<RecipeStep>
{
	public void Configure(EntityTypeBuilder<RecipeStep> builder)
	{
		builder.HasKey(recipeStep => recipeStep.Id)
			.IsClustered(false);

		builder.Property(recipeStep => recipeStep.Content)
			.HasMaxLength(500);
	}
}