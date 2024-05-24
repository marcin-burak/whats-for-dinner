using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Security.Claims;
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
    .AddFluentUIComponents()
    .AddAntiforgery(options =>
    {
        options.Cookie.Name = "Antiforgery";
    });

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

app.MapGet("/.auth/signout", async (HttpContext httpContext, IOptions<MicrosoftIdentityPlatformOptions> msalOptions, CancellationToken cancellationToken) =>
{
    // Clear token cache also?
    var logoutHint = httpContext.User.FindFirstValue("login_hint");
    await httpContext.SignOutAsync();
    return TypedResults.Redirect($"https://login.microsoftonline.com/consumers/oauth2/v2.0/logout?logout_hint={logoutHint}");
});

app.MapGet("/.auth/signout-oidc", async (HttpContext httpContext, CancellationToken cancellationToken) =>
{
    await httpContext.SignOutAsync();
    return TypedResults.Ok();
}).AllowAnonymous();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(WhatsForDinner.Blazor.Client._Imports).Assembly);

app.MapReverseProxy();

app.Run();
