using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Domain.ValueObjects;

namespace DomainTests;

public class AssignmentTests
{
	private readonly Email emailTest = Email.From("test@example.com");
	private readonly PasswordHash passwordTest = PasswordHasher.Hash("hash");

	[Fact]
	public void AssignExecutor_ShouldAddUserAndSyncTask()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		var task = new WorkTask("Task", null);

		task.AssignExecutor(user);

		Assert.Single(task.Executors);
		Assert.Single(user.Tasks);
	}

	[Fact]
	public void RemoveExecutor_ShouldRemoveUserAndSyncTask()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		var task = new WorkTask("Task", null);

		task.AssignExecutor(user);
		task.RemoveExecutor(user);

		Assert.DoesNotContain(user, task.Executors);
		Assert.DoesNotContain(task, user.Tasks);
	}

	[Fact]
	public void CompleteBy_ShouldCompleteAssignment()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		var task = new WorkTask("Task", null);

		task.AssignExecutor(user);

		task.CompleteBy(user);

		Assert.Equal(WorkTaskStatus.Completed, task.Status);
	}

	[Fact]
	public void CompleteBy_WhenAllAssignmentsCompleted_ShouldSetTaskCompleted()
	{
		var user1 = new User("John", emailTest, passwordTest, UserRole.Employee);
		var user2 = new User("John", Email.From("other@mail.com"), passwordTest, UserRole.Employee);

		var task = new WorkTask("Task", null);

		task.AssignExecutor(user1);
		task.AssignExecutor(user2);

		task.CompleteBy(user1);
		task.CompleteBy(user2);

		Assert.Equal(WorkTaskStatus.Completed, task.Status);
	}

	[Fact]
	public void CompleteBy_WhenNotAllAssignmentsCompleted_ShouldNotSetTaskCompleted()
	{
		var user1 = new User("John", emailTest, passwordTest, UserRole.Employee);
		var user2 = new User("John", Email.From("other@mail.com"), passwordTest, UserRole.Employee);

		var task = new WorkTask("Task", null);

		task.AssignExecutor(user1);
		task.AssignExecutor(user2);

		task.CompleteBy(user2);

		Assert.Equal(WorkTaskStatus.Active, task.Status);
	}
}