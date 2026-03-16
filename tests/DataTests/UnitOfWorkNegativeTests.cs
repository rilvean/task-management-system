using Data;

namespace DataTests;

public class UnitOfWorkNegativeTests
{
	[Fact]
	public async Task SaveChangesAsync_ShouldThrow_WhenDbContextDisposed()
	{
		var context = DbContextFactory.Create();
		var uow = new UnitOfWork(context);

		context.Dispose();

		await Assert.ThrowsAsync<ObjectDisposedException>(async () =>
		{
			await uow.SaveChangesAsync(TestContext.Current.CancellationToken);
		});
	}

	[Fact]
	public void Constructor_ShouldThrow_WhenDbContextIsNull()
	{
		Assert.Throws<ArgumentNullException>(() =>
		{
			var uow = new UnitOfWork(null!);
		});
	}
}