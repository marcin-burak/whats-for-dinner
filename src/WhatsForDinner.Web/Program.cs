using Microsoft.AspNetCore.Authentication;
using WhatsForDinner.Common.ApplicationInsights;
using WhatsForDinner.SqlServer;
using WhatsForDinner.Web.Dependencies.Authentication;
using WhatsForDinner.Web.Dependencies.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsOptions()
    .AddOpenApiConfiguration(builder.Configuration)
    .AddSqlServerConfiguration(builder.Configuration)
    .AddAuthenticationConfiguration(builder.Configuration);

var app = builder.Build();

app
    .UseHttpsRedirection()
    .UseOpenApiWhenEnabled()
    .UseAuthentication()
    .UseAuthorization();

app.MapGet("/.auth/sign-in", () =>
{
    return TypedResults.Challenge(new AuthenticationProperties { RedirectUri = "https://localhost:5000" }, ["Microsoft"]);
});

await DatabaseMigrations.RunMigrationsIfConfigured(app.Services, CancellationToken.None);

app.Run();