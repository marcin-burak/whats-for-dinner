using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WhatsForDinner.SqlServer;
using WhatsForDinner.SqlServer.Entities;
using WhatsForDinner.SqlServer.Extensions;

namespace WhatsForDinner.UserProvisioning.Console;

public sealed class UserProvisioning(ILogger<UserProvisioning> logger, DatabaseContext context, UserManager<User> userManager)
{
    private readonly ILogger<UserProvisioning> _logger = logger;
    private readonly DatabaseContext _context = context;
    private readonly UserManager<User> _userManager = userManager;

    public async Task ProvisionUsers(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting user provisioning process...");

        _logger.LogInformation("Adding first test user...");

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        User user = new()
        {
            FirstName = "Marcin",
            LastName = "Burak",
            FullName = "Marcin Burak",
            UserName = "marcin.burak@outlook.com"
        };

        var createUserResult = await _userManager.CreateAsync(user);
        createUserResult.ThrowOnFail($"Failed to create user '{user.UserName}'");

        var addUserLoginResult = await _userManager.AddLoginAsync(user, new(
            loginProvider: "Microsoft",
            providerKey: "Marcin Burak",
            displayName: "Microsoft"
        ));
        addUserLoginResult.ThrowOnFail($"Failed to add Microsoft user login for user '{user.UserName}'");

        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        _logger.LogInformation("Fist test user added.");

        _logger.LogInformation("User provisioning process has finished.");
    }
}
