namespace WhatsForDinner.Web.Features.Common.Authentication;

public interface IAuthentication
{
    Guid GetCurrentUserId();
}

public sealed class Authentication : IAuthentication
{
    private readonly Guid? _currentUserId;

    public Authentication(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext?.User is null)
        {
            return;
        }

        var oidClaimStringValue = httpContextAccessor.HttpContext.User.FindFirst("oid")?.Value;
        if (Guid.TryParse(oidClaimStringValue, out var oidClaimValue))
        {
            _currentUserId = oidClaimValue;
        }
    }

    public Guid GetCurrentUserId()
    {
        if (_currentUserId.HasValue is false)
        {
            throw new InvalidOperationException("Failed to obtain current user ID.");
        }

        return _currentUserId.Value;
    }
}
