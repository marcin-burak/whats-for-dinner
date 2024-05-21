using Microsoft.Extensions.DependencyInjection;

namespace WhatsForDinner.Common.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationContext(this IServiceCollection services) => services
        .AddScoped<IAuthentication, Authentication>();
}
