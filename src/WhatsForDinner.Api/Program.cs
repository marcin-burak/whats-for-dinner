using System.Reflection;
using WhatsForDinner.Api.Dependencies.MicrosoftIdentityPlatform;
using WhatsForDinner.Api.Dependencies.OpenApi;
using WhatsForDinner.Api.Features.Meals.Queries;
using WhatsForDinner.Api.Features.Users.Queries;
using WhatsForDinner.Api.Policies;
using WhatsForDinner.Common.ApplicationInsights;
using WhatsForDinner.Common.Authentication;
using WhatsForDinner.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsOptions(services => services.AddApplicationInsightsTelemetry())
    .AddOpenApiConfiguration(builder.Configuration)
    .AddSqlServerConfiguration(builder.Configuration)
    .AddAuthenticationConfiguration(builder.Configuration)
    .AddMediatR(options => options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
    .AddAuthenticationContext()
    .AddPolicies();

var app = builder.Build();

app
    .UseHttpsRedirection()
    .UseOpenApiWhenEnabled()
    .UseAuthentication()
    .UseAuthorization();

app
    .MapGetMeEndpoint()
    .MapListMealsEndpoint();

await DatabaseMigrations.RunMigrationsIfConfigured(app.Services, CancellationToken.None);

app.Run();