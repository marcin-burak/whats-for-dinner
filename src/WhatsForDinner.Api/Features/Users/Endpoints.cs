using WhatsForDinner.Api.Features.Users.Queries;

namespace WhatsForDinner.Api.Features.Users;

internal static class Endpoints
{
	public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder builder)
	{
		var group = builder.MapGroup("users")
			.WithTags("users");

		group.MapGetCurrentUserEndpoint();

		return builder;
	}
}
