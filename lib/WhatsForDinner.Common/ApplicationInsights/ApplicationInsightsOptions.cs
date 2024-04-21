using FluentValidation;
using Microsoft.Extensions.Hosting;
using WhatsForDinner.Common.FluentValidation;

namespace WhatsForDinner.Common.ApplicationInsights;

public sealed class ApplicationInsightsOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public bool DeveloperMode { get; set; }
}

public sealed class ApplicationInsightsOptionsValidator : AbstractValidator<ApplicationInsightsOptions>
{
    public ApplicationInsightsOptionsValidator(IHostEnvironment environment)
    {
        RuleFor(options => options.ConnectionString)
            .NotEmpty()
            .Trimmed()
            .Must(connectionString => string.IsNullOrWhiteSpace(connectionString) && connectionString.Contains("IngestionKey="))
                .WithMessage("{PropertyName} has to contain ingestion key.")
            .WithName("Application Insights connection string");

        When(_ => environment.IsDevelopment() is false, () =>
        {
            RuleFor(options => options.DeveloperMode)
                .Equal(false)
                    .WithMessage("{PropertyName} has to be disabled in non-development environments.")
                .WithName("Application Insights developer mode");
        });
    }
}