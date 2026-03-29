using Domain.Models;

namespace InfrastructureTests;

public class AppDbContextTests
{
	[Fact]
	public async Task SaveChangesAsync_ShouldSetShadowProperties()
	{
		var context = DbContextFactory.Create();

		var task = new WorkTask("task", null);

		context.Tasks.Add(task);

		await context.SaveChangesAsync(TestContext.Current.CancellationToken);

		var entry = context.Entry(task);

		var created = entry.Property<DateTimeOffset>("CreatedAt").CurrentValue;
		var updated = entry.Property<DateTimeOffset>("UpdatedAt").CurrentValue;

		Assert.NotEqual(default, created);
		Assert.NotEqual(default, updated);
	}
}