using FluentValidation;
using WhatsForDinner.Common.FluentValidation;
using static WhatsForDinner.Web.Dependencies.Authentication.AuthenticationOptions;

namespace WhatsForDinner.Web.Dependencies.Authentication;

public sealed class AuthenticationOptions
{
    public MicrosoftAuthenticationOptions Microsoft { get; set; } = new();

    public sealed class MicrosoftAuthenticationOptions
    {
        public string ClientId { get; set; } = string.Empty;

        public string ClientSecret { get; set; } = string.Empty;
    }
}

public sealed class AuthenticationOptionsValidator : AbstractValidator<AuthenticationOptions>
{
    public AuthenticationOptionsValidator()
    {
        RuleFor(options => options.Microsoft)
            .NotEmpty()
            .SetValidator(new MicrosoftAuthenticationOptionsValidator());
    }
}

public sealed class MicrosoftAuthenticationOptionsValidator : AbstractValidator<MicrosoftAuthenticationOptions>
{
    public MicrosoftAuthenticationOptionsValidator()
    {
        RuleFor(options => options.ClientId)
            .NotEmpty()
            .Trimmed()
            .NonEmptyGuid();

        RuleFor(options => options.ClientSecret)
            .NotEmpty()
            .Trimmed();
    }
}