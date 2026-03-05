using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Domain.ValueObjects;

namespace DomainTests;

public class UserNegativeTests
{
	private Email emailTest = Email.From("test@example.com");
	private PasswordHash passwordTest = PasswordHash.From(PasswordHasher.Hash("hash"));

	[Fact]
	public void User_Creation_ShouldThrow_WhenNameIsNullOrWhitespace()
	{
		Assert.Throws<ArgumentNullException>(() => new User(null!, emailTest, passwordTest, UserRole.Employee));
		Assert.Throws<ArgumentNullException>(() => new User("", emailTest, passwordTest, UserRole.Employee));
		Assert.Throws<ArgumentNullException>(() => new User("   ", emailTest, passwordTest, UserRole.Employee));
	}

	[Fact]
	public void ChangeName_ShouldThrow_WhenNameIsNullOrWhitespace()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);

		Assert.Throws<ArgumentNullException>(() => user.ChangeName(null!));
		Assert.Throws<ArgumentNullException>(() => user.ChangeName(""));
		Assert.Throws<ArgumentNullException>(() => user.ChangeName("   "));
	}

	[Fact]
	public void ChangeEmail_ShouldThrow_WhenEmailIsNull()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		Assert.Throws<ArgumentNullException>(() => user.ChangeEmail(null!));
	}

	[Fact]
	public void ChangePassword_ShouldThrow_WhenPasswordIsNull()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		Assert.Throws<ArgumentNullException>(() => user.ChangePassword(null!));
	}
}