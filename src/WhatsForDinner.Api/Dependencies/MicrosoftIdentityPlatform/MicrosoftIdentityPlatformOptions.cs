using WhatsForDinner.Common.MicrosoftIdentityPlatform;

namespace WhatsForDinner.Api.Dependencies.MicrosoftIdentityPlatform;

public sealed class MicrosoftIdentityPlatformOptions : MicrosoftIdentityPlatformBaseOptions { }

public sealed class MicrosoftIdentityPlatformOptionsValidator : MicrosoftIdentityPlatformBaseOptionsValidator<MicrosoftIdentityPlatformOptions>
{
	public MicrosoftIdentityPlatformOptionsValidator() : base() { }
}