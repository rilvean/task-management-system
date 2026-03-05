using Domain.Enums;
using Domain.Models;

namespace DomainTests;

public class TaskTests
{
	[Fact]
	public void Task_Creation_ShouldInitializeProperties()
	{
		var task = new MyTask("Task1", "Description");

		Assert.Equal("Task1", task.Name);
		Assert.Equal("Description", task.Description);
		Assert.Null(task.Deadline);
		Assert.Equal(MyTaskPriority.Medium, task.Priority);
		Assert.Equal(MyTaskStatus.Active, task.Status);
		Assert.Empty(task.Users);
		Assert.NotEqual(Guid.Empty, task.Id);
	}

	[Fact]
	public void Rename_ShouldUpdateName()
	{
		var task = new MyTask("OldName", null);

		task.Rename("NewName");

		Assert.Equal("NewName", task.Name);
	}

	[Fact]
	public void ChangeDescription_ShouldUpdateDescription()
	{
		var task = new MyTask("Task", "Old");

		task.ChangeDescription("New");

		Assert.Equal("New", task.Description);
	}

	[Fact]
	public void SetDeadline_ShouldSet_WhenDeadlineInFuture()
	{
		var task = new MyTask("Task", null);
		var future = DateTime.UtcNow.AddMinutes(5);

		task.SetDeadline(future);

		Assert.Equal(future, task.Deadline);
	}

	[Fact]
	public void SetDeadline_ShouldSet_WhenDeadlineIsNull()
	{
		var task = new MyTask("Task", null);

		task.SetDeadline(null);

		Assert.Equal(null!, task.Deadline);
	}

	[Fact]
	public void ChangePriority_ShouldUpdatePriority()
	{
		var task = new MyTask("Task", null);

		task.ChangePriority(MyTaskPriority.High);

		Assert.Equal(MyTaskPriority.High, task.Priority);
	}

	[Fact]
	public void ChangeStatus_ShouldUpdateStatus()
	{
		var task = new MyTask("Task", null);

		task.ChangeStatus(MyTaskStatus.Completed);

		Assert.Equal(MyTaskStatus.Completed, task.Status);
	}

	[Fact]
	public void ChangePriority_ShouldNotThrow_ForAnyEnumValue()
	{
		var task = new MyTask("Task", null);
		foreach (MyTaskPriority value in Enum.GetValues(typeof(MyTaskPriority)))
		{
			task.ChangePriority(value);
			Assert.Equal(value, task.Priority);
		}
	}

	[Fact]
	public void ChangeStatus_ShouldNotThrow_ForAnyEnumValue()
	{
		var task = new MyTask("Task", null);
		foreach (MyTaskStatus value in Enum.GetValues(typeof(MyTaskStatus)))
		{
			task.ChangeStatus(value);
			Assert.Equal(value, task.Status);
		}
	}
}