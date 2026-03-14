namespace Domain.Models.Submodels;

public class Assignment
{
	#region Fields
	public Guid TaskId { get; private set; }
	public Guid UserId { get; private set; }
	public bool IsCompleted { get; private set; }

	public WorkTask Task { get; private set; } = null!;
	public User User { get; private set; } = null!;
	#endregion

	private Assignment() { }

	internal Assignment(WorkTask task, User user)
	{
		Task = task;
		User = user;

		TaskId = task.Id;
		UserId = user.Id;
	}

	internal void Complete()
		=> IsCompleted = true;
}