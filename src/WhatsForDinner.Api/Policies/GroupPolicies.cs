using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using WhatsForDinner.Api.Dependencies.ProblemDetails;
using WhatsForDinner.Common.Authentication;
using WhatsForDinner.SqlServer;

namespace WhatsForDinner.Api.Policies;

public sealed class CommonGroupPolicy(
    ILogger<CommonGroupPolicy> logger,
    IAuthentication authentication,
    DatabaseContext databaseContext)
{
    private readonly ILogger<CommonGroupPolicy> _logger = logger;
    private readonly IAuthentication _authentication = authentication;
    private readonly DatabaseContext _databaseContext = databaseContext;

    public async Task<OneOf<Success, GroupNotFound, UserNotGroupMember>> Enforce(Guid groupId, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Enforcing common group policy...");

        _logger.LogDebug("Getting current user ID...");

        var currentUserId = _authentication.GetCurrentUserId();

        _logger.LogDebug("Got current user ID.");

        _logger.LogDebug("Getting group and current user group membership...");

        var group = await _databaseContext.Group
            .AsNoTracking()
            .Include(group => group.Memberships.Where(membership => membership.UserId == currentUserId).Take(1))
            .Select(group => new { group.Id, group.Memberships })
            .SingleOrDefaultAsync(group => group.Id == groupId, cancellationToken);

        _logger.LogDebug("Checking whether group exists...");

        if (group is null)
        {
            _logger.LogWarning("Group does not exist.");
            return new GroupNotFound(groupId);
        }

        _logger.LogDebug("Group exists.");

        _logger.LogDebug("Checking whether current user is a member of the group...");

        if (group.Memberships.Count == 0)
        {
            _logger.LogWarning("Current user is not a member of the group.");
            return new UserNotGroupMember(groupId, currentUserId);
        }

        _logger.LogInformation("Current user is a member of the group.");
        return new Success();
    }
}

#region Results

public sealed record GroupNotFound(Guid GroupId) : IProblemDetailsResult
{
    public ProblemDetails ToProblemDetails() => new NotFoundProblemDetails("Group has not been found", $"Group with ID '{GroupId}' has not been found.");
}

public sealed record UserNotGroupMember(Guid GroupId, Guid UserId) : IProblemDetailsResult
{
    public ProblemDetails ToProblemDetails() => new ForbiddenProblemDetails("You are not a member of the group", $"You are not a member of group with ID '{GroupId}'.");
}

#endregion