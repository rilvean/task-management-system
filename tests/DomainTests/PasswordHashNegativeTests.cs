using Domain.Exceptions;
using Domain.ValueObjects;

namespace DomainTests;

public class PasswordHashNegativeTests
{
	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData("not69lengthstring")]
	public void PasswordHash_From_ShouldThrow_WhenLengthNotMatchOrEmpty(string input)
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