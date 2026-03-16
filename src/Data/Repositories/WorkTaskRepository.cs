using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class WorkTaskRepository(AppDbContext dbContext) : IWorkTaskRepository
{
	public async Task AddAsync(WorkTask task)
		=> await dbContext.Tasks.AddAsync(task);

	public async Task<IReadOnlyList<WorkTask>> GetAllSortedAsync(WorkTaskSortBy sortBy = default, bool descending = default)
	{
		IQueryable<WorkTask> query = dbContext.Tasks.AsNoTracking()
			.IncludeAssignments();

		query = sortBy switch
		{
			WorkTaskSortBy.Deadline
				=> descending ? query.OrderByDescending(t => t.Deadline)
					: query.OrderBy(t => t.Deadline),

			WorkTaskSortBy.Priority
				=> descending ? query.OrderByDescending(t => t.Priority)
				: query.OrderBy(t => t.Priority),

			WorkTaskSortBy.Status
				=> descending ? query.OrderByDescending(t => t.Status)
					: query.OrderBy(t => t.Status),

			WorkTaskSortBy.Name
				=> descending ? query.OrderByDescending(t => t.Name)
					: query.OrderBy(t => t.Name),

			_ => query
		};

		return await query.ToListAsync();
	}

	public async Task<WorkTask?> GetByIdAsync(Guid Id)
		=> await dbContext.Tasks
			.IncludeAssignments()
			.SingleOrDefaultAsync(t => t.Id == Id);

	public async Task<WorkTask?> GetByNameAsync(string name)
		=> await dbContext.Tasks
			.IncludeAssignments()
			.SingleOrDefaultAsync(t => t.Name == name);

	public async Task<IReadOnlyList<WorkTask>> GetByPriorityAsync(WorkTaskPriority priority)
		=> await dbContext.Tasks.AsNoTracking()
			.IncludeAssignments()
			.Where(t => t.Priority == priority)
			.ToListAsync();

	public async Task<IReadOnlyList<WorkTask>> GetByStatusAsync(WorkTaskStatus status)
		=> await dbContext.Tasks.AsNoTracking()
			.IncludeAssignments()
			.Where(t => t.Status == status)
			.ToListAsync();

	public void Delete(WorkTask task)
		=> dbContext.Tasks.Remove(task);
}