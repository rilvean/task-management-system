using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models.Submodels;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class WorkTask : IAuditable
{
	#region Fields
	private const int MAX_NAME_LENGTH = 400;
	private const int MAX_DESC_LENGTH = 2000;

	private string _name = null!;
	private string? _description;

	private readonly List<Assignment> _assignments = [];

	public Guid Id { get; private set; } = Guid.NewGuid();
	public string Name
	{
		get => _name;
		private set
		{
			if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(Name));
			if (value.Length > MAX_NAME_LENGTH) throw new ArgumentOutOfRangeException(nameof(Name));
			_name = value;
		}
	}

	public string? Description
	{
		get => _description;
		private set
		{
			if (value?.Length > MAX_DESC_LENGTH) throw new ArgumentOutOfRangeException(nameof(Description));
			_description = value;
		}
	}

	public DateTimeOffset? Deadline { get; private set; }

	public MyTaskPriority Priority { get; private set; } = MyTaskPriority.Medium;
	public MyTaskStatus Status { get; private set; } = MyTaskStatus.Active;

	public IEnumerable<User> Users => _assignments.Select(x => x.User);
	#endregion

	private WorkTask() { }

	public WorkTask(string name, string? description)
	{
		Name = name;
		Description = description;
	}

	public void Rename(string newName) => Name = newName;
	public void ChangeDescription(string? newDescription) => Description = newDescription;
	public void SetDeadline(DateTimeOffset? deadline)
	{
		if (deadline.HasValue && deadline <= DateTimeOffset.UtcNow)
			throw new ArgumentException(nameof(deadline));

		Deadline = deadline;
	}

	public void ChangePriority(MyTaskPriority newPriority) => Priority = newPriority;
	public void ChangeStatus(MyTaskStatus newStatus) => Status = newStatus;

	#region Methods for executors
	public void AssignExecutor(User employee)
	{
		if (employee is null) throw new ArgumentNullException(nameof(employee));

		EnsureEmployee(employee);

		if (_assignments.Any(a => a.UserId == employee.Id))
			throw new RepeatException(nameof(employee));

		var assignment = new Assignment(this, employee);

		_assignments.Add(assignment);
		employee.AddAssignment(assignment);
	}

	public void RemoveExecutor(User employee)
	{
		if (employee is null) throw new ArgumentNullException(nameof(employee));

		EnsureEmployee(employee);

		var assignment = _assignments.SingleOrDefault(a => a.UserId == employee.Id) ??
			throw new NotFoundException(nameof(employee));

		if (_assignments.Remove(assignment))
			employee.RemoveAssignment(assignment);
	}

	public void CompleteBy(User employee)
	{
		if (employee is null) throw new ArgumentNullException(nameof(employee));

		EnsureEmployee(employee);

		var assignment = _assignments.SingleOrDefault(x => x.UserId == employee.Id)
			?? throw new NotFoundException("Assignment not found.");

		assignment.Complete();

		var isAllComplete = _assignments.All(x => x.IsCompleted);

		if (isAllComplete)
			Status = MyTaskStatus.Completed;
	}
	#endregion

	private static void EnsureEmployee(User user)
	{
		if (user.Role is not UserRole.Employee)
			throw new DomainRuleException("Only employee allowed");
	}
}