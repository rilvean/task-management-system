using Domain.Services;
using Domain.ValueObjects;

namespace DomainTests;

public class PasswordHasherTests
{
	[Fact]
	public void Hash_ShouldReturnPasswordHash()
	{
		string password = "MySecurePassword123!";
		var hash = PasswordHasher.Hash(password);

		Assert.NotNull(hash);
		Assert.IsType<PasswordHash>(hash);
		Assert.Contains(":", hash.Value);
		Assert.True(hash.Value.Length >= 20);
	}

	[Fact]
	public void Verify_ShouldReturnTrue_ForCorrectPassword()
	{
		string password = "MyPassword123!";
		var hash = PasswordHasher.Hash(password);

		bool result = PasswordHasher.Verify(password, hash);

		Assert.True(result);
	}

	[Fact]
	public void Verify_ShouldReturnFalse_ForIncorrectPassword()
	{
		string password = "MyPassword123!";
		string wrongPassword = "WrongPassword!";
		var hash = PasswordHasher.Hash(password);

		bool result = PasswordHasher.Verify(wrongPassword, hash);

		Assert.False(result);
	}
}