using WhatsForDinner.Api.Dependencies.ApiVersioning;
using WhatsForDinner.Api.Features.Meals.Queries;

namespace WhatsForDinner.Api.Features.Meals;

internal static class Endpoints
{
	public static IEndpointRouteBuilder MapMealsEndpoints(this IEndpointRouteBuilder builder)
	{
		var group = builder.MapGroup("groups/{groupId:guid}/meals")
			.WithTags("Meals")
			.WithApiVersionSet(ApiVersionSets.Common);

		group.MapListMealsEndpoint();

		return builder;
	}
}