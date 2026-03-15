using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services;
using Domain.ValueObjects;

namespace DomainTests;

public class AssignmentNegativeTests
{
	private readonly Email emailTest = Email.From("test@example.com");
	private readonly PasswordHash passwordTest = PasswordHash.From(PasswordHasher.Hash("hash"));

	[Fact]
	public void AssignExecutor_ShouldThrow_WhenUserIsNull()
	{
		var task = new WorkTask("Task", null);
		Assert.Throws<ArgumentNullException>(() => task.AssignExecutor(null!));
	}

	[Fact]
	public void RemoveExecutor_ShouldThrow_WhenUserIsNull()
	{
		var task = new WorkTask("Task", null);
		Assert.Throws<ArgumentNullException>(() => task.RemoveExecutor(null!));
	}

	[Fact]
	public void AssignExecutor_ShouldThrow_WhenUserRoleIsNotEmployee()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Admin);
		var task = new WorkTask("Task", null);
		Assert.Throws<DomainRuleException>(() => task.AssignExecutor(user));
	}

	[Fact]
	public void RemoveExecutor_ShouldThrow_WhenUserRoleIsNotEmployee()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Admin);
		var task = new WorkTask("Task", null);
		Assert.Throws<DomainRuleException>(() => task.RemoveExecutor(user));
	}

	[Fact]
	public void RemoveExecutor_ShouldThrow_WhenUserNotAssigned()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		var task = new WorkTask("Task", null);
		Assert.Throws<NotFoundException>(() => task.RemoveExecutor(user));
	}

	[Fact]
	public void CompleteBy_WhenEmployeeIsNull_ShouldThrow()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		var task = new WorkTask("Task", null);

		task.AssignExecutor(user);

		Assert.Throws<ArgumentNullException>(() =>
			task.CompleteBy(null!));
	}

	[Fact]
	public void CompleteBy_WhenUserIsNotEmployee_ShouldThrow()
	{
		var employee = new User("John", emailTest, passwordTest, UserRole.Employee);
		var task = new WorkTask("Task", null);

		task.AssignExecutor(employee);

		var manager = new User("John", Email.From("other@mail.com"), passwordTest, UserRole.Manager);

		Assert.Throws<DomainRuleException>(() =>
			task.CompleteBy(manager));
	}

	[Fact]
	public void CompleteBy_WhenAssignmentNotFound_ShouldThrow()
	{
		var user = new User("John", emailTest, passwordTest, UserRole.Employee);
		var task = new WorkTask("Task", null);

		Assert.Throws<NotFoundException>(() =>
			task.CompleteBy(user));
	}
}