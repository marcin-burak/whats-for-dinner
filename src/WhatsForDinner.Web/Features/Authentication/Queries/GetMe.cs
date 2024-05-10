using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using WhatsForDinner.SqlServer;
using WhatsForDinner.Web.Features.Common.Authentication;

namespace WhatsForDinner.Web.Features.Authentication.Queries;

public static class GetMe
{
    public static IEndpointRouteBuilder MapGetMeEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/v1/me", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new GetMeQuery(), cancellationToken);
            return result.Match<IResult>(
                response => TypedResults.Ok(response),
                userNotFound => TypedResults.NotFound()
            );
        }).RequireAuthorization();

        return builder;
    }

    #region Contracts

    public sealed record GetMeResponse
    {
        public required Guid Id { get; init; }

        public required string Email { get; init; } = string.Empty;

        public required string DisplayName { get; init; } = string.Empty;

        public required IReadOnlyCollection<GetMeResponseGroup> Groups { get; init; } = [];

        public sealed record GetMeResponseGroup
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

    public sealed class GetMeQuery : IRequest<OneOf<GetMeResponse, UserNotFound>> { }

    public sealed class GetMeQueryHandler(
        ILogger<GetMeQueryHandler> logger,
        IAuthentication authentication,
        DatabaseContext databaseContext) : IRequestHandler<GetMeQuery, OneOf<GetMeResponse, UserNotFound>>
    {
        private readonly ILogger<GetMeQueryHandler> _logger = logger;
        private readonly IAuthentication _authentication = authentication;
        private readonly DatabaseContext _databaseContext = databaseContext;

        public async Task<OneOf<GetMeResponse, UserNotFound>> Handle(GetMeQuery query, CancellationToken cancellationToken)
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

            return new GetMeResponse
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.FullName,
                Groups = user.Groups.Select(group => new GetMeResponse.GetMeResponseGroup
                {
                    Id = group.Id,
                    Name = group.Name
                }).ToArray()
            };
        }
    }

    #endregion
}