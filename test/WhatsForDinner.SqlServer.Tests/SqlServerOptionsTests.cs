using FluentValidation.TestHelper;
using Microsoft.Extensions.Hosting;
using NSubstitute;

namespace WhatsForDinner.SqlServer.Tests;

public sealed class SqlServerOptionsTests
{
	[Theory]
	[MemberData(nameof(ValidOptions))]
	public void OptionsValidationSucceeds(SqlServerOptions options, IHostEnvironment environment)
	{
		SqlServerOptionsValidator validator = new(environment);

		var result = validator.TestValidate(options);

		result.ShouldNotHaveAnyValidationErrors();
	}

	[Theory]
	[MemberData(nameof(InvalidOptions))]
	public void OptionsValidationFails(SqlServerOptions options, IHostEnvironment environment)
	{
		SqlServerOptionsValidator validator = new(environment);

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

	public static TheoryData<SqlServerOptions, IHostEnvironment> ValidOptions => new()
	{
		{
			new()
			{
				ConnectionString = "Server=localhost,1433;Database=WhatsForDinner;User=sa;Password=P@ssw0rd;TrustServerCertificate=true;",
				RunMigrationsOnStartup = true
			},
			DevelopmentEnvironment
		},
		{
			new()
			{
				ConnectionString = "Server=localhost,1433;Database=WhatsForDinner;User=sa;Password=P@ssw0rd;TrustServerCertificate=true;",
				RunMigrationsOnStartup = false
			},
			NonDevelopmentEnvironment
		},
	};

	public static TheoryData<SqlServerOptions, IHostEnvironment> InvalidOptions => new()
	{
		{
			new()
			{
				ConnectionString = null!,
				RunMigrationsOnStartup = true
			},
			DevelopmentEnvironment
		},
		{
			new()
			{
				ConnectionString = string.Empty,
				RunMigrationsOnStartup = true
			},
			DevelopmentEnvironment
		},
		{
			new()
			{
				ConnectionString = " ",
				RunMigrationsOnStartup = true
			},
			DevelopmentEnvironment
		},
		{
			new()
			{
				ConnectionString = " Server=sqlserver,1433;Database=WhatsForDinner;User=sa;Password=P@ssw0rd;TrustServerCertificate=true; ",
				RunMigrationsOnStartup = true
			},
			DevelopmentEnvironment
		},
		{
			new()
			{
				ConnectionString = "Serer=sqlserver,1433;Database=WhatsForDinner;User=sa;Password=P@ssw0rd;TrustServerCertificate=true;",
				RunMigrationsOnStartup = true
			},
			DevelopmentEnvironment
		},
		{
			new()
			{
				ConnectionString = "Serer=sqlserver,1433;Databse=WhatsForDinner;User=sa;Password=P@ssw0rd;TrustServerCertificate=true;",
				RunMigrationsOnStartup = true
			},
			DevelopmentEnvironment
		},
		{
			new()
			{
				ConnectionString = "server=sqlserver,1433;database=WhatsForDinner;User=sa;Password=P@ssw0rd;TrustServerCertificate=true;",
				RunMigrationsOnStartup = true
			},
			DevelopmentEnvironment
		},
		{
			new()
			{
				ConnectionString = "Server=sqlserver,1433;Database=WhatsForDinner;User=sa;Password=P@ssw0rd;TrustServerCertificate=true;",
				RunMigrationsOnStartup = true
			},
			NonDevelopmentEnvironment
		},
	};

	#endregion
}