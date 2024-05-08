using Microsoft.AspNetCore.Identity;
using WhatsForDinner.SqlServer.Extensions;

namespace WhatsForDinner.SqlServer.Tests.Extensions;

public sealed class IdentityResultExtensionsTests
{
    [Fact]
    public void ThrowOnFail_ThrowsOnEmptyMessage()
    {
        var result = IdentityResult.Success;

        Assert.Throws<ArgumentNullException>(() => result.ThrowOnFail(null!));
        Assert.Throws<ArgumentException>(() => result.ThrowOnFail(string.Empty));
        Assert.Throws<ArgumentException>(() => result.ThrowOnFail(" "));

    }

    [Fact]
    public void ThrowOnFail_ThrowsOnFail()
    {
        var failedResult = IdentityResult.Failed(new IdentityError { Code = "Code", Description = "Description" });
        Assert.Throws<IdentityException>(() => failedResult.ThrowOnFail("Error message"));
    }

    [Fact]
    public void ThrowOnFail_ContinuesOnSuccess()
    {
        var successResult = IdentityResult.Success;
        successResult.ThrowOnFail("Error message");
    }
}
