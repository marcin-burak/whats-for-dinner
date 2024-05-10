using FluentValidation;
using WhatsForDinner.Common.FluentValidation;

namespace WhatsForDinner.Web.Dependencies.Authentication;

public sealed class MicrosoftIdentityPlatformOptions
{
    public string TenantId { get; } = "consumers";

    public string Instance { get; } = "https://login.microsoftonline.com";

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string CallbackPath { get; } = "/.auth/signin-oidc";

    public IReadOnlyCollection<string> Scope { get; set; } = [];
}

public sealed class MicrosoftIdentityPlatformOptionsValidator : AbstractValidator<MicrosoftIdentityPlatformOptions>
{
    public MicrosoftIdentityPlatformOptionsValidator()
    {
        RuleFor(options => options.TenantId)
            .NotEmpty()
            .Trimmed()
            .Equal("consumers");

        RuleFor(options => options.Instance)
            .NotEmpty()
            .Trimmed()
            .Equal("https://login.microsoftonline.com");

        RuleFor(options => options.ClientId)
            .NotEmpty()
            .Trimmed()
            .NonEmptyGuid();

        RuleFor(options => options.ClientSecret)
            .NotEmpty()
            .Trimmed();

        RuleFor(options => options.CallbackPath)
            .NotEmpty()
            .Trimmed()
            .Equal("/.auth/signin-oidc");

        RuleFor(options => options.Scope)
            .NotEmpty()
            .DistinctItems();

        RuleForEach(options => options.Scope)
            .NotEmpty()
            .Trimmed()
            .AbsoluteHttpsUri();
    }
}