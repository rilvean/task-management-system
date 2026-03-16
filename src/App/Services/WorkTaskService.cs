using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;

namespace App.Services;

public class WorkTaskService(IUnitOfWork unitOfWork)
{
	private readonly ServiceHelper _helper = new ServiceHelper(unitOfWork);

	public async Task<IReadOnlyList<WorkTask>> GetPerEmployeeAsync(Guid employeeId)
	{
		var employee = await _helper.EnsureAccessAsync(employeeId, UserRole.Employee);

		return employee.Tasks.ToList();
	}

	public async Task<IReadOnlyList<WorkTask>> GetAllSortedAsync(Guid managerId, WorkTaskSortBy sortBy = default, bool descending = default)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);

		return await unitOfWork.WorkTaskRepository.GetAllSortedAsync(sortBy, descending);
	}

	public async Task<IReadOnlyList<WorkTask>> GetByPriorityAsync(Guid managerId, WorkTaskPriority priority)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);

		return await unitOfWork.WorkTaskRepository.GetByPriorityAsync(priority);
	}

	public async Task<IReadOnlyList<WorkTask>> GetByStatusAsync(Guid managerId, WorkTaskStatus status)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);

		return await unitOfWork.WorkTaskRepository.GetByStatusAsync(status);
	}

	public async Task<Guid> CreateAsync(Guid managerId, string name, string? description)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);

		var task = new WorkTask(name, description);

		await unitOfWork.WorkTaskRepository.AddAsync(task);
		await unitOfWork.SaveChangesAsync();

		return task.Id;
	}

	public async Task DeleteAsync(Guid managerId, Guid taskId)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await _helper.EnsureTaskExistAsync(taskId);

		unitOfWork.WorkTaskRepository.Delete(task);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task AssignExecutorAsync(Guid managerId, Guid taskId, Guid employeeId)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await _helper.EnsureTaskExistAsync(taskId);
		var employee = await _helper.EnsureAccessAsync(employeeId, UserRole.Employee);

		task.AssignExecutor(employee);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task RemoveExecutorAsync(Guid managerId, Guid taskId, Guid employeeId)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await _helper.EnsureTaskExistAsync(taskId);
		var employee = await _helper.EnsureAccessAsync(employeeId, UserRole.Employee);

		task.RemoveExecutor(employee);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangePriorityAsync(Guid managerId, Guid taskId, WorkTaskPriority newPriority)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.ChangePriority(newPriority);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangeStatusAsync(Guid managerId, Guid taskId, WorkTaskStatus newStatus)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.ChangeStatus(newStatus);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task RenameAsync(Guid managerId, Guid taskId, string newName)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.ChangeName(newName);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangeDescriptionAsync(Guid managerId, Guid taskId, string newDescription)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.ChangeDescription(newDescription);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task SetDeadlineAsync(Guid managerId, Guid taskId, DateTimeOffset? deadline)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.SetDeadline(deadline);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task CompleteByAsync(Guid employeeId, Guid taskId)
	{
		var employee = await _helper.EnsureAccessAsync(employeeId, UserRole.Employee);
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.CompleteBy(employee);

		await unitOfWork.SaveChangesAsync();
	}
}