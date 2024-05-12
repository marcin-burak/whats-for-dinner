using FluentValidation.TestHelper;
using WhatsForDinner.Web.Dependencies.Authentication;

namespace WhatsForDinner.Web.Tests.Dependencies.MicrosoftIdentityPlatform;

public sealed class MicrosoftIdentityPlatformOptionsTests
{
    [Fact]
    public void EnsureConstantValues()
    {
        MicrosoftIdentityPlatformOptions options = new();

        Assert.Equal("consumers", options.TenantId);
        Assert.Equal("https://login.microsoftonline.com", options.Instance);
    }

    [Theory]
    [MemberData(nameof(ValidOptions))]
    public void OptionsValidationPasses(MicrosoftIdentityPlatformOptions options)
    {
        var result = new MicrosoftIdentityPlatformOptionsValidator().TestValidate(options);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(InvalidOptions))]
    public void OptionsValidationFails(MicrosoftIdentityPlatformOptions options)
    {
        var result = new MicrosoftIdentityPlatformOptionsValidator().TestValidate(options);
        result.ShouldHaveAnyValidationError();
    }



    #region Test data

    public static TheoryData<MicrosoftIdentityPlatformOptions> ValidOptions => new()
    {
        {
            new()
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = "secret"
            }
        }
    };

    public static TheoryData<MicrosoftIdentityPlatformOptions> InvalidOptions => new()
    {
        {
            new()
            {
                ClientId = null!,
                ClientSecret = null!
            }
        },
        {
            new()
            {
                ClientId = "",
                ClientSecret = ""
            }
        },
        {
            new()
            {
                ClientId = $" {Guid.NewGuid()} ",
                ClientSecret = " secret "
            }
        },
    };

    #endregion
}
