using FluentValidation;
using WhatsForDinner.Common.FluentValidation;

namespace WhatsForDinner.UserProvisioning.Console;

public sealed class ProvisioningOptions
{
	public IReadOnlyCollection<Group> Groups { get; set; } = [];

	public IReadOnlyCollection<User> Users { get; set; } = [];

	public sealed class Group
	{
		public string Id { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;
	}

	public sealed class User
	{
		public string Id { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string FullName { get; set; } = string.Empty;

		public IReadOnlyCollection<string> GroupIds { get; set; } = [];
	}
}

public sealed class ProvisioningOptionsValidator : AbstractValidator<ProvisioningOptions>
{
	public ProvisioningOptionsValidator()
	{
		RuleFor(options => options.Groups)
			.NotEmpty()
			.NoNullItems()
			.Must(groups => groups is null || groups.Count == 0 || groups.Count == groups.DistinctBy(group => group.Id).Count()) //TODO: Use expression trees
				.WithMessage("{PropertyName} has to contain groups with unique IDs.")
			.WithName("Provisioning user groups collection");

		RuleForEach(options => options.Groups)
			.SetValidator(new GroupValidator());



		RuleFor(options => options.Users)
			.NotEmpty()
			.NoNullItems()
			.Must(users => users is null || users.Count == 0 || users.Count == users.DistinctBy(user => user.Id).Count()) //TODO: Use expression trees
				.WithMessage("{PropertyName} has to contain users with unique IDs.")
			.WithName("Provisioning users collection");

		RuleForEach(options => options.Users)
			.SetValidator(new UserValidator());
	}

	private class GroupValidator : AbstractValidator<ProvisioningOptions.Group>
	{
		public GroupValidator()
		{
			RuleFor(group => group.Id)
				.NotEmpty()
				.Trimmed()
				.NonEmptyGuid()
				.WithName("Group ID");

			RuleFor(group => group.Name)
				.NotEmpty()
				.Trimmed()
				.WithName("Group name");
		}
	}

	private class UserValidator : AbstractValidator<ProvisioningOptions.User>
	{
		public UserValidator()
		{
			RuleFor(user => user.Id)
				.NotEmpty()
				.Trimmed()
				.NonEmptyGuid()
				.WithName("User ID");

			RuleFor(user => user.Email)
				.NotEmpty()
				.Trimmed()
				.EmailAddress()
				.WithName("User email address");

			RuleFor(user => user.FullName)
				.NotEmpty()
				.Trimmed()
				.Must(fullName => string.IsNullOrWhiteSpace(fullName) || fullName.Where(character => character == ' ').Count() == 1)
					.WithMessage("{PropertyName} has to contain exactly one space character.")
				.WithName("User full name");

			RuleFor(user => user.GroupIds)
				.NotEmpty()
				.WithName("User group IDs collection");

			RuleForEach(user => user.GroupIds)
				.NotEmpty()
				.NonEmptyGuid()
				.WithName("User group ID");
		}
	}
}