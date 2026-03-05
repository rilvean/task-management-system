using Domain.Exceptions;
using Domain.ValueObjects;

namespace DomainTests;

public class EmailNegativeTests
{
	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData("invalid")]
	[InlineData("a@b@c.com")]
	public void Email_From_ShouldThrow_WhenInvalid(string input)
	{
		Assert.Throws<EmailException>(() => Email.From(input));
	}

	[Fact]
	public void Email_From_ShouldThrow_WhenNull()
	{
		string? input = null;
		Assert.Throws<EmailException>(() => Email.From(input!));
	}

	[Fact]
	public void Email_From_ShouldThrow_WhenTooLong()
	{
		string longEmail = "toolong" + new string('a', 50) + "@example.com";
		Assert.Throws<EmailException>(() => Email.From(longEmail));
	}
}