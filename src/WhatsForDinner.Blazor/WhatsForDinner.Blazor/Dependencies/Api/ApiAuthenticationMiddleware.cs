using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using WhatsForDinner.Blazor.Dependencies.MicrosoftIdentityPlatform;

namespace WhatsForDinner.Blazor.Dependencies.Api;

public sealed class ApiAuthenticationMiddleware(IOptions<MicrosoftIdentityPlatformOptions> options, ITokenAcquisition msal) : DelegatingHandler
{
    private readonly MicrosoftIdentityPlatformOptions _options = options.Value;
    private readonly ITokenAcquisition _msal = msal;


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var userApiAccessToken = await _msal.GetAccessTokenForUserAsync(
            scopes: _options.ApiScopes,
            authenticationScheme: OpenIdConnectDefaults.AuthenticationScheme,
            tokenAcquisitionOptions: new()
            {
                CancellationToken = cancellationToken
            }
        );

        request.Headers.Authorization = new(JwtBearerDefaults.AuthenticationScheme, userApiAccessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
