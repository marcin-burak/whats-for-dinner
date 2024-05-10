using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WhatsForDinner.SqlServer.Entities;

public sealed class Group
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;



    public List<User> Users { get; } = [];

    public List<Membership> Memberships { get; } = [];



    public List<Meal> Meals { get; } = [];
}

public sealed class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.HasKey(group => group.Id)
            .IsClustered(false);
    }
}