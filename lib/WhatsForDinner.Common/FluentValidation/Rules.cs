using FluentValidation;

namespace WhatsForDinner.Common.FluentValidation;

public static class Rules
{
    public static IRuleBuilderOptions<T, string> Trimmed<T>(this IRuleBuilderOptions<T, string> builder) => builder
        .Must(value => string.IsNullOrWhiteSpace(value) is false && value.Length == value.Trim().Length)
        .WithMessage("{PropertyName} has to be trimmed.");

    public static IRuleBuilderOptions<T, string> NonEmptyGuid<T>(this IRuleBuilderOptions<T, string> builder) => builder
        .Must(value => string.IsNullOrWhiteSpace(value) is false && Guid.TryParse(value, out var guid) && guid != Guid.Empty)
        .WithMessage("{PropertyName} has to be a non empty GUID.");
}
