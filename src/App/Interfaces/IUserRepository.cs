using App.Enums;
using Domain.Models;
using Domain.ValueObjects;

namespace App.Interfaces;

public interface IUserRepository
{
	Task<User?> GetByIdAsync(Guid Id);
	Task<User?> GetByEmailAsync(Email email);
	Task<IReadOnlyList<User>> GetAllSortedAsync(UserSortBy sortBy = default, bool desc = default);

	Task AddAsync(User user);
	void Delete(User user);
}