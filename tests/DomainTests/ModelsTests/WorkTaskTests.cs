using Domain.Enums;
using Domain.Models;

namespace DomainTests.ModelsTests;

public class WorkTaskTests
{
	[Fact]
	public void WorkTask_Creation_ShouldInitializeProperties()
	{
		var task = new WorkTask("Task1", "Description");

		Assert.Equal("Task1", task.Name);
		Assert.Equal("Description", task.Description);
		Assert.Null(task.Deadline);
		Assert.Equal(WorkTaskPriority.Medium, task.Priority);
		Assert.Equal(WorkTaskStatus.Active, task.Status);
		Assert.Empty(task.Executors);
		Assert.NotEqual(Guid.Empty, task.Id);
	}

	[Fact]
	public void ChangeName_ShouldUpdateName()
	{
		var task = new WorkTask("OldName", null);

		task.ChangeName("NewName");

		Assert.Equal("NewName", task.Name);
	}

	[Fact]
	public void ChangeDescription_ShouldUpdateDescription()
	{
		var task = new WorkTask("Task", "Old");

		task.ChangeDescription("New");

		Assert.Equal("New", task.Description);
	}

	[Fact]
	public void SetDeadline_ShouldSet_WhenDeadlineInFuture()
	{
		var task = new WorkTask("Task", null);
		var future = DateTime.UtcNow.AddMinutes(5);

		task.SetDeadline(future);

		Assert.Equal(future, task.Deadline);
	}

	[Fact]
	public void SetDeadline_ShouldSet_WhenDeadlineIsNull()
	{
		var task = new WorkTask("Task", null);

		task.SetDeadline(null);

		Assert.Equal(null!, task.Deadline);
	}

	[Fact]
	public void ChangePriority_ShouldUpdatePriority()
	{
		var task = new WorkTask("Task", null);

		task.ChangePriority(WorkTaskPriority.High);

		Assert.Equal(WorkTaskPriority.High, task.Priority);
	}

	[Fact]
	public void ChangeStatus_ShouldUpdateStatus()
	{
		var task = new WorkTask("Task", null);

		task.ChangeStatus(WorkTaskStatus.Completed);

		Assert.Equal(WorkTaskStatus.Completed, task.Status);
	}

	[Fact]
	public void ChangePriority_ShouldNotThrow_ForAnyEnumValue()
	{
		var task = new WorkTask("Task", null);
		foreach (WorkTaskPriority value in Enum.GetValues(typeof(WorkTaskPriority)))
		{
			task.ChangePriority(value);
			Assert.Equal(value, task.Priority);
		}
	}

	[Fact]
	public void ChangeStatus_ShouldNotThrow_ForAnyEnumValue()
	{
		var task = new WorkTask("Task", null);
		foreach (WorkTaskStatus value in Enum.GetValues(typeof(WorkTaskStatus)))
		{
			task.ChangeStatus(value);
			Assert.Equal(value, task.Status);
		}
	}
}