using Domain.Enums;
using App.Interfaces;
using Domain.Models;
using App.Enums;

namespace App.Services;

public class WorkTaskService(IUnitOfWork unitOfWork)
{
	private readonly ServiceHelper _helper = new ServiceHelper(unitOfWork);

	public async Task<IReadOnlyList<WorkTask>> GetPerEmployeeAsync(Guid employeeId)
	{
		var employee = await _helper.EnsureUserExistAsync(employeeId);

		return employee.Tasks.ToList();
	}

	public async Task<IReadOnlyList<WorkTask>> GetAllSortedAsync(WorkTaskSortBy sortBy = default, bool descending = default)
	{
		return await unitOfWork.WorkTaskRepository.GetAllSortedAsync(sortBy, descending);
	}

	public async Task<IReadOnlyList<WorkTask>> GetByPriorityAsync(WorkTaskPriority priority)
	{
		return await unitOfWork.WorkTaskRepository.GetByPriorityAsync(priority);
	}

	public async Task<IReadOnlyList<WorkTask>> GetByStatusAsync(WorkTaskStatus status)
	{
		return await unitOfWork.WorkTaskRepository.GetByStatusAsync(status);
	}

	public async Task<Guid> CreateAsync(string name, string? description)
	{
		var task = new WorkTask(name, description);

		await unitOfWork.WorkTaskRepository.AddAsync(task);
		await unitOfWork.SaveChangesAsync();

		return task.Id;
	}

	public async Task DeleteAsync(Guid taskId)
	{
		var task = await _helper.EnsureTaskExistAsync(taskId);

		unitOfWork.WorkTaskRepository.Delete(task);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task AssignExecutorAsync(Guid taskId, Guid employeeId)
	{
		var task = await _helper.EnsureTaskExistAsync(taskId);
		var employee = await _helper.EnsureUserExistAsync(employeeId);

		task.AssignExecutor(employee);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task RemoveExecutorAsync(Guid taskId, Guid employeeId)
	{
		var task = await _helper.EnsureTaskExistAsync(taskId);
		var employee = await _helper.EnsureUserExistAsync(employeeId);

		task.RemoveExecutor(employee);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangePriorityAsync(Guid taskId, WorkTaskPriority newPriority)
	{
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.ChangePriority(newPriority);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangeStatusAsync(Guid taskId, WorkTaskStatus newStatus)
	{
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.ChangeStatus(newStatus);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task RenameAsync(Guid taskId, string newName)
	{
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.ChangeName(newName);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangeDescriptionAsync(Guid taskId, string newDescription)
	{
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.ChangeDescription(newDescription);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task SetDeadlineAsync(Guid taskId, DateTimeOffset? deadline)
	{
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.SetDeadline(deadline);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task CompleteByAsync(Guid employeeId, Guid taskId)
	{
		var employee = await _helper.EnsureUserExistAsync(employeeId);
		var task = await _helper.EnsureTaskExistAsync(taskId);

		task.CompleteBy(employee);

		await unitOfWork.SaveChangesAsync();
	}
}