using Data.Repositories;

namespace DataTests;

public class TaskRepositoryNegativeTests
{
	[Fact]
	public async Task GetByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
	{
		var context = DbContextFactory.Create();
		var repo = new TaskRepository(context);

		var result = await repo.GetByIdAsync(Guid.NewGuid());

		Assert.Null(result);
	}

	[Fact]
	public async Task GetByNameAsync_ShouldReturnNull_WhenTaskDoesNotExist()
	{
		var context = DbContextFactory.Create();
		var repo = new TaskRepository(context);

		var result = await repo.GetByNameAsync("unknown");

		Assert.Null(result);
	}

	[Fact]
	public async Task GetByPriorityAsync_ShouldReturnEmpty_WhenNoTasks()
	{
		var context = DbContextFactory.Create();
		var repo = new TaskRepository(context);

		var result = await repo.GetByPriorityAsync(Domain.Enums.MyTaskPriority.High);

		Assert.Empty(result);
	}
}