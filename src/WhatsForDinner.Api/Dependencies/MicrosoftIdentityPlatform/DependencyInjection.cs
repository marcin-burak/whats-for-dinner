using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.JsonWebTokens;
using WhatsForDinner.Common.Extensions;

namespace WhatsForDinner.Api.Dependencies.MicrosoftIdentityPlatform;

public static class DependencyInjection
{
	public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

		services
			.AddOptionsByConvention<MicrosoftIdentityPlatformOptions>()
			.AddAuthorization(options =>
			{
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.RequireClaim("oid")
					.Build();
			})
			.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApi(
			jwtBearerOptions =>
			{

			},
			microsoftIdentityOptions =>
			{
				var microsoftIdentityPlatformConfigurationSection = configuration.GetRequiredSection(nameof(MicrosoftIdentityPlatformOptions)[..^7]);
				microsoftIdentityPlatformConfigurationSection.Bind(microsoftIdentityOptions);

				var microsoftIdentityPlatformOptions = configuration.GetOptionsByConvention<MicrosoftIdentityPlatformOptions>();
				microsoftIdentityOptions.TenantId = microsoftIdentityPlatformOptions.TenantId;
				microsoftIdentityOptions.Instance = microsoftIdentityPlatformOptions.Instance;
			})
			.EnableTokenAcquisitionToCallDownstreamApi(msalOptions => { })
			.AddMicrosoftGraph()
			.AddInMemoryTokenCaches();

		return services;
	}
}
