using Domain.ValueObjects;

namespace DomainTests;

public class PasswordHashTests
{
	[Fact]
	public void PasswordHash_From_ShouldTrimValue()
	{
		var hash = PasswordHash.From("   12345678901234567890   ");
		Assert.Equal("12345678901234567890", hash.Value);
	}

	[Fact]
	public void PasswordHash_ImplicitConversion_ToString_Works()
	{
		var hash = PasswordHash.From("12345678901234567890");
		string str = hash;
		Assert.Equal("12345678901234567890", str);
		Assert.Equal(hash.Value, str);
	}

	[Fact]
	public void PasswordHash_ExplicitConversion_ToString_Works()
	{
		var hash = PasswordHash.From("12345678901234567890");
		string str = hash.ToString();
		Assert.Equal("12345678901234567890", str);
		Assert.Equal(hash.Value, str);
	}
}