using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatsForDinner.Common.Extensions;

namespace WhatsForDinner.SqlServer;

public static class DependencyInjection
{
	public static IServiceCollection AddSqlServerConfiguration(this IServiceCollection services, IConfiguration configuration) => services
		.AddOptionsByConvention<SqlServerOptions>()
		.AddSqlServer<DatabaseContext>(configuration.TryGetOptionsByConvention<SqlServerOptions>()?.ConnectionString)
		.AddTransient<DatabaseMigrations>();
}
