using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.JsonWebTokens;
using WhatsForDinner.Common.Extensions;

namespace WhatsForDinner.Blazor.Dependencies.MicrosoftIdentityPlatform;

internal static class DependencyInjection
{
	public static IServiceCollection AddMicrosoftIdentityPlatformConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

		return services
			.AddOptionsByConvention<MicrosoftIdentityPlatformOptions>()
			.AddAuthorization(authorizationOptions =>
			{
				authorizationOptions.FallbackPolicy = new AuthorizationPolicyBuilder(OpenIdConnectDefaults.AuthenticationScheme)
					.RequireAuthenticatedUser()
					.RequireClaim("oid")
					.Build();
			})
			.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApp(
			microsoftIdentityOptions =>
			{
				var configurationSection = configuration.GetRequiredSection("MicrosoftIdentityPlatform");
				configurationSection.Bind(microsoftIdentityOptions);

				var microsoftIdentityPlatformOptions = configuration.GetOptionsByConvention<MicrosoftIdentityPlatformOptions>();
				microsoftIdentityOptions.TenantId = microsoftIdentityPlatformOptions.TenantId;
				microsoftIdentityOptions.Instance = microsoftIdentityPlatformOptions.Instance;

				microsoftIdentityOptions.Scope.Clear();
				microsoftIdentityOptions.Scope.Add("openid");
				microsoftIdentityOptions.Scope.Add("offline_access");

				foreach (var scope in microsoftIdentityPlatformOptions.ApiScopes)
				{
					microsoftIdentityOptions.Scope.Add(scope);
				}

				microsoftIdentityOptions.CallbackPath = microsoftIdentityPlatformOptions.CallbackPath;
			},
			cookieOptions =>
			{
				//https://joonasw.net/view/aad-single-sign-out-in-asp-net-core
				cookieOptions.Cookie.Name = "Authentication";
				cookieOptions.Cookie.SameSite = SameSiteMode.None;
			})
			.EnableTokenAcquisitionToCallDownstreamApi()
			.AddInMemoryTokenCaches()
			.Services;
	}
}
