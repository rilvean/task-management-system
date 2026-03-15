using Domain.Services;
using Domain.ValueObjects;

namespace DomainTests;

public class PasswordHashTests
{
	[Fact]
	public void PasswordHash_From_ShouldTrimValue()
	{
		var password = PasswordHasher.Hash(new string('1', 20));
		var hash = PasswordHash.From($"   {password}   ");
		Assert.Equal(password, hash.Value);
	}

	[Fact]
	public void PasswordHash_ImplicitConversion_ToString_Works()
	{
		var password = PasswordHasher.Hash(new string('1', 20));
		var hash = PasswordHash.From(password);

		string str = hash;
		
		Assert.Equal(password, str);
		Assert.Equal(hash, str);
	}

	[Fact]
	public void PasswordHash_ExplicitConversion_ToString_Works()
	{
		var password = PasswordHasher.Hash(new string('1', 20));
		var hash = PasswordHash.From(password);

		string str = hash.ToString();
		
		Assert.Equal(password, str);
		Assert.Equal(hash, str);
	}
}