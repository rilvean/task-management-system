using Domain.Models;

namespace DomainTests;

public class TaskNegativeTests
{
	[Fact]
	public void Task_Creation_ShouldThrow_WhenNameIsNullOrWhitespace()
	{
		Assert.Throws<ArgumentNullException>(() => new WorkTask(null!, null));
		Assert.Throws<ArgumentNullException>(() => new WorkTask("", null));
		Assert.Throws<ArgumentNullException>(() => new WorkTask("   ", null));
	}

	[Fact]
	public void SetDeadline_ShouldThrow_WhenDeadlineIsInPast()
	{
		var task = new WorkTask("Task", null);
		var past = DateTime.UtcNow.AddSeconds(-1);
		Assert.Throws<ArgumentException>(() => task.SetDeadline(past));
	}
}