using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WhatsForDinner.Common.FluentValidation;

namespace WhatsForDinner.Common.Extensions;

public static class Extensions
{
	public static IServiceCollection AddOptionsByConvention<TOptions>(this IServiceCollection services) where TOptions : class
	{
		var optionsTypeValidator = typeof(TOptions).Assembly
			.GetTypes()
			.SingleOrDefault(type => type.IsClass && type.IsSealed && type.IsAssignableTo(typeof(AbstractValidator<>).MakeGenericType(typeof(TOptions))));

		if (optionsTypeValidator is null)
		{
			throw new InvalidOperationException($"Failed to find '{typeof(TOptions).FullName}' options type validator.");
		}

		services.TryAddSingleton(typeof(IValidator<>).MakeGenericType(typeof(TOptions)), optionsTypeValidator);

		return services.AddOptionsWithValidateOnStart<TOptions, OptionsValidator<TOptions>>().BindConfiguration(GetOptionsConfigurationSectionKey<TOptions>()).Services;
	}

	public static TOptions GetOptionsByConvention<TOptions>(this IConfiguration configuration) where TOptions : class =>
		GetOptions<TOptions>(configuration, isRequired: true)!;

	public static TOptions? TryGetOptionsByConvention<TOptions>(this IConfiguration configuration) where TOptions : class =>
		GetOptions<TOptions>(configuration, isRequired: false);



	private static TOptions? GetOptions<TOptions>(this IConfiguration configuration, bool isRequired) where TOptions : class
	{
		var configurationSectionKey = GetOptionsConfigurationSectionKey<TOptions>();
		var configurationSection = configuration.GetSection(configurationSectionKey);

		if (isRequired && configurationSection is null)
		{
			throw new InvalidOperationException($"Failed to find configuration section with key '{configurationSectionKey}'.");
		}

		if (configurationSection is null)
		{
			return null;
		}

		var optionsInstance = Activator.CreateInstance<TOptions>();
		configurationSection.Bind(optionsInstance);

		return optionsInstance;
	}

	private static string GetOptionsConfigurationSectionKey<TOptions>() where TOptions : class => typeof(TOptions).Name[..^7];
}
