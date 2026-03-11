using Domain.Enums;
using Domain.Interfaces;
using Domain.Services;
using Domain.ValueObjects;

namespace App.Services;

public class AuthorizationService(IUserRepository ur)
{
	public async Task<(Guid id, UserRole role)> LoginAsync(string email, string password)
	{
		var user = await ur.GetByEmailAsync(Email.From(email))
			?? throw new AuthorizationException("User not found.");

		if (!PasswordHasher.Verify(password, user.PasswordHash))
			throw new AuthorizationException($"Invalid {nameof(password)}.");

		return (user.Id, user.Role);
	}
}