using FluentValidation;
using Microsoft.Extensions.Options;
using WhatsForDinner.Common.Extensions;

namespace WhatsForDinner.Web.Dependencies.OpenApi;

public sealed class OpenApiOptions
{
    public bool Enabled { get; set; }
}

public sealed class OpenApiOptionsValidator : AbstractValidator<OpenApiOptions>
{
    public OpenApiOptionsValidator(IHostEnvironment environment)
    {
        When(_ => environment.IsDevelopment() is false, () =>
        {
            RuleFor(options => options.Enabled)
                .Equal(false)
                    .WithMessage("Open API features have to be disabled in non-development environments.");
        });
    }
}

public static class Configuration
{
    public static IServiceCollection AddOpenApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsByConvention<OpenApiOptions>();

        var openApiOptions = configuration.GetOptionsByConvention<OpenApiOptions>();
        if (openApiOptions.Enabled)
        {
            services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();
        }

        return services;
    }

    public static IApplicationBuilder UseOpenApiWhenEnabled(this IApplicationBuilder application)
    {
        var openApiOptions = application.ApplicationServices.GetRequiredService<IOptions<OpenApiOptions>>().Value;
        if (openApiOptions.Enabled)
        {
            application
                .UseSwagger()
                .UseSwaggerUI();
        }

        return application;
    }
}