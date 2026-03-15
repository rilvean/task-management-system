using Data.Repositories;
using Domain.Enums;
using Domain.Models;

namespace DataTests;

public class TaskRepositoryTests
{
	[Fact]
	public async Task AddAsync_ShouldAddTask()
	{
		var context = DbContextFactory.Create();
		var repo = new TaskRepository(context);

		var task = new WorkTask("task1", null);

		await repo.AddAsync(task);
		await context.SaveChangesAsync(TestContext.Current.CancellationToken);

		Assert.Single(context.Tasks);
	}

	[Fact]
	public async Task GetByIdAsync_ShouldReturnTask()
	{
		var context = DbContextFactory.Create();
		var repo = new TaskRepository(context);

		var task = new WorkTask("task1", null);

		context.Tasks.Add(task);
		await context.SaveChangesAsync(TestContext.Current.CancellationToken);

		var result = await repo.GetByIdAsync(task.Id);

		Assert.NotNull(result);
		Assert.Equal(task.Id, result!.Id);
	}

	[Fact]
	public async Task GetByNameAsync_ShouldReturnTask()
	{
		var context = DbContextFactory.Create();
		var repo = new TaskRepository(context);

		var task = new WorkTask("task1", null);

		context.Tasks.Add(task);
		await context.SaveChangesAsync(TestContext.Current.CancellationToken);

		var result = await repo.GetByNameAsync("task1");

		Assert.NotNull(result);
	}

	[Fact]
	public async Task GetByPriorityAsync_ShouldReturnFilteredTasks()
	{
		var context = DbContextFactory.Create();
		var repo = new TaskRepository(context);

		var task1 = new WorkTask("task1", null);
		var task2 = new WorkTask("task2", null);
		task2.ChangePriority(WorkTaskPriority.Low);

		context.Tasks.AddRange(task1, task2);
		await context.SaveChangesAsync(TestContext.Current.CancellationToken);

		var result = await repo.GetByPriorityAsync(WorkTaskPriority.Medium);

		Assert.Single(result);
	}

	[Fact]
	public async Task GetByStatusAsync_ShouldReturnFilteredTasks()
	{
		var context = DbContextFactory.Create();
		var repo = new TaskRepository(context);

		var task1 = new WorkTask("task1", null);
		var task2 = new WorkTask("task2", null);
		task2.ChangeStatus(WorkTaskStatus.Completed);

		context.Tasks.AddRange(task1, task2);
		await context.SaveChangesAsync(TestContext.Current.CancellationToken);

		var result = await repo.GetByStatusAsync(WorkTaskStatus.Completed);

		Assert.Single(result);
	}

	[Fact]
	public async Task GetAllAsync_ShouldSortByName()
	{
		var context = DbContextFactory.Create();
		var repo = new TaskRepository(context);

		var t1 = new WorkTask("a", null);
		var t2 = new WorkTask("b", null);

		context.Tasks.AddRange(t1, t2);
		await context.SaveChangesAsync(TestContext.Current.CancellationToken);

		var result = await repo.GetAllSortedAsync(WorkTaskSortBy.Name);

		Assert.Equal("a", result.First().Name);
	}

	[Fact]
	public async Task Remove_ShouldDeleteTask()
	{
		var context = DbContextFactory.Create();
		var repo = new TaskRepository(context);

		var task = new WorkTask("task", null);

		context.Tasks.Add(task);
		await context.SaveChangesAsync(TestContext.Current.CancellationToken);

		repo.Delete(task);
		await context.SaveChangesAsync(TestContext.Current.CancellationToken);

		Assert.Empty(context.Tasks);
	}
}