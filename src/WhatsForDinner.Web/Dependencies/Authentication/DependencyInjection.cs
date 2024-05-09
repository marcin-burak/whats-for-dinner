using WhatsForDinner.Common.Extensions;

namespace WhatsForDinner.Web.Dependencies.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptionsByConvention<AuthenticationOptions>()
            //.AddIdentity<User, IdentityRole<int>>().AddEntityFrameworkStores<DatabaseContext>().Services
            .AddAuthorization()
            .AddAuthentication("cookie")
            .AddCookie("cookie")
                .AddMicrosoftAccount(options =>
                {
                    var authenticationOptions = configuration.GetOptionsByConvention<AuthenticationOptions>();

                    options.ClientId = authenticationOptions.Microsoft.ClientId;
                    options.ClientSecret = authenticationOptions.Microsoft.ClientSecret;

                    options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
                    options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";

                    options.CallbackPath = "/.auth/sign-in/callback";
                });

        return services;
    }
}
