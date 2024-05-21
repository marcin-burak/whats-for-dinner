using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using WhatsForDinner.Common.Extensions;

namespace WhatsForDinner.Common.ApplicationInsights;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationInsightsOptions(this IServiceCollection services, Action<IServiceCollection> configure)
    {
        services
            .AddOptionsByConvention<ApplicationInsightsOptions>()
            .AddSingleton<ITelemetryInitializer, DefaultTelemetryInitializer>();

        configure(services);

        return services;
    }
}
