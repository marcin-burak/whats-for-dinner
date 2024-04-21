using WhatsForDinner.Common.ApplicationInsights;
using WhatsForDinner.Web.Dependencies.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsOptions()
    .AddOpenApiConfiguration(builder.Configuration);

var app = builder.Build();

app
    .UseHttpsRedirection()
    .UseOpenApiWhenEnabled();

app.Run();