using FluentValidation;
using WhatsForDinner.Common.FluentValidation;

namespace WhatsForDinner.Blazor.Dependencies.Yarp;

public sealed class YarpOptions
{
    public string ApiUrl { get; set; } = string.Empty;

    public bool DisableApiSslCertificateValidation { get; set; }
}

public sealed class YarpOptionsValidator : AbstractValidator<YarpOptions>
{
    public YarpOptionsValidator(IHostEnvironment environment)
    {
        RuleFor(options => options.ApiUrl)
            .NotEmpty()
            .Trimmed()
            .AbsoluteHttpsUri();

        When(_ => environment.IsDevelopment(), () =>
        {
            RuleFor(options => options.DisableApiSslCertificateValidation)
                .Equal(true).WithMessage("API SSL certificate validation has to be enabled in development enviorment.");
        });

        When(_ => environment.IsDevelopment() is false, () =>
        {
            RuleFor(options => options.DisableApiSslCertificateValidation)
                .Equal(false).WithMessage("API SSL certificate validation has to be disabled in non-development enviorments.");
        });
    }
}