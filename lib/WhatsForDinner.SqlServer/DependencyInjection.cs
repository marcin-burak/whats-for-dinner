﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatsForDinner.Common.Extensions;
using WhatsForDinner.SqlServer.Entities;

namespace WhatsForDinner.SqlServer;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlServerConfiguration(this IServiceCollection services, IConfiguration configuration) => services
        .AddOptionsByConvention<SqlServerOptions>()
        .AddSqlServer<DatabaseContext>(configuration.TryGetOptionsByConvention<SqlServerOptions>()?.ConnectionString)
        .AddIdentityCore<User>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .Services
        .AddTransient<DatabaseMigrations>();
}