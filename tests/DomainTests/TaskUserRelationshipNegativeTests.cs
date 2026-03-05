using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services;
using Domain.ValueObjects;

namespace DomainTests;

public class TaskUserRelationshipNegativeTests
{
	private Email emailTest = Email.From("test@example.com");
	private PasswordHash passwordTest = PasswordHash.From(PasswordHasher.Hash("hash"));

	[Fact]
	public void AssignExecuter_ShouldThrow_WhenUserIsNull()
	{
		var task = new MyTask("Task", null);
		Assert.Throws<ArgumentNullException>(() => task.AssignExecuter(null!));
	}

	[Fact]
	public void RemoveExecuter_ShouldThrow_WhenUserIsNull()
	{
		var task = new MyTask("Task", null);
		Assert.Throws<ArgumentNullException>(() => task.RemoveExecuter(null!));
	}

	[Fact]
	public void AssignExecuter_ShouldThrow_WhenUserRoleIsNotEmployee()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Admin);
		var task = new MyTask("Task", null);
		Assert.Throws<ArgumentException>(() => task.AssignExecuter(user));
	}

	[Fact]
	public void RemoveExecuter_ShouldThrow_WhenUserRoleIsNotEmployee()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Admin);
		var task = new MyTask("Task", null);
		Assert.Throws<ArgumentException>(() => task.RemoveExecuter(user));
	}

	[Fact]
	public void RemoveExecuter_ShouldThrow_WhenUserNotAssigned()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		var task = new MyTask("Task", null);
		Assert.Throws<NotFoundException>(() => task.RemoveExecuter(user));
	}
}