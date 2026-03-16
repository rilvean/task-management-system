using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;

namespace App;

sealed class ServiceHelper(IUnitOfWork unitOfWork)
{
	internal async Task<User> EnsureAccessAsync(Guid id, UserRole requiredRole)
	{
		var user = await EnsureUserExistAsync(id);

		if (user.Role != requiredRole)
			throw new AccessException("Access denied.");

		return user;
	}

	internal async Task<User> EnsureUserExistAsync(Guid id)
	=> await unitOfWork.UserRepository.GetByIdAsync(id)
		?? throw new NotFoundException("User not found.");
	internal async Task<WorkTask> EnsureTaskExistAsync(Guid taskId)
		=> await unitOfWork.WorkTaskRepository.GetByIdAsync(taskId)
			?? throw new NotFoundException("Task not found.");
}