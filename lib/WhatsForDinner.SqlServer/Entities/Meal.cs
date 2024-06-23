using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WhatsForDinner.SqlServer.Entities;

public sealed class Meal
{
	public Guid Id { get; set; }

	public string Name { get; set; } = string.Empty;



	public string EffortId { get; set; } = string.Empty;

	public Effort Effort { get; set; } = null!;



	public List<MealIngredient> Ingredients { get; set; } = [];

	public List<RecipeStep> RecipeSteps { get; set; } = [];



	public Guid GroupId { get; set; }

	public Group Group { get; set; } = null!;



	public Guid CreatedById { get; set; }

	public User CreatedBy { get; set; } = null!;



	public Guid? ModifiedById { get; set; }

	public User? ModifiedBy { get; set; }




	public DateTime CreatedAt { get; set; }

	public DateTime? ModifiedAt { get; set; }
}

public sealed class MealConfiguration : IEntityTypeConfiguration<Meal>
{
	public void Configure(EntityTypeBuilder<Meal> builder)
	{
		builder.HasKey(meal => meal.Id)
			.IsClustered(false);
	}
}