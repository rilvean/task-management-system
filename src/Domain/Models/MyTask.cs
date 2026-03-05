using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Models;

public class MyTask
{
	#region Fields
	private string _name = null!;

	private readonly List<User> _users = [];

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
	public string? Description { get; private set; }

	public DateTime? Deadline { get; private set; }

	public MyTaskPriority Priority { get; private set; } = MyTaskPriority.Medium;
	public MyTaskStatus Status { get; private set; } = MyTaskStatus.Active;

	public IReadOnlyList<User> Users => _users;
	#endregion

	protected MyTask() { }

	public MyTask(string name, string? description)
	{
		Name = name;
		Description = description;
	}

	public void Rename(string newName) => Name = newName;
	public void ChangeDescription(string? newDescription) => Description = newDescription;
	public void SetDeadline(DateTime? deadline)
	{
		if (deadline.HasValue && deadline <= DateTime.UtcNow)
			throw new ArgumentException(nameof(deadline));

		Deadline = deadline;
	}

	public void ChangePriority(MyTaskPriority newPriority) => Priority = newPriority;
	public void ChangeStatus(MyTaskStatus newStatus) => Status = newStatus;

	#region Methods for executors
	public void AssignExecuter(User user)
	{
		if (user is null) throw new ArgumentNullException(nameof(user));

		if (user.Role is not UserRole.Employee) throw new ArgumentException(nameof(user.Role));

		if (_users.Any(u => u.Id == user.Id))
			throw new RepeatException(nameof(user));

		_users.Add(user);
		user.AddTask(this);
	}

	public void RemoveExecuter(User user)
	{
		if (user is null) throw new ArgumentNullException(nameof(user));

		if (user.Role is not UserRole.Employee) throw new ArgumentException(nameof(user.Role));

		var executer = _users.SingleOrDefault(u => u.Id == user.Id) ??
			throw new NotFoundException(nameof(user));

		if (_users.Remove(executer)) executer.RemoveTask(this);
	}
	#endregion
}