using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace WhatsForDinner.Common.ApplicationInsights;

public sealed class DefaultTelemetryInitializer(IOptions<ApplicationInsightsOptions> options, IHttpContextAccessor httpContextAccessor) : ITelemetryInitializer
{
    private readonly ApplicationInsightsOptions _options = options.Value;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.Cloud.RoleName = _options.CloudRoleName;

        var currentUserId = _httpContextAccessor.HttpContext?.User?.Claims?.SingleOrDefault(claim => claim.Type == "oid")?.Value;
        if (string.IsNullOrWhiteSpace(currentUserId) is false)
        {
            telemetry.Context.User.AuthenticatedUserId = currentUserId;
        }
    }
}
