using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository(AppDbContext dbContext)
	: IUserRepository
{
	public async Task AddAsync(User user)
		=> await dbContext.Users.AddAsync(user);

	public async Task<IReadOnlyList<User>> GetAllAsync(UserSortBy sortBy = UserSortBy.Email, bool desc = false)
	{
		IQueryable<User> query = dbContext.Users.AsNoTracking();

		query = sortBy switch
		{
			UserSortBy.Name
				=> desc ? query.OrderByDescending(u => u.Name)
					: query.OrderBy(u => u.Name),

			UserSortBy.Email
				=> desc ? query.OrderByDescending(u => u.Email)
					: query.OrderBy(u => u.Email),

			UserSortBy.Role
				=> desc ? query.OrderByDescending(u => u.Role)
					: query.OrderBy(u => u.Role),

			_ => query
		};

		return await query.ToListAsync();
	}

	public async Task<User?> GetByEmailAsync(Email email)
		=> await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email == email);

	public async Task<User?> GetByIdAsync(Guid Id)
		=> await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == Id);

	public async Task<IReadOnlyList<User>> GetByTaskAsync(Guid taskId)
		=> await dbContext.Users.AsNoTracking()
		.Where(u => u.Tasks.Any(t => t.Id == taskId))
		.ToListAsync();

	public void Remove(User user)
		=> dbContext.Users.Remove(user);
}