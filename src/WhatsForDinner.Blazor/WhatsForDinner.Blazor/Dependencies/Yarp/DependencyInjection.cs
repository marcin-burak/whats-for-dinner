using WhatsForDinner.Common.Extensions;
using Yarp.ReverseProxy.Configuration;

namespace WhatsForDinner.Blazor.Dependencies.Yarp;

internal static class DependencyInjection
{
    public static IServiceCollection AddYarpConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetOptionsByConvention<YarpOptions>();

        IList<RouteConfig> routes = [
            new()
            {
                RouteId = "api",
                Match = new()
                {
                    Path = "/api/{**catch-all}"
                },
                ClusterId = YarpConstants.ApiClusterId
            }
        ];

        IList<ClusterConfig> clusters = [
            new()
            {
                ClusterId = YarpConstants.ApiClusterId,
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    { "common", new() { Address = options.ApiUrl } }
                }
            }
        ];

        return services
            .AddOptionsByConvention<YarpOptions>()
            .AddReverseProxy()
            .LoadFromMemory(routes.AsReadOnly(), clusters.AsReadOnly())
            .AddTransforms<ApiTransformProvider>()
            .ConfigureHttpClient((httpClientContext, socketsHttpHandler) =>
            {
                if (options.DisableApiSslCertificateValidation && httpClientContext.ClusterId == YarpConstants.ApiClusterId)
                {
                    socketsHttpHandler.SslOptions.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                }
            })
            .Services;
    }
}
