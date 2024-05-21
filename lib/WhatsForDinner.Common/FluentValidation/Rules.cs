using FluentValidation;

namespace WhatsForDinner.Common.FluentValidation;

public static class Rules
{
    public static IRuleBuilderOptions<T, string> Trimmed<T>(this IRuleBuilder<T, string> builder) => builder
        .Must(value => string.IsNullOrWhiteSpace(value) is false && value.Length == value.Trim().Length)
        .WithMessage("{PropertyName} has to be trimmed.");

    public static IRuleBuilderOptions<T, string> NonEmptyGuid<T>(this IRuleBuilder<T, string> builder) => builder
        .Must(value => string.IsNullOrWhiteSpace(value) is false && Guid.TryParse(value, out var guid) && guid != Guid.Empty)
        .WithMessage("{PropertyName} has to be a non empty GUID.");

    public static IRuleBuilderOptions<T, string> AbsoluteHttpUri<T>(this IRuleBuilder<T, string> builder) => builder
        .Must(value => string.IsNullOrWhiteSpace(value) is false && value.StartsWith("http://") && Uri.TryCreate(value, UriKind.Absolute, out var _))
        .WithMessage("{PropertyName} has to be an absolute HTTP URI.");

    public static IRuleBuilderOptions<T, string> AbsoluteHttpsUri<T>(this IRuleBuilder<T, string> builder) => builder
        .Must(value => string.IsNullOrWhiteSpace(value) is false && value.StartsWith("https://") && Uri.TryCreate(value, UriKind.Absolute, out var _))
        .WithMessage("{PropertyName} has to be an absolute HTTPS URI.");

    public static IRuleBuilderOptions<T, IReadOnlyCollection<TItem>> NoNullItems<T, TItem>(this IRuleBuilder<T, IReadOnlyCollection<TItem>> builder) where TItem : class => builder
        .Must(collection => collection is not null && collection.All(item => item is not null))
        .WithMessage("{PropertyName} must not contain NULL items.");

    public static IRuleBuilderOptions<T, IReadOnlyCollection<TItem>> DistinctItems<T, TItem>(this IRuleBuilder<T, IReadOnlyCollection<TItem>> builder) where TItem : class => builder
        .Must(collection => collection is not null && collection.Count > 0 && collection.Count == collection.Distinct().Count())
        .WithMessage("{PropertyName} has to contain distinct items.");
}
