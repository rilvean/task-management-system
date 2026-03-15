using Data;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Domain.ValueObjects;

namespace App.Services;

public class UserService(
	AppDbContext db,
	IUserRepository ur,
	ITaskRepository tr)
{
	public async Task<IReadOnlyList<User>> GetPerTaskAsync(Guid managerId, Guid taskId)
	{
		await EnsureAccessAsync(managerId, UserRole.Manager);
		var task = await EnsureTaskExistAsync(taskId);

		return task.Executors.ToList();
	}

	public async Task<IReadOnlyList<User>> GetAllSortedAsync(Guid adminId, UserSortBy sortBy = default, bool descending = default)
	{
		await EnsureAccessAsync(adminId, UserRole.Admin);

		return await ur.GetAllSortedAsync(sortBy, descending);
	}

	public async Task<Guid> CreateAsync(Guid adminId, string name, Email email, PasswordHash password, UserRole role)
	{
		await EnsureAccessAsync(adminId, UserRole.Admin);

		var user = new User(name, email, password, role);

		await ur.AddAsync(user);
		await db.SaveChangesAsync();

		return user.Id;
	}

	public async Task DeleteAsync(Guid adminId, Guid userId)
	{
		await EnsureAccessAsync(adminId, UserRole.Admin);
		var user = await EnsureUserExistAsync(userId);

		ur.Delete(user);

		await db.SaveChangesAsync();
	}

	public async Task RenameAsync(Guid adminId, Guid userId, string newName)
	{
		await EnsureAccessAsync(adminId, UserRole.Admin);
		var user = await EnsureUserExistAsync(userId);

		user.ChangeName(newName);

		await db.SaveChangesAsync();
	}

	public async Task ChangeEmailAsync(Guid adminId, Guid userId, Email newEmail)
	{
		await EnsureAccessAsync(adminId, UserRole.Admin);
		var user = await EnsureUserExistAsync(userId);

		user.ChangeEmail(newEmail);

		await db.SaveChangesAsync();
	}

	public async Task ChangePasswordAsync(Guid adminId, Guid userId, PasswordHash newPassword)
	{
		await EnsureAccessAsync(adminId, UserRole.Admin);
		var user = await EnsureUserExistAsync(userId);

		user.ChangePassword(newPassword);

		await db.SaveChangesAsync();
	}

	public async Task ChangeRoleAsync(Guid adminId, Guid userId, UserRole role)
	{
		await EnsureAccessAsync(adminId, UserRole.Admin);
		var user = await EnsureUserExistAsync(userId);

		user.ChangeRole(role);

		await db.SaveChangesAsync();
	}

	private async Task<User> EnsureAccessAsync(Guid id, UserRole requiredRole)
	{
		var user = await EnsureUserExistAsync(id);

		if (user.Role != requiredRole)
			throw new AccessException("Access denied.");

		return user;
	}
	private async Task<User> EnsureUserExistAsync(Guid id)
	=> await ur.GetByIdAsync(id)
		?? throw new NotFoundException($"User not found.");
	private async Task<WorkTask> EnsureTaskExistAsync(Guid taskId)
		=> await tr.GetByIdAsync(taskId)
			?? throw new NotFoundException("Task not found.");
}