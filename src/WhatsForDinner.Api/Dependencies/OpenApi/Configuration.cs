﻿using FluentValidation;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WhatsForDinner.Api.Dependencies.MicrosoftIdentityPlatform;
using WhatsForDinner.Common.Extensions;
using WhatsForDinner.Common.FluentValidation;

namespace WhatsForDinner.Api.Dependencies.OpenApi;

public sealed class OpenApiOptions
{
	public bool Enabled { get; set; }

	public string? ClientId { get; set; }
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

		When(options => options.Enabled, () =>
		{
			RuleFor(options => options.ClientId!)
				.NotEmpty()
				.Trimmed()
				.NonEmptyGuid()
				.WithName("Open API client ID");
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
				.AddTransient<IConfigureOptions<SwaggerGenOptions>, ApiVersioning>()
				.AddSwaggerGen(options =>
				{
					options.OperationFilter<SwaggerDefaultValues>();

					const string securityDefinitionKey = "Microsoft Identity Platform";
					var microsoftIdentityPlatformOptions = configuration.GetOptionsByConvention<MicrosoftIdentityPlatformOptions>();

					options.AddSecurityDefinition(securityDefinitionKey, new()
					{
						Type = SecuritySchemeType.OAuth2,
						Flows = new()
						{
							AuthorizationCode = new()
							{
								AuthorizationUrl = new Uri("https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize"),
								TokenUrl = new Uri("https://login.microsoftonline.com/consumers/oauth2/v2.0/token"),
								Scopes =
								{
									{ $"api://{microsoftIdentityPlatformOptions.ClientId}/user_impersonation", "User impersonation" }
								}
							}
						}
					});

					options.AddSecurityRequirement(new()
					{
						{
							new()
							{
								Reference = new()
								{
									Id = securityDefinitionKey,
									Type = ReferenceType.SecurityScheme
								}
						},
							[]
						}
					});
				});
		}

		return services;
	}

	public static IApplicationBuilder UseOpenApiWhenEnabled(this IApplicationBuilder app)
	{
		var application = app as WebApplication ?? throw new InvalidOperationException($"Failed to cast '{nameof(IApplicationBuilder)}' to '{nameof(WebApplication)}'.");
		var openApiOptions = application.Services.GetRequiredService<IOptions<OpenApiOptions>>().Value;
		if (openApiOptions.Enabled)
		{
			application
				.UseSwagger()
				.UseSwaggerUI(options =>
				{
					options.OAuthClientId(openApiOptions.ClientId);
					options.OAuthUsePkce();
					options.EnablePersistAuthorization();
					options.EnableTryItOutByDefault();

					foreach (var description in application.DescribeApiVersions())
					{
						options.SwaggerEndpoint(
							$"/swagger/{description.GroupName}/swagger.json",
							description.GroupName);
					}
				});
		}

		return application;
	}
}