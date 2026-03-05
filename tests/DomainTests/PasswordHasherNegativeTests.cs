using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects;

namespace DomainTests;

public class PasswordHasherNegativeTests
{
	[Fact]
	public void Hash_ShouldThrow_WhenPasswordIsNullOrWhitespace()
	{
		Assert.Throws<PasswordHasherException>(() => PasswordHasher.Hash(null!));
		Assert.Throws<PasswordHasherException>(() => PasswordHasher.Hash(""));
		Assert.Throws<PasswordHasherException>(() => PasswordHasher.Hash("   "));
	}

	[Fact]
	public void Verify_ShouldThrow_WhenPasswordIsNullOrWhitespace()
	{
		var hash = PasswordHasher.Hash("abc1234567890");
		Assert.Throws<PasswordHasherException>(() => PasswordHasher.Verify(null!, hash));
		Assert.Throws<PasswordHasherException>(() => PasswordHasher.Verify("", hash));
		Assert.Throws<PasswordHasherException>(() => PasswordHasher.Verify("   ", hash));
	}


	[Fact]
	public void Verify_ShouldThrow_WhenHashIsNull()
	{
		string password = "password";
		PasswordHash? hash = null;
		Assert.Throws<PasswordHasherException>(() => PasswordHasher.Verify(password, hash!));
	}
}