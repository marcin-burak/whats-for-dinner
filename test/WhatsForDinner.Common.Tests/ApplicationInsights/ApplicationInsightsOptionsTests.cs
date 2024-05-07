using FluentValidation.TestHelper;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using WhatsForDinner.Common.ApplicationInsights;

namespace WhatsForDinner.Common.Tests.ApplicationInsights;

public sealed class ApplicationInsightsOptionsTests
{
    [Theory]
    [MemberData(nameof(ValidOptions))]
    public void OptionsValidationSucceeds(ApplicationInsightsOptions options, IHostEnvironment environment)
    {
        ApplicationInsightsOptionsValidator validator = new(environment);

        var result = validator.TestValidate(options);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(InvalidOptions))]
    public void OptionsValidationFails(ApplicationInsightsOptions options, IHostEnvironment environment)
    {
        ApplicationInsightsOptionsValidator validator = new(environment);

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

    private static IHostEnvironment TestEnvironment
    {
        get
        {
            var mock = Substitute.For<IHostEnvironment>();
            mock.EnvironmentName = "Test";

            return mock;
        }
    }

    public static TheoryData<ApplicationInsightsOptions, IHostEnvironment> ValidOptions => new()
    {
        {
            new()
            {
                ConnectionString = "IngestionKey=",
                DeveloperMode = true
            },
            DevelopmentEnvironment
        },
        {
            new()
            {
                ConnectionString = "IngestionKey=",
                DeveloperMode = false
            },
            TestEnvironment
        },
    };

    public static TheoryData<ApplicationInsightsOptions, IHostEnvironment> InvalidOptions => new()
    {
        {
            new()
            {
                ConnectionString = null!,
                DeveloperMode = true
            },
            DevelopmentEnvironment
        },
        {
            new()
            {
                ConnectionString = string.Empty,
                DeveloperMode = true
            },
            DevelopmentEnvironment
        },
        {
            new()
            {
                ConnectionString = " ",
                DeveloperMode = true
            },
            DevelopmentEnvironment
        },
        {
            new()
            {
                ConnectionString = "IngestionKy=",
                DeveloperMode = true
            },
            DevelopmentEnvironment
        },
        {
            new()
            {
                ConnectionString = "ingestionkey=",
                DeveloperMode = true
            },
            DevelopmentEnvironment
        },
        {
            new()
            {
                ConnectionString = " IngestionKey= ",
                DeveloperMode = true
            },
            DevelopmentEnvironment
        },
        {
            new()
            {
                ConnectionString = "IngestionKey=",
                DeveloperMode = true
            },
            TestEnvironment
        },
    };

    #endregion
}
