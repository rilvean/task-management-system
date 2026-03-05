using Domain.Exceptions;
using Domain.ValueObjects;

namespace DomainTests;

public class PasswordHashNegativeTests
{
	[Theory]
	[InlineData("")]
	[InlineData("shortstring")]
	public void PasswordHash_From_ShouldThrow_WhenTooShortOrEmpty(string input)
	{
		Assert.Throws<PasswordHashException>(() => PasswordHash.From(input));
	}

	[Fact]
	public void PasswordHash_From_ShouldThrow_WhenNull()
	{
		string? input = null;
		Assert.Throws<PasswordHashException>(() => PasswordHash.From(input!));
	}
}