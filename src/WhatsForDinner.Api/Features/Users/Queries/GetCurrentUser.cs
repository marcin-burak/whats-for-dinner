using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WhatsForDinner.Common.Authentication;
using WhatsForDinner.SqlServer;

namespace WhatsForDinner.Api.Features.Users.Queries;

public static class GetCurrentUser
{
	public static IEndpointRouteBuilder MapGetCurrentUserEndpoint(this IEndpointRouteBuilder builder)
	{
		builder.MapGet("me", async (ISender sender, CancellationToken cancellationToken) =>
		{
			var result = await sender.Send(new GetCurrentUserQuery(), cancellationToken);
			return result.Match<IResult>(
				response => TypedResults.Ok(response),
				userNotFound => TypedResults.NotFound()
			);
		})
		.WithOpenApi(options =>
		{
			options.OperationId = "get-current-user";
			options.Summary = "Get current user data.";
			options.Description = "Get the currently signed in user data.";
			return options;
		})
		.Produces<GetCurrentUserResponse>()
		.Produces(401)
		.Produces(404)
		.Produces(500);

		return builder;
	}

	#region Contracts

	public sealed record GetCurrentUserResponse
	{
		public required Guid Id { get; init; }

		public required string Email { get; init; } = string.Empty;

		public required string DisplayName { get; init; } = string.Empty;

		public required string Initials { get; init; } = string.Empty;

		public required GetCurrentUserResponseGroup DefaultGroup { get; set; }

		public required IReadOnlyCollection<GetCurrentUserResponseGroup> Groups { get; init; } = [];

		public sealed record GetCurrentUserResponseGroup
		{
			public required Guid Id { get; init; }

			public required string Name { get; init; } = string.Empty;
		}
	}

	#endregion

	#region Results

	public sealed class UserNotFound { }

	#endregion

	#region CQRS

	public sealed class GetCurrentUserQuery : IRequest<OneOf<GetCurrentUserResponse, UserNotFound>> { }

	public sealed class GetCurrentUserQueryHandler(
		ILogger<GetCurrentUserQueryHandler> logger,
		IAuthentication authentication,
		DatabaseContext databaseContext) : IRequestHandler<GetCurrentUserQuery, OneOf<GetCurrentUserResponse, UserNotFound>>
	{
		private readonly ILogger<GetCurrentUserQueryHandler> _logger = logger;
		private readonly IAuthentication _authentication = authentication;
		private readonly DatabaseContext _databaseContext = databaseContext;

		public async Task<OneOf<GetCurrentUserResponse, UserNotFound>> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
		{
			var currentUserId = _authentication.GetCurrentUserId();

			_logger.LogInformation("Trying to get current user data from the database...");

			var user = await _databaseContext.User
				.AsNoTracking()
				.Include(user => user.Groups)
				.SingleOrDefaultAsync(user => user.Id == currentUserId, cancellationToken);

			if (user is null)
			{
				_logger.LogWarning("Failed to get current user data.");
				return new UserNotFound();
			}

			_logger.LogInformation("Got current user data with ID '{CurrentUserId}'.", user.Id);

			var groups = user.Groups.Select(group => new GetCurrentUserResponse.GetCurrentUserResponseGroup
			{
				Id = group.Id,
				Name = group.Name
			}).ToArray();

			return new GetCurrentUserResponse
			{
				Id = user.Id,
				Email = user.Email,
				DisplayName = user.FullName,
				Initials = $"{user.FirstName.First()}{user.LastName.First()}",
				DefaultGroup = groups.First(),
				Groups = groups
			};
		}
	}

	#endregion
}