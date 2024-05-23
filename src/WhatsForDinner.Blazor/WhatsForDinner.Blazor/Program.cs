using Microsoft.FluentUI.AspNetCore.Components;
using WhatsForDinner.Blazor.Components;
using WhatsForDinner.Blazor.Dependencies.Api;
using WhatsForDinner.Blazor.Dependencies.MicrosoftIdentityPlatform;
using WhatsForDinner.Blazor.Dependencies.Yarp;
using WhatsForDinner.Common.ApplicationInsights;
using WhatsForDinner.Common.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveWebAssemblyComponents().Services
    .AddApplicationInsightsOptions(services => services.AddApplicationInsightsTelemetry())
    .AddAuthenticationContext()
    .AddMicrosoftIdentityPlatformConfiguration(builder.Configuration)
    .AddYarpConfiguration(builder.Configuration)
    .AddApiHttpClient()
    .AddFluentUIComponents();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(WhatsForDinner.Blazor.Client._Imports).Assembly);

app.MapReverseProxy();

app.Run();
