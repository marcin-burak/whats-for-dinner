using FluentValidation.TestHelper;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using WhatsForDinner.Web.Dependencies.OpenApi;

namespace WhatsForDinner.Web.Tests.Dependencies.OpenApi;

public sealed class ConfigurationTests
{
    [Theory]
    [MemberData(nameof(ValidOptions))]
    public void ValidationSucceeds(OpenApiOptions options, IHostEnvironment environment)
    {
        OpenApiOptionsValidator validator = new(environment);

        var result = validator.TestValidate(options);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(InvalidOptions))]
    public void ValidationFails(OpenApiOptions options, IHostEnvironment environment)
    {
        OpenApiOptionsValidator validator = new(environment);

        var result = validator.TestValidate(options);

        result.ShouldHaveAnyValidationError();
    }



    #region Test data

    private static IHostEnvironment DevelopmentEnvironment
    {
        get
        {
            var mock = Substitute.For<IHostEnvironment>();
            mock.EnvironmentName = "Development";

            return mock;
        }
    }

    private static IHostEnvironment NonDevelopmentEnvironment
    {
        get
        {
            var mock = Substitute.For<IHostEnvironment>();
            mock.EnvironmentName = "NonDevelopment";

            return mock;
        }
    }

    public static TheoryData<OpenApiOptions, IHostEnvironment> ValidOptions => new()
    {
        {
            new()
            {
                Enabled = true
            },
            DevelopmentEnvironment
        },
        {
            new()
            {
                Enabled = false
            },
            NonDevelopmentEnvironment
        },
    };

    public static TheoryData<OpenApiOptions, IHostEnvironment> InvalidOptions => new()
    {
        {
            new()
            {
                Enabled = true
            },
            NonDevelopmentEnvironment
        },
    };

    #endregion
}
