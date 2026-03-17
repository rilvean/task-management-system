using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;

namespace App.Services;

public class UserService(IUnitOfWork unitOfWork)
{
	private readonly ServiceHelper _helper = new ServiceHelper(unitOfWork);

	public async Task<IReadOnlyList<User>> GetPerTaskAsync(Guid managerId, Guid taskId)
	{
		await _helper.EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await _helper.EnsureTaskExistAsync(taskId);

		return task.Executors.ToList();
	}

	public async Task<IReadOnlyList<User>> GetAllSortedAsync(Guid adminId, UserSortBy sortBy = default, bool descending = default)
	{
		await _helper.EnsureAccessAsync(adminId, UserRole.Admin);

		return await unitOfWork.UserRepository.GetAllSortedAsync(sortBy, descending);
	}

	public async Task<Guid> CreateAsync(Guid adminId, string name, Email email, PasswordHash password, UserRole role)
	{
		await _helper.EnsureAccessAsync(adminId, UserRole.Admin);

		var user = new User(name, email, password, role);

		await unitOfWork.UserRepository.AddAsync(user);
		await unitOfWork.SaveChangesAsync();

		return user.Id;
	}

	public async Task DeleteAsync(Guid adminId, Guid userId)
	{
		await _helper.EnsureAccessAsync(adminId, UserRole.Admin);
		var user = await _helper.EnsureUserExistAsync(userId);

		unitOfWork.UserRepository.Delete(user);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task RenameAsync(Guid adminId, Guid userId, string newName)
	{
		await _helper.EnsureAccessAsync(adminId, UserRole.Admin);
		var user = await _helper.EnsureUserExistAsync(userId);

		user.ChangeName(newName);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangeEmailAsync(Guid adminId, Guid userId, Email newEmail)
	{
		await _helper.EnsureAccessAsync(adminId, UserRole.Admin);
		var user = await _helper.EnsureUserExistAsync(userId);

		user.ChangeEmail(newEmail);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangePasswordAsync(Guid adminId, Guid userId, PasswordHash newPassword)
	{
		await _helper.EnsureAccessAsync(adminId, UserRole.Admin);
		var user = await _helper.EnsureUserExistAsync(userId);

		user.ChangePassword(newPassword);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangeRoleAsync(Guid adminId, Guid userId, UserRole role)
	{
		await _helper.EnsureAccessAsync(adminId, UserRole.Admin);
		var user = await _helper.EnsureUserExistAsync(userId);

		foreach (var t in user.Tasks.ToList())
			t.RemoveExecutor(user);

		user.ChangeRole(role);

		await unitOfWork.SaveChangesAsync();
	}
}