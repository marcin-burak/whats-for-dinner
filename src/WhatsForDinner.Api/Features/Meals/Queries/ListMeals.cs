using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WhatsForDinner.Api.Policies;
using WhatsForDinner.Common.Authentication;
using WhatsForDinner.SqlServer;

namespace WhatsForDinner.Api.Features.Meals.Queries;

public static class ListMeals
{
	public static IEndpointRouteBuilder MapListMealsEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapGet("", async (ISender sender, Guid groupId, CancellationToken cancellationToken) =>
		{
			var result = await sender.Send(new ListMealsQuery(groupId), cancellationToken);
			return result.Match<IResult>(
				response => TypedResults.Ok(response),
				groupNotFound => TypedResults.Problem(groupNotFound.ToProblemDetails()),
				userNotGroupMember => TypedResults.Problem(userNotGroupMember.ToProblemDetails())
			);
		})
		.WithOpenApi(options =>
		{
			options.OperationId = "list-group-meals";
			options.Summary = "List group meals.";
			options.Description = "List group meals.";
			return options;
		})
		.Produces<ListMealsResponse>()
		.Produces(401)
		.Produces(403)
		.Produces(404)
		.Produces(500);

		return builder;
	}

	#region Contracts

	public sealed record ListMealsResponse
	{
		public required IReadOnlyCollection<ListMealsResponseMeal> Meals { get; set; }

		public sealed record ListMealsResponseMeal
		{
			public required Guid Id { get; init; }

			public required string Name { get; init; }
		}
	}

	#endregion

	#region CQRS

	public sealed record ListMealsQuery(Guid GroupId) : IRequest<OneOf<ListMealsResponse, GroupNotFound, UserNotGroupMember>>;

	public sealed class ListMealsQueryHandler(
		ILogger<ListMealsQueryHandler> logger,
		CommonGroupPolicy commonGroupPolicy,
		IAuthentication authentication,
		DatabaseContext databaseContext) : IRequestHandler<ListMealsQuery, OneOf<ListMealsResponse, GroupNotFound, UserNotGroupMember>>
	{
		private readonly ILogger<ListMealsQueryHandler> _logger = logger;
		private readonly CommonGroupPolicy _commonGroupPolicy = commonGroupPolicy;
		private readonly IAuthentication _authentication = authentication;
		private readonly DatabaseContext _databaseContext = databaseContext;

		public async Task<OneOf<ListMealsResponse, GroupNotFound, UserNotGroupMember>> Handle(ListMealsQuery query, CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(query);

			using var groupLogScope = _logger.BeginScope("Listing meals for group with ID '{GroupId}.'", query.GroupId);

			var commonGroupPolicyResult = await _commonGroupPolicy.Enforce(query.GroupId, cancellationToken);
			var commonGroupPolicyFailResult = commonGroupPolicyResult.Match<OneOf<ListMealsResponse, GroupNotFound, UserNotGroupMember>?>(
				success => null,
				groupNotFound => groupNotFound,
				userNotGroupMember => userNotGroupMember
			);

			if (commonGroupPolicyFailResult.HasValue)
			{
				return commonGroupPolicyFailResult.Value;
			}



			_logger.LogDebug("Fetching all group meals from the database...");

			var meals = await _databaseContext.Meal
				.AsNoTracking()
				.Where(meal => meal.GroupId == query.GroupId)
				.Select(meal => new { meal.Id, meal.Name })
				.OrderBy(meal => meal.Name)
				.ToArrayAsync(cancellationToken);

			_logger.LogDebug("Fetched all group meals from the database.");



			_logger.LogDebug("Mapping meal database entities to response DTOs...");

			ListMealsResponse response = new()
			{
				Meals = meals.Select(meal => new ListMealsResponse.ListMealsResponseMeal
				{
					Id = meal.Id,
					Name = meal.Name
				}).ToArray()
			};

			_logger.LogDebug("Mapped meal database entities to response DTOs.");



			_logger.LogInformation("Listed all '{MealCount}' meals for group.", response.Meals.Count);

			return response;
		}
	}

	#endregion
}