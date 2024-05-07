using WhatsForDinner.Common.ApplicationInsights;
using WhatsForDinner.SqlServer;
using WhatsForDinner.Web.Dependencies.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsOptions()
    .AddOpenApiConfiguration(builder.Configuration)
    .AddSqlServerConfiguration(builder.Configuration);

var app = builder.Build();

app
    .UseHttpsRedirection()
    .UseOpenApiWhenEnabled();

await DatabaseMigrations.RunMigrationsIfConfigured(app.Services, CancellationToken.None);

app.Run();