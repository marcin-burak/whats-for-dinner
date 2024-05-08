using Microsoft.AspNetCore.Identity;

namespace WhatsForDinner.SqlServer.Extensions;

public static class IdentityResultExtensions
{
    public static void ThrowOnFail(this IdentityResult identityResult, string errorMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);
        if (identityResult.Succeeded is false)
        {
            throw new IdentityException(errorMessage, identityResult.Errors);
        }
    }
}
