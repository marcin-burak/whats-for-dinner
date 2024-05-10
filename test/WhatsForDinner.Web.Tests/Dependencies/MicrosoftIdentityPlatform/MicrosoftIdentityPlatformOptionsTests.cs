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
        Assert.Equal("/.auth/signin-oidc", options.CallbackPath);
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
                ClientSecret = "secret",
                Scope =
                [
                    "https://graph.microsoft.com/User.Read",
                    "https://graph.microsoft.com/Tasks.ReadWrite.Shared"
                ]
            }
        }
    };

    public static TheoryData<MicrosoftIdentityPlatformOptions> InvalidOptions => new()
    {
        {
            new()
            {
                ClientId = null!,
                ClientSecret = null!,
                Scope = null!
            }
        },
        {
            new()
            {
                ClientId = "",
                ClientSecret = "",
                Scope = []
            }
        },
        {
            new()
            {
                ClientId = $" {Guid.NewGuid()} ",
                ClientSecret = " secret ",
                Scope = [null!]
            }
        },
        {
            new()
            {
                ClientId = Guid.Empty.ToString(),
                ClientSecret = "secret",
                Scope = [""]
            }
        },
        {
            new()
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = "secret",
                Scope = [null!, null!, null!]
            }
        },
        {
            new()
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = "secret",
                Scope = ["", "", ""]
            }
        },
        {
            new()
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = "secret",
                Scope = [null!, "", null!]
            }
        },
        {
            new()
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = "secret",
                Scope = ["  ", " ", "  "]
            }
        },
        {
            new()
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = "secret",
                Scope =
                [
                    " https://graph.microsoft.com/User.Read ",
                ]
            }
        },
        {
            new()
            {
                ClientId = Guid.NewGuid().ToString(),
                ClientSecret = "secret",
                Scope =
                [
                    "https://graph.microsoft.com/User.Read",
                    "https://graph.microsoft.com/User.Read"
                ]
            }
        },
    };

    #endregion
}
