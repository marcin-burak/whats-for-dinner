using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WhatsForDinner.SqlServer;
using WhatsForDinner.UserProvisioning.Console;

var builder = Host.CreateApplicationBuilder();

builder.Services
    .AddSqlServerConfiguration(builder.Configuration)
    .AddScoped<UserProvisioning>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

scope.ServiceProvider.GetRequiredService<IStartupValidator>().Validate();

var userProvisioning = scope.ServiceProvider.GetRequiredService<UserProvisioning>();
await userProvisioning.ProvisionUsers(CancellationToken.None);