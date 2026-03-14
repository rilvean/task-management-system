using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Submodels;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class TaskRepository(AppDbContext dbContext)
	: ITaskRepository
{
	public async Task AddAsync(WorkTask task)
		=> await dbContext.Tasks.AddAsync(task);

	public async Task<IReadOnlyList<WorkTask>> GetAllAsync(MyTaskSortBy sortBy = MyTaskSortBy.Deadline, bool desc = false)
	{
		IQueryable<WorkTask> query = dbContext.Tasks.AsNoTracking()
			.Include(t => EF.Property<IEnumerable<Assignment>>(t, "_assignments"))
				.ThenInclude(a => a.User);

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

	public async Task<WorkTask?> GetByIdAsync(Guid Id)
		=> await dbContext.Tasks
			.Include(t => EF.Property<IEnumerable<Assignment>>(t, "_assignments"))
				.ThenInclude(a => a.User)
			.SingleOrDefaultAsync(t => t.Id == Id);

	public async Task<WorkTask?> GetByNameAsync(string name)
		=> await dbContext.Tasks
			.Include(t => EF.Property<IEnumerable<Assignment>>(t, "_assignments"))
				.ThenInclude(a => a.User)
			.SingleOrDefaultAsync(t => t.Name == name);

	public async Task<IReadOnlyList<WorkTask>> GetByPriorityAsync(MyTaskPriority priority)
		=> await dbContext.Tasks.AsNoTracking()
			.Include(t => EF.Property<IEnumerable<Assignment>>(t, "_assignments"))
				.ThenInclude(a => a.User)
			.Where(t => t.Priority == priority)
			.ToListAsync();

	public async Task<IReadOnlyList<WorkTask>> GetByStatusAsync(MyTaskStatus status)
		=> await dbContext.Tasks.AsNoTracking()
			.Include(t => EF.Property<IEnumerable<Assignment>>(t, "_assignments"))
				.ThenInclude(a => a.User)
			.Where(t => t.Status == status)
			.ToListAsync();

	public void Remove(WorkTask task)
		=> dbContext.Tasks.Remove(task);
}