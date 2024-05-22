using FluentValidation;
using WhatsForDinner.Common.FluentValidation;
using WhatsForDinner.Common.MicrosoftIdentityPlatform;

namespace WhatsForDinner.Blazor.Dependencies.MicrosoftIdentityPlatform;

public sealed class MicrosoftIdentityPlatformOptions : MicrosoftIdentityPlatformBaseOptions
{
    public string ApiClientId { get; set; } = string.Empty;

    public string CallbackPath { get; set; } = "/.auth/signin-oidc";

    public IReadOnlyCollection<string> ApiScopes => [$"api://{ApiClientId}/user_impersonation"];
}

public sealed class MicrosoftIdentityPlatformOptionsValidator : MicrosoftIdentityPlatformBaseOptionsValidator<MicrosoftIdentityPlatformOptions>
{
    public MicrosoftIdentityPlatformOptionsValidator() : base()
    {
        RuleFor(options => options.ApiClientId)
            .NotEmpty()
            .Trimmed()
            .NonEmptyGuid();

        RuleFor(options => options.CallbackPath)
            .NotEmpty()
            .Trimmed();
    }
}