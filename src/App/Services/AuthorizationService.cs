using App.Excepions;
using App.Interfaces;
using Domain.Enums;
using Domain.Services;
using Domain.ValueObjects;

namespace App.Services;

public class AuthorizationService(IUnitOfWork unitOfWork)
{
	public async Task<(Guid id, UserRole role, string name)> LoginAsync(string email, string password)
	{
		var user = await unitOfWork.UserRepository.GetByEmailAsync(Email.From(email))
			?? throw new AuthorizationException("User not found.");

		if (!PasswordHasher.Verify(password, user.PasswordHash))
			throw new AuthorizationException($"Invalid {nameof(password)}.");

		return (user.Id, user.Role, user.Name);
	}
}