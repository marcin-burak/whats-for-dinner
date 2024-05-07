using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WhatsForDinner.SqlServer;

public sealed class DatabaseMigrations(IOptions<SqlServerOptions> options, DatabaseContext context)
{
    private readonly SqlServerOptions _options = options.Value;
    private readonly DatabaseContext _context = context;

    public async ValueTask TryRunMigrations(CancellationToken cancellationToken)
    {
        if (_options.RunMigrationsOnStartup is false)
        {
            return;
        }

        await _context.Database.MigrateAsync(cancellationToken);
    }

    public static async ValueTask RunMigrationsIfConfigured(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var databaseMigrations = scope.ServiceProvider.GetRequiredService<DatabaseMigrations>();
        await databaseMigrations.TryRunMigrations(cancellationToken);
    }
}
