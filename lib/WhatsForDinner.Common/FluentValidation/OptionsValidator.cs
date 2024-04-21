using FluentValidation;
using Microsoft.Extensions.Options;

namespace WhatsForDinner.Common.FluentValidation;

public sealed class OptionsValidator<TOptions>(IValidator<TOptions> validator) : IValidateOptions<TOptions> where TOptions : class
{
    private readonly IValidator<TOptions> _validator = validator;

    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var validationResult = _validator.Validate(options);
        if (validationResult.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        var errorMessages = validationResult.Errors.Select(error => error.ErrorMessage);

        return ValidateOptionsResult.Fail(errorMessages);
    }
}
