using Domain.Enums;
using Domain.Models;
using Domain.Services;
using Domain.ValueObjects;

namespace DomainTests;

public class TaskUserRelationshipTests
{
	private Email emailTest = Email.From("test@example.com");
	private PasswordHash passwordTest = PasswordHash.From(PasswordHasher.Hash("hash"));

	[Fact]
	public void AssignExecuter_ShouldAddUserAndSyncTask()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		var task = new MyTask("Task", null);

		task.AssignExecuter(user);

		Assert.Contains(user, task.Users);
		Assert.Contains(task, user.Tasks);
	}

	[Fact]
	public void RemoveExecuter_ShouldRemoveUserAndSyncTask()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		var task = new MyTask("Task", null);

		task.AssignExecuter(user);
		task.RemoveExecuter(user);

		Assert.DoesNotContain(user, task.Users);
		Assert.DoesNotContain(task, user.Tasks);
	}
}