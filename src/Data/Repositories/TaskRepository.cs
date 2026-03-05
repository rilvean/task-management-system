using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class TaskRepository(AppDbContext dbContext)
	: ITaskRepository
{
	public async Task AddAsync(MyTask task)
		=> await dbContext.Tasks.AddAsync(task);

	public async Task<IReadOnlyList<MyTask>> GetAllAsync(MyTaskSortBy sortBy = MyTaskSortBy.Deadline, bool desc = false)
	{
		IQueryable<MyTask> query = dbContext.Tasks.AsNoTracking();

		query = sortBy switch
		{
			MyTaskSortBy.Name
				=> desc ? query.OrderByDescending(t => t.Name)
					: query.OrderBy(t => t.Name),

			MyTaskSortBy.Deadline
				=> desc ? query.OrderByDescending(t => t.Deadline)
					: query.OrderBy(t => t.Deadline),

			MyTaskSortBy.Priority
				=> desc ? query.OrderByDescending(t => t.Priority)
					: query.OrderBy(t => t.Priority),

			MyTaskSortBy.Status
				=> desc ? query.OrderByDescending(t => t.Status)
					: query.OrderBy(t => t.Status),

			_ => query
		};

		return await query.ToListAsync();
	}

	public async Task<MyTask?> GetByIdAsync(Guid Id)
		=> await dbContext.Tasks.AsNoTracking().SingleOrDefaultAsync(t => t.Id == Id);

	public async Task<MyTask?> GetByNameAsync(string name)
		=> await dbContext.Tasks.AsNoTracking().SingleOrDefaultAsync(t => t.Name == name);

	public async Task<IReadOnlyList<MyTask>> GetByPriorityAsync(MyTaskPriority priority)
		=> await dbContext.Tasks.AsNoTracking()
		.Where(t => t.Priority == priority)
		.ToListAsync();

	public async Task<IReadOnlyList<MyTask>> GetByStatusAsync(MyTaskStatus status)
		=> await dbContext.Tasks.AsNoTracking()
		.Where(t => t.Status == status)
		.ToListAsync();

	public async Task<IReadOnlyList<MyTask>> GetByUserAsync(Guid userId)
		=> await dbContext.Tasks.AsNoTracking()
		.Where(t => t.Users.Any(u => u.Id == userId))
		.ToListAsync();

	public void Remove(MyTask task)
		=> dbContext.Tasks.Remove(task);
}