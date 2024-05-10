using FluentValidation;
using FluentValidation.TestHelper;
using WhatsForDinner.Common.FluentValidation;

namespace WhatsForDinner.Common.Tests.FluentValidation;

public sealed class RulesTests
{
    [Theory]
    [InlineData("a")]
    [InlineData("aaa")]
    [InlineData("test")]
    public void Trimmed_ValidationSuceeds(string value)
    {
        var validator = new InlineValidator<Test<string>>();
        validator.RuleFor(test => test.Value).Trimmed();

        var result = validator.TestValidate(new Test<string> { Value = value });

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" a")]
    [InlineData("a ")]
    [InlineData(" a ")]
    public void Trimmed_ValidationFails(string? value)
    {
        var validator = new InlineValidator<Test<string>>();
        validator.RuleFor(test => test.Value).Trimmed();

        var result = validator.TestValidate(new Test<string> { Value = value! });

        result.ShouldHaveAnyValidationError();
    }



    [Theory]
    [InlineData("77c23b53-3545-439e-b972-5adaa4461c7a")]
    public void NonEmptyGuid_ValidationSuceeds(string value)
    {
        var validator = new InlineValidator<Test<string>>();
        validator.RuleFor(test => test.Value).NonEmptyGuid();

        var result = validator.TestValidate(new Test<string> { Value = value });

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("77c23b53-3545-439e-b972-5adaa4461c")]
    public void NonEmptyGuid_ValidationFails(string? value)
    {
        var validator = new InlineValidator<Test<string>>();
        validator.RuleFor(test => test.Value).NonEmptyGuid();

        var result = validator.TestValidate(new Test<string> { Value = value! });

        result.ShouldHaveAnyValidationError();
    }



    [Theory]
    [InlineData("https://test.com/hello?query=test")]
    public void AbsoluteHttpsUri_ValidationSuceeds(string value)
    {
        var validator = new InlineValidator<Test<string>>();
        validator.RuleFor(test => test.Value).AbsoluteHttpsUri();

        var result = validator.TestValidate(new Test<string> { Value = value });

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("http://test.com/hello?query=test")]
    [InlineData("https:/test.com/hello?query=test")]
    [InlineData("https://")]
    public void AbsoluteHttpsUri_ValidationFails(string? value)
    {
        var validator = new InlineValidator<Test<string>>();
        validator.RuleFor(test => test.Value).AbsoluteHttpsUri();

        var result = validator.TestValidate(new Test<string> { Value = value! });

        result.ShouldHaveAnyValidationError();
    }



    [Theory]
    [MemberData(nameof(NoNullItems_Valid))]
    public void NoNullItems_ValidationSuceeds(IReadOnlyCollection<object> value)
    {
        var validator = new InlineValidator<Test<IReadOnlyCollection<object>>>();
        validator.RuleFor(test => test.Value).NoNullItems();

        var result = validator.TestValidate(new Test<IReadOnlyCollection<object>> { Value = value });

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(NoNullItems_Invalid))]
    public void NoNullItems_ValidationFails(IReadOnlyCollection<object> value)
    {
        var validator = new InlineValidator<Test<IReadOnlyCollection<object>>>();
        validator.RuleFor(test => test.Value).NoNullItems();

        var result = validator.TestValidate(new Test<IReadOnlyCollection<object>> { Value = value });

        result.ShouldHaveAnyValidationError();
    }



    [Theory]
    [MemberData(nameof(DistinctItems_Valid))]
    public void DistinctItems_ValidationSuceeds(IReadOnlyCollection<object> value)
    {
        var validator = new InlineValidator<Test<IReadOnlyCollection<object>>>();
        validator.RuleFor(test => test.Value).DistinctItems();

        var result = validator.TestValidate(new Test<IReadOnlyCollection<object>> { Value = value });

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(DistinctItems_Invalid))]
    public void DistinctItems_ValidationFails(IReadOnlyCollection<object> value)
    {
        var validator = new InlineValidator<Test<IReadOnlyCollection<object>>>();
        validator.RuleFor(test => test.Value).DistinctItems();

        var result = validator.TestValidate(new Test<IReadOnlyCollection<object>> { Value = value });

        result.ShouldHaveAnyValidationError();
    }



    #region Test data

    private sealed class Test<T>
    {
        public required T Value { get; init; }
    }



    public static TheoryData<IReadOnlyCollection<object>> NoNullItems_Valid => new()
    {
        {
            [""]
        },
        {
            ["", "", ""]
        },
    };

    public static TheoryData<IReadOnlyCollection<object>> NoNullItems_Invalid => new()
    {
        {
            [null!]
        },
        {
            [null!, null!, null!]
        },
        {
            ["", null!, ""]
        },
    };



    public static TheoryData<IReadOnlyCollection<object>> DistinctItems_Valid => new()
    {
        {
            ["1"]
        },
        {
            ["1", "2", "3"]
        },
    };

    public static TheoryData<IReadOnlyCollection<object>> DistinctItems_Invalid => new()
    {
        {
            [null!, null!]
        },
        {
            ["", null!, ""]
        },
        {
            ["1", "2", "3", "1"]
        },
    };

    #endregion
}