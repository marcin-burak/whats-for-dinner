using FluentValidation;

namespace WhatsForDinner.Common.FluentValidation;

public static class Rules
{
    public static IRuleBuilderOptions<T, string> Trimmed<T>(this IRuleBuilderOptions<T, string> builder) => builder
        .Must(value => string.IsNullOrWhiteSpace(value) is false && value.Length == value.Trim().Length)
        .WithMessage("{PropertyName} has to be trimmed.");
}
