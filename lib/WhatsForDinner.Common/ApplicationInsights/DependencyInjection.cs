using Microsoft.Extensions.DependencyInjection;
using WhatsForDinner.Common.Extensions;

namespace WhatsForDinner.Common.ApplicationInsights;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationInsightsOptions(this IServiceCollection services) => services
        .AddOptionsByConvention<ApplicationInsightsOptions>();
}
