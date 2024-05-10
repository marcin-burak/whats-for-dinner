using System.Reflection;
using WhatsForDinner.Common.ApplicationInsights;
using WhatsForDinner.SqlServer;
using WhatsForDinner.Web.Dependencies.Authentication;
using WhatsForDinner.Web.Dependencies.OpenApi;
using WhatsForDinner.Web.Features.Authentication.Queries;
using WhatsForDinner.Web.Features.Common.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsOptions()
    .AddOpenApiConfiguration(builder.Configuration)
    .AddSqlServerConfiguration(builder.Configuration)
    .AddAuthenticationConfiguration(builder.Configuration)
    .AddMediatR(options => options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
    .AddScoped<IAuthentication, Authentication>();

var app = builder.Build();

app
    .UseHttpsRedirection()
    .UseOpenApiWhenEnabled()
    .UseAuthentication()
    .UseAuthorization();

app.MapGetMeEndpoint();

await DatabaseMigrations.RunMigrationsIfConfigured(app.Services, CancellationToken.None);

app.Run();