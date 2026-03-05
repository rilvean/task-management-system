using Domain.Exceptions;

namespace Domain.ValueObjects;

public sealed record PasswordHash
{
	private const int MIN_LENGTH = 20;

	public string Value { get; }

	private PasswordHash(string value) => Value = value;

	public static PasswordHash From(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new PasswordHashException($"'{nameof(value)}' cannot be null.");

		if (value.Length < MIN_LENGTH)
			throw new PasswordHashException($"'{nameof(value)}' too small.");

		value = value.Trim();

		return new PasswordHash(value);
	}

	public static implicit operator string(PasswordHash hash) => hash.Value;
	public override string ToString() => Value;
}