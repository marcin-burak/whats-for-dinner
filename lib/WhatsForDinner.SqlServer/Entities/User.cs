using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WhatsForDinner.SqlServer.Entities;

public sealed class User : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;
}

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.FirstName)
            .HasMaxLength(8);

        builder.Property(user => user.LastName)
            .HasMaxLength(10);

        builder.Property(user => user.FullName)
            .HasMaxLength(18);
    }
}