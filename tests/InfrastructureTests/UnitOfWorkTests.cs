using Infrastructure;
using Domain.Models;

namespace InfrastructureTests;

public class UnitOfWorkTests
{
	[Fact]
	public async Task TaskRepository_ShouldBeCreated()
	{
		var context = DbContextFactory.Create();
		var uow = new UnitOfWork(context);

		var repo = uow.WorkTaskRepository;

		Assert.NotNull(repo);
	}

	[Fact]
	public void TaskRepository_ShouldReturnSameInstance()
	{
		var context = DbContextFactory.Create();
		var uow = new UnitOfWork(context);

		var repo1 = uow.WorkTaskRepository;
		var repo2 = uow.WorkTaskRepository;

		Assert.Same(repo1, repo2);
	}

	[Fact]
	public void UserRepository_ShouldReturnSameInstance()
	{
		var context = DbContextFactory.Create();
		var uow = new UnitOfWork(context);

		var repo1 = uow.UserRepository;
		var repo2 = uow.UserRepository;

		Assert.Same(repo1, repo2);
	}

	[Fact]
	public async Task SaveChangesAsync_ShouldPersistTask()
	{
		var context = DbContextFactory.Create();
		var uow = new UnitOfWork(context);

		var task = new WorkTask("task", null);

		await uow.WorkTaskRepository.AddAsync(task);
		await uow.SaveChangesAsync(TestContext.Current.CancellationToken);

		var result = await context.Tasks.FindAsync(new object[] { task.Id }, cancellationToken: TestContext.Current.CancellationToken);

		Assert.NotNull(result);
	}
}