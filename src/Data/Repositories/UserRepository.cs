using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Submodels;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserRepository(AppDbContext dbContext)
	: IUserRepository
{
	public async Task AddAsync(User user)
		=> await dbContext.Users.AddAsync(user);

	public async Task<IReadOnlyList<User>> GetAllSortedAsync(UserSortBy sortBy = default, bool descending = default)
	{
		IQueryable<User> query = dbContext.Users.AsNoTracking()
			.Include(u => EF.Property<IEnumerable<Assignment>>(u, "_assignments"))
				.ThenInclude(a => a.Task);

		query = sortBy switch
		{
			UserSortBy.Email
				=> descending ? query.OrderByDescending(u => u.Email)
					: query.OrderBy(u => u.Email),

			UserSortBy.Role
				=> descending ? query.OrderByDescending(u => u.Role)
					: query.OrderBy(u => u.Role),

			UserSortBy.Name
				=> descending ? query.OrderByDescending(u => u.Name)
					: query.OrderBy(u => u.Name),

			_ => query
		};

		return await query.ToListAsync();
	}

	public async Task<User?> GetByEmailAsync(Email email)
		=> await dbContext.Users
			.Include(u => EF.Property<IEnumerable<Assignment>>(u, "_assignments"))
				.ThenInclude(a => a.Task)
			.SingleOrDefaultAsync(u => u.Email == email);

	public async Task<User?> GetByIdAsync(Guid Id)
		=> await dbContext.Users
			.Include(u => EF.Property<IEnumerable<Assignment>>(u, "_assignments"))
				.ThenInclude(a => a.Task)
			.SingleOrDefaultAsync(u => u.Id == Id);

	public void Delete(User user)
		=> dbContext.Users.Remove(user);
}