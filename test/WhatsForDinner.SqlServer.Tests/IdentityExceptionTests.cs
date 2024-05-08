using Microsoft.AspNetCore.Identity;
using System.Text;

namespace WhatsForDinner.SqlServer.Tests;

public sealed class IdentityExceptionTests
{
    [Fact]
    public void ThrowsOnEmptyMessage()
    {
        IReadOnlyCollection<IdentityError> identityErrors = [ExampleIdentityError];

        Assert.Throws<ArgumentNullException>(() => new IdentityException(null!, identityErrors));
        Assert.Throws<ArgumentException>(() => new IdentityException(string.Empty, identityErrors));
        Assert.Throws<ArgumentException>(() => new IdentityException("", identityErrors));

    }

    [Fact]
    public void ThrowsOnEmptyErrors()
    {
        var message = "Message";

        Assert.Throws<ArgumentNullException>(() => new IdentityException(message, null!));
        Assert.Throws<ArgumentException>(() => new IdentityException(message, []));
    }

    [Fact]
    public void TrimsMessage()
    {
        var exception = new IdentityException(" Message ", [ExampleIdentityError]);
        Assert.StartsWith("Message. Errors: '", exception.Message);
    }

    [Fact]
    public void CorrectlyCombinesErrorMessage()
    {
        var message = "Message";

        IReadOnlyCollection<IdentityError> errors = Enumerable.Range(1, 10)
            .Select(index => new IdentityError
            {
                Code = $"Code_{index}",
                Description = $"Description_{index}"
            })
            .ToArray();

        var errorsString = errors.Aggregate(new StringBuilder(), (errors, error) =>
        {
            errors.Append($"{error.Code}: {error.Description} | ");
            return errors;
        }, errors => errors.Remove(errors.Length - 3, 3).ToString());

        var correctlyCombinedErrorMessage = $"Message. Errors: '{errorsString}'.";

        var exception = new IdentityException(message, errors);

        Assert.Equal(correctlyCombinedErrorMessage, exception.Message);
    }



    #region Test data

    private static IdentityError ExampleIdentityError => new()
    {
        Code = "Code",
        Description = "Description"
    };

    #endregion
}
