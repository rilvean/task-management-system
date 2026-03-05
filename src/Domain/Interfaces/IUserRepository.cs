using Domain.Enums;
using Domain.Models;
using Domain.ValueObjects;

namespace Domain.Interfaces;

public interface IUserRepository
{
	Task<User?> GetByIdAsync(Guid Id);
	Task<User?> GetByEmailAsync(Email email);
	Task<IReadOnlyList<User>> GetByTaskAsync(Guid taskId);
	Task<IReadOnlyList<User>> GetAllAsync(
		UserSortBy sortBy = UserSortBy.Email,
		bool desc = false);

	Task AddAsync(User user);
	void Remove(User user);
}