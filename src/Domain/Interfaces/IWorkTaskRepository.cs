using Domain.Enums;
using Domain.Models;

namespace Domain.Interfaces;

public interface IWorkTaskRepository
{
	Task<WorkTask?> GetByIdAsync(Guid Id);
	Task<WorkTask?> GetByNameAsync(string name);
	Task<IReadOnlyList<WorkTask>> GetByPriorityAsync(WorkTaskPriority priority);
	Task<IReadOnlyList<WorkTask>> GetByStatusAsync(WorkTaskStatus status);
	Task<IReadOnlyList<WorkTask>> GetAllSortedAsync(WorkTaskSortBy sortBy = default, bool desc = default);

	Task AddAsync(WorkTask task);
	void Delete(WorkTask task);
}