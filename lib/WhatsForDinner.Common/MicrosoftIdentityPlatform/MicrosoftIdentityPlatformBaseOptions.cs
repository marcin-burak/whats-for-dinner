using FluentValidation;
using WhatsForDinner.Common.FluentValidation;

namespace WhatsForDinner.Common.MicrosoftIdentityPlatform;

public abstract class MicrosoftIdentityPlatformBaseOptions
{
	public string TenantId { get; } = "consumers";

	public string Instance { get; } = "https://login.microsoftonline.com";

	public string ClientId { get; set; } = string.Empty;

	public string ClientSecret { get; set; } = string.Empty;
}

public abstract class MicrosoftIdentityPlatformBaseOptionsValidator<TOptions> : AbstractValidator<TOptions> where TOptions : MicrosoftIdentityPlatformBaseOptions
{
	public MicrosoftIdentityPlatformBaseOptionsValidator()
	{
		RuleFor(options => options.TenantId)
			.NotEmpty()
			.Trimmed()
			.Equal("consumers");

		RuleFor(options => options.Instance)
			.NotEmpty()
			.Trimmed()
			.Equal("https://login.microsoftonline.com");

		RuleFor(options => options.ClientId)
			.NotEmpty()
			.Trimmed()
			.NonEmptyGuid();

		RuleFor(options => options.ClientSecret)
			.NotEmpty()
			.Trimmed();
	}
}