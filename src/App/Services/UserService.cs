using Domain.Enums;
using App.Interfaces;
using Domain.Models;
using Domain.ValueObjects;
using App.Enums;

namespace App.Services;

public class UserService(IUnitOfWork unitOfWork)
{
	private readonly ServiceHelper _helper = new ServiceHelper(unitOfWork);

	public async Task<IReadOnlyList<User>> GetPerTaskAsync(Guid taskId)
	{
		var task = await _helper.EnsureTaskExistAsync(taskId);

		return task.Executors.ToList();
	}

	public async Task<IReadOnlyList<User>> GetAllSortedAsync(UserSortBy sortBy = default, bool descending = default)
	{
		return await unitOfWork.UserRepository.GetAllSortedAsync(sortBy, descending);
	}

	public async Task<Guid> CreateAsync(string name, Email email, PasswordHash password, UserRole role)
	{
		var user = new User(name, email, password, role);

		await unitOfWork.UserRepository.AddAsync(user);
		await unitOfWork.SaveChangesAsync();

		return user.Id;
	}

	public async Task DeleteAsync(Guid userId)
	{
		var user = await _helper.EnsureUserExistAsync(userId);

		unitOfWork.UserRepository.Delete(user);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task RenameAsync(Guid userId, string newName)
	{
		var user = await _helper.EnsureUserExistAsync(userId);

		user.ChangeName(newName);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangeEmailAsync(Guid userId, Email newEmail)
	{
		var user = await _helper.EnsureUserExistAsync(userId);

		user.ChangeEmail(newEmail);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangePasswordAsync(Guid userId, PasswordHash newPassword)
	{
		var user = await _helper.EnsureUserExistAsync(userId);

		user.ChangePassword(newPassword);

		await unitOfWork.SaveChangesAsync();
	}

	public async Task ChangeRoleAsync(Guid userId, UserRole role)
	{
		var user = await _helper.EnsureUserExistAsync(userId);

		foreach (var t in user.Tasks.ToList())
			t.RemoveExecutor(user);

		user.ChangeRole(role);

		await unitOfWork.SaveChangesAsync();
	}
}