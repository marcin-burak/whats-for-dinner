using FluentValidation;
using Microsoft.Extensions.Hosting;
using WhatsForDinner.Common.FluentValidation;

namespace WhatsForDinner.SqlServer;

public sealed class SqlServerOptions
{
	public string ConnectionString { get; set; } = string.Empty;

	public bool RunMigrationsOnStartup { get; set; }
}

public sealed class SqlServerOptionsValidator : AbstractValidator<SqlServerOptions>
{
	public SqlServerOptionsValidator(IHostEnvironment environment)
	{
		RuleFor(options => options.ConnectionString)
			.NotEmpty()
			.Trimmed()
			.Must(connectionString =>
				string.IsNullOrWhiteSpace(connectionString) is false &&
				connectionString.Contains("Server=") &&
				connectionString.Contains("Database=")
			).WithMessage("{PropertyName} has to contain server and database information.")
			.WithName("SQL Server connection string");

		When(_ => environment.IsDevelopment() is false, () =>
		{
			RuleFor(options => options.RunMigrationsOnStartup)
				.Equal(false)
				.WithMessage("Running database migrations on application startup has to be disabled in non-development environments.");
		});
	}
}