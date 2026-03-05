using Domain.Enums;
using Domain.Models;

namespace DomainTests;

public class TaskNegativeTests
{
	[Fact]
	public void Task_Creation_ShouldThrow_WhenNameIsNullOrWhitespace()
	{
		Assert.Throws<ArgumentNullException>(() => new MyTask(null!, null));
		Assert.Throws<ArgumentNullException>(() => new MyTask("", null));
		Assert.Throws<ArgumentNullException>(() => new MyTask("   ", null));
	}

	[Fact]
	public void SetDeadline_ShouldThrow_WhenDeadlineIsInPast()
	{
		var task = new MyTask("Task", null);
		var past = DateTime.UtcNow.AddSeconds(-1);
		Assert.Throws<ArgumentException>(() => task.SetDeadline(past));
	}
}