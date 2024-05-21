using WhatsForDinner.Common.ApplicationInsights;
using WhatsForDinner.Common.Authentication;
using WhatsForDinner.Web.Dependencies.MicrosoftIdentityPlatform;
using WhatsForDinner.Web.Dependencies.Yarp;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationInsightsOptions(services => services.AddApplicationInsightsTelemetry())
    .AddAuthenticationContext()
    .AddMicrosoftIdentityPlatformConfiguration(builder.Configuration)
    .AddYarpConfiguration(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();