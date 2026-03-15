using Data;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;

namespace App.Services;

public class TaskService(
	AppDbContext db,
	ITaskRepository tr,
	IUserRepository ur)
{
	public async Task<IReadOnlyList<WorkTask>> GetPerEmployeeAsync(Guid employeeId)
	{
		var employee = await EnsureAccessAsync(employeeId, UserRole.Employee);

		return employee.Tasks.ToList();
	}

	public async Task<IReadOnlyList<WorkTask>> GetAllSortedAsync(Guid managerId, WorkTaskSortBy sortBy = default, bool descending = default)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);

		return await tr.GetAllSortedAsync(sortBy, descending);
	}

	public async Task<IReadOnlyList<WorkTask>> GetByPriorityAsync(Guid managerId, WorkTaskPriority priority)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);

		return await tr.GetByPriorityAsync(priority);
	}

	public async Task<IReadOnlyList<WorkTask>> GetByStatusAsync(Guid managerId, WorkTaskStatus status)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);

		return await tr.GetByStatusAsync(status);
	}

	public async Task<Guid> CreateAsync(Guid managerId, string name, string? description)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);

		var task = new WorkTask(name, description);

		await tr.AddAsync(task);
		await db.SaveChangesAsync();

		return task.Id;
	}

	public async Task DeleteAsync(Guid managerId, Guid taskId)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await EnsureTaskExistAsync(taskId);

		tr.Delete(task);

		await db.SaveChangesAsync();
	}

	public async Task AssignExecutorAsync(Guid managerId, Guid taskId, Guid employeeId)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await EnsureTaskExistAsync(taskId);
		var employee = await EnsureAccessAsync(employeeId, UserRole.Employee);

		task.AssignExecutor(employee);

		await db.SaveChangesAsync();
	}

	public async Task RemoveExecutorAsync(Guid managerId, Guid taskId, Guid employeeId)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await EnsureTaskExistAsync(taskId);
		var employee = await EnsureAccessAsync(employeeId, UserRole.Employee);

		task.RemoveExecutor(employee);

		await db.SaveChangesAsync();
	}

	public async Task ChangePriorityAsync(Guid managerId, Guid taskId, WorkTaskPriority newPriority)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await EnsureTaskExistAsync(taskId);

		task.ChangePriority(newPriority);

		await db.SaveChangesAsync();
	}

	public async Task ChangeStatusAsync(Guid managerId, Guid taskId, WorkTaskStatus newStatus)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await EnsureTaskExistAsync(taskId);

		task.ChangeStatus(newStatus);

		await db.SaveChangesAsync();
	}

	public async Task RenameAsync(Guid managerId, Guid taskId, string newName)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await EnsureTaskExistAsync(taskId);

		task.ChangeName(newName);

		await db.SaveChangesAsync();
	}

	public async Task ChangeDescriptionAsync(Guid managerId, Guid taskId, string newDescription)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await EnsureTaskExistAsync(taskId);

		task.ChangeDescription(newDescription);

		await db.SaveChangesAsync();
	}

	public async Task SetDeadlineAsync(Guid managerId, Guid taskId, DateTimeOffset? deadline)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await EnsureTaskExistAsync(taskId);

		task.SetDeadline(deadline);

		await db.SaveChangesAsync();
	}

	public async Task CompleteByAsync(Guid employeeId, Guid taskId)
	{
		var employee = await EnsureAccessAsync(employeeId, UserRole.Employee);
		var task = await EnsureTaskExistAsync(taskId);

		task.CompleteBy(employee);

		await db.SaveChangesAsync();
	}

	private async Task<User> EnsureAccessAsync(Guid id, UserRole requiredRole)
	{
		var user = await ur.GetByIdAsync(id)
			?? throw new NotFoundException($"User not found.");

		if (user.Role != requiredRole)
			throw new AccessException("Access denied.");

		return user;
	}
	private async Task<WorkTask> EnsureTaskExistAsync(Guid taskId)
		=> await tr.GetByIdAsync(taskId)
			?? throw new NotFoundException("Task not found.");
}