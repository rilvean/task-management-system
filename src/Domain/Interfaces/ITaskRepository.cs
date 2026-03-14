using Domain.Enums;
using Domain.Models;

namespace Domain.Interfaces;

public interface ITaskRepository
{
	Task<WorkTask?> GetByIdAsync(Guid Id);
	Task<WorkTask?> GetByNameAsync(string name);
	Task<IReadOnlyList<WorkTask>> GetByPriorityAsync(MyTaskPriority priority);
	Task<IReadOnlyList<WorkTask>> GetByStatusAsync(MyTaskStatus status);
	Task<IReadOnlyList<WorkTask>> GetAllAsync(
		MyTaskSortBy sortBy = MyTaskSortBy.Deadline,
		bool desc = false);

	Task AddAsync(WorkTask task);
	void Remove(WorkTask task);
}