using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using WhatsForDinner.Web.Dependencies.MicrosoftIdentityPlatform;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace WhatsForDinner.Web.Dependencies.Yarp;

public sealed class ApiTransformProvider(IOptions<MicrosoftIdentityPlatformOptions> msalOptions) : ITransformProvider
{
    private readonly MicrosoftIdentityPlatformOptions _msalOptions = msalOptions.Value;

    public void Apply(TransformBuilderContext context)
    {
        if (string.IsNullOrWhiteSpace(context.Cluster?.ClusterId))
        {
            return;
        }

        if (context.Cluster.ClusterId == YarpConstants.ApiClusterId)
        {
            context.AddRequestTransform(async requestTransformContext =>
            {
                var msal = requestTransformContext.HttpContext.RequestServices.GetRequiredService<ITokenAcquisition>();

                var currentUserApiAccessToken = await msal.GetAccessTokenForUserAsync(
                    scopes: _msalOptions.ApiScopes,
                    authenticationScheme: OpenIdConnectDefaults.AuthenticationScheme,
                    tokenAcquisitionOptions: new() { CancellationToken = requestTransformContext.CancellationToken }
                );

                requestTransformContext.ProxyRequest.Headers.Authorization = new(JwtBearerDefaults.AuthenticationScheme, currentUserApiAccessToken);
            });
        }
    }

    public void ValidateCluster(TransformClusterValidationContext context) { }

    public void ValidateRoute(TransformRouteValidationContext context) { }
}
