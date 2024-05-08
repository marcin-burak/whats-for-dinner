using Microsoft.AspNetCore.Identity;
using System.Text;

namespace WhatsForDinner.SqlServer;

public sealed class IdentityException(string message, IEnumerable<IdentityError> identityErrors) : Exception(GetErrorMessage(message, identityErrors))
{
    private static string GetErrorMessage(string message, IEnumerable<IdentityError> identityErrors)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentNullException.ThrowIfNull(identityErrors);
        if (identityErrors.Any() is false)
        {
            throw new ArgumentException("Collection must not be empty.", nameof(identityErrors));
        }

        var combinedMessage = identityErrors.Aggregate(new StringBuilder($"{message.Trim()}. Errors: '"), (errors, error) =>
        {
            errors.Append($"{error.Code}: {error.Description} | ");
            return errors;
        },
            errors => errors.Remove(errors.Length - 3, 3).Append("'.")
        );

        return combinedMessage.ToString();
    }
}
