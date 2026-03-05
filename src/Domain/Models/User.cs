using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Models;

public class User
{
	#region Fields
	private string _name = null!;

	private readonly List<MyTask> _tasks = [];

	public Guid Id { get; private set; } = Guid.NewGuid();
	public string Name
	{
		get => _name;
		private set
		{
			if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(Name));
			_name = value;
		}
	}
	public Email Email { get; private set; } = null!;
	public PasswordHash PasswordHash { get; private set; } = null!;
	public UserRole Role { get; private set; }

	public IReadOnlyList<MyTask> Tasks => _tasks;
	#endregion

	protected User() { }

	public User(string name, Email email, PasswordHash passwordHash, UserRole role)
	{
		Name = name;
		Email = email;
		PasswordHash = passwordHash;
		Role = role;
	}

	public void ChangeName(string newName) => Name = newName;
	public void ChangeEmail(Email newEmail) => Email = newEmail;
	public void ChangePassword(PasswordHash newPasswordHash) => PasswordHash = newPasswordHash;
	public void ChangeRole(UserRole newRole) => Role = newRole;

	#region Internal methods
	internal void AddTask(MyTask task)
	{
		if (task is null) throw new ArgumentNullException(nameof(task));

		if (!_tasks.Any(t => t.Id == task.Id))
			_tasks.Add(task);
	}

	internal void RemoveTask(MyTask task)
	{
		if (task is null) throw new ArgumentNullException(nameof(task));

		var existing = _tasks.SingleOrDefault(t => t.Id == task.Id);
		if (existing is not null)
			_tasks.Remove(existing);
	}
	#endregion
}