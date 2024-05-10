using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.JsonWebTokens;
using WhatsForDinner.Common.Extensions;

namespace WhatsForDinner.Web.Dependencies.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services
            .AddOptionsByConvention<MicrosoftIdentityPlatformOptions>()
            .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(
                microsoftIdentityOptions =>
                {
                    var microsoftIdentityPlatformConfigurationSection = configuration.GetRequiredSection(nameof(MicrosoftIdentityPlatformOptions)[..^7]);
                    microsoftIdentityPlatformConfigurationSection.Bind(microsoftIdentityOptions);

                    var microsoftIdentityPlatformOptions = configuration.GetOptionsByConvention<MicrosoftIdentityPlatformOptions>();
                    microsoftIdentityOptions.TenantId = microsoftIdentityPlatformOptions.TenantId;
                    microsoftIdentityOptions.Instance = microsoftIdentityPlatformOptions.Instance;
                    microsoftIdentityOptions.CallbackPath = microsoftIdentityPlatformOptions.CallbackPath;
                },
                authenticationCookieOptions =>
                {
                    authenticationCookieOptions.Cookie.Name = "Authentication";
                })
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddMicrosoftGraph()
                .AddInMemoryTokenCaches()
                .Services
            .AddAuthorization();

        return services;
    }
}
