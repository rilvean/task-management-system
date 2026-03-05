using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed record Email
{
	private const int MAX_LENGHT = 50;

	private static readonly Regex EmailRegex = new(
		@"^(?!.*(\.\.|__|\+\+|--))[A-Za-z0-9][A-Za-z0-9._+-]+[A-Za-z0-9]@[a-z][a-z0-9.-]+[a-z0-9]\.[a-z]{2,}$",
		RegexOptions.Compiled
	);

	public string Value { get; }

	private Email(string value) => Value = value;

	public static Email From(string value)
	{
		if (string.IsNullOrWhiteSpace(value) || !EmailRegex.IsMatch(value))
			throw new EmailException("Invalid email.");

		if (value.Length > MAX_LENGHT)
			throw new EmailException("Email too long.");

		value = value.Trim().ToLower();

		return new Email(value);
	}

	public static implicit operator string(Email email) => email.Value;
	public override string ToString() => Value;
}