using Domain.Models;

namespace DataTests;

public class AppDbContextTests
{
	[Fact]
	public async Task SaveChangesAsync_ShouldSetShadowProperties()
	{
		var context = DbContextFactory.Create();

		var task = new MyTask("task", null);

		context.Tasks.Add(task);

		await context.SaveChangesAsync();

		var entry = context.Entry(task);

		var created = entry.Property<DateTime>("CreatedAt").CurrentValue;
		var updated = entry.Property<DateTime>("UpdatedAt").CurrentValue;

		Assert.NotEqual(default, created);
		Assert.NotEqual(default, updated);
	}
}