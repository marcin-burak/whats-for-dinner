using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WhatsForDinner.SqlServer;
using WhatsForDinner.SqlServer.Entities;

namespace WhatsForDinner.UserProvisioning.Console;

public sealed class UserProvisioning(ILogger<UserProvisioning> logger, DatabaseContext context, IOptions<ProvisioningOptions> options)
{
    private readonly ILogger<UserProvisioning> _logger = logger;
    private readonly DatabaseContext _context = context;
    private readonly ProvisioningOptions _options = options.Value;

    public async Task ProvisionUsers(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting user provisioning process...");

        _logger.LogInformation("Saving data to database...");

        using (var transaction = await _context.Database.BeginTransactionAsync(cancellationToken))
        {
            var groupEntities = _options.Groups.Select(group => new Group
            {
                Id = Guid.Parse(group.Id),
                Name = group.Name
            });

            await _context.AddRangeAsync(groupEntities, cancellationToken);



            var userEntities = _options.Users.Select(user =>
            {
                var fullNameSplit = user.FullName.Split(' ');
                return new User
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    FirstName = fullNameSplit.First(),
                    LastName = fullNameSplit.Last(),
                    FullName = user.FullName
                };
            });

            await _context.User.AddRangeAsync(userEntities, cancellationToken);



            var memberships = _options.Users.Aggregate(new List<Membership>(), (memberships, user) =>
            {
                var currentUserMemberships = user.GroupIds.Select(groupId => new Membership
                {
                    UserId = Guid.Parse(user.Id),
                    GroupId = Guid.Parse(groupId)
                });

                memberships.AddRange(currentUserMemberships);

                return memberships;
            });

            await _context.Membership.AddRangeAsync(memberships, cancellationToken);



            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }

        _logger.LogInformation("Data added to the database.");

        _logger.LogInformation("User provisioning process has finished.");
    }
}
