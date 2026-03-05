using Domain.ValueObjects;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Services;

public static class PasswordHasher
{
	public static PasswordHash Hash(string password)
	{
		byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
		byte[] hash = ComputeHash(password, saltBytes);

		string result = $"{Convert.ToBase64String(saltBytes)}:{Convert.ToBase64String(hash)}";

		return PasswordHash.From(result);
	}

	public static bool Verify(string password, PasswordHash hash)
	{
		string[] parts = hash.Value.Split(':');
		if (parts.Length != 2) return false;

		byte[] salt = Convert.FromBase64String(parts[0]);
		string storedHash = parts[1];

		string computedHash = Convert.ToBase64String(ComputeHash(password, salt));

		return storedHash == computedHash;
	}

	private static byte[] ComputeHash(string password, byte[] salt)
	{
		using var sha = SHA256.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(password);

		byte[] combined = new byte[bytes.Length + salt.Length];
		Buffer.BlockCopy(bytes, 0, combined, 0, bytes.Length);
		Buffer.BlockCopy(salt, 0, combined, bytes.Length, salt.Length);

		return sha.ComputeHash(combined);
	}
}