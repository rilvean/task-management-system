using Domain.Enums;
using Domain.Models;

namespace Domain.Interfaces;

public interface ITaskRepository
{
	Task<MyTask?> GetByIdAsync(Guid Id);
	Task<MyTask?> GetByNameAsync(string name);
	Task<IReadOnlyList<MyTask>> GetByPriorityAsync(MyTaskPriority priority);
	Task<IReadOnlyList<MyTask>> GetByStatusAsync(MyTaskStatus status);
	Task<IReadOnlyList<MyTask>> GetByUserAsync(Guid userId);
	Task<IReadOnlyList<MyTask>> GetAllAsync(
		MyTaskSortBy sortBy = MyTaskSortBy.Deadline,
		bool desc = false);

	Task AddAsync(MyTask task);
	void Remove(MyTask task);
}