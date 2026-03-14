using Domain.Exceptions;

namespace Domain.ValueObjects;

public sealed record PasswordHash
{
	private const int LENGTH = 69;

	public string Value { get; }

	private PasswordHash(string value) => Value = value;

	public static PasswordHash From(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
			throw new PasswordHashException($"'{nameof(value)}' is empty.");

		value = value.Trim();

		if (value.Length != LENGTH)
			throw new PasswordHashException($"Invalid {nameof(value)}.");

		return new PasswordHash(value);
	}

	public static implicit operator string(PasswordHash hash) => hash.Value;
	public override string ToString() => Value;
}