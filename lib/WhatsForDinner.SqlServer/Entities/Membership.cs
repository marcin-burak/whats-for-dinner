using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WhatsForDinner.SqlServer.Entities;

public sealed class Membership
{
    public Guid UserId { get; set; }

    public User User { get; set; } = null!;



    public Guid GroupId { get; set; }

    public Group Group { get; set; } = null!;
}

public sealed class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.HasKey(user => new { user.UserId, user.GroupId })
            .IsClustered(false);
    }
}