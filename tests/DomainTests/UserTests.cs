using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Domain.ValueObjects;

namespace DomainTests;

public class UserTests
{
	private Email emailTest = Email.From("test@example.com");
	private PasswordHash passwordTest = PasswordHash.From(PasswordHasher.Hash("hash"));

	[Fact]
	public void User_Creation_ShouldInitializeProperties()
	{
		var email = emailTest;
		var password = passwordTest;
		var role = UserRole.Employee;

		var user = new User("John", email, password, role);

		Assert.Equal("John", user.Name);
		Assert.Equal(email, user.Email);
		Assert.Equal(password, user.PasswordHash);
		Assert.Equal(role, user.Role);
		Assert.Empty(user.Tasks);
		Assert.NotEqual(Guid.Empty, user.Id);
	}

	[Fact]
	public void ChangeName_ShouldUpdateName()
	{
		var user = new User(
			"Old",
			emailTest,
			passwordTest,
			UserRole.Employee);

		user.ChangeName("New");

		Assert.Equal("New", user.Name);
	}

	[Fact]
	public void ChangeEmail_ShouldUpdateEmail()
	{
		var user = new User(
			"Name",
			emailTest,
			passwordTest,
			UserRole.Employee);
		var newEmail = Email.From("new@example.com");

		user.ChangeEmail(newEmail);

		Assert.Equal(newEmail, user.Email);
	}

	[Fact]
	public void ChangePassword_ShouldUpdatePassword()
	{
		var user = new User(
			"Name", 
			emailTest,
			passwordTest,
			UserRole.Employee);
		var newPassword = PasswordHash.From(PasswordHasher.Hash("new"));

		user.ChangePassword(newPassword);

		Assert.Equal(newPassword, user.PasswordHash);
	}

	[Fact]
	public void ChangeRole_ShouldUpdateRole()
	{
		var user = new User(
			"Name", 
			emailTest,
			passwordTest,
			UserRole.Employee);

		user.ChangeRole(UserRole.Admin);

		Assert.Equal(UserRole.Admin, user.Role);
	}
}