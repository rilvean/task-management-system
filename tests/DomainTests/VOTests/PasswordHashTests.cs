using Domain.Services;
using Domain.ValueObjects;

namespace DomainTests.VOTests;

public class PasswordHashTests
{
	private readonly PasswordHash password = PasswordHasher.Hash(new string('1', 20));

	[Fact]
	public void PasswordHash_From_ShouldTrimValue()
	{
		var hash = PasswordHash.From($"   {password}   ");

		Assert.Equal(password, hash.Value);
	}

	[Fact]
	public void PasswordHash_ImplicitConversion_ToString_Works()
	{
		var hash = PasswordHash.From(password);

		string str = hash;

		Assert.Equal(password, str);
		Assert.Equal(hash, str);
	}

	[Fact]
	public void PasswordHash_ExplicitConversion_ToString_Works()
	{
		var hash = PasswordHash.From(password);

		string str = hash.ToString();

		Assert.Equal(password, str);
		Assert.Equal(hash, str);
	}
}