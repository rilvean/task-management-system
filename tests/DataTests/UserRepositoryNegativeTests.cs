using Data.Repositories;
using Domain.ValueObjects;

namespace DataTests;

public class UserRepositoryNegativeTests
{
	[Fact]
	public async Task GetByEmailAsync_ShouldReturnNull_WhenUserNotExists()
	{
		var context = DbContextFactory.Create();
		var repo = new UserRepository(context);

		var result = await repo.GetByEmailAsync(Email.From("unknown@mail.com"));

		Assert.Null(result);
	}

	[Fact]
	public async Task GetByIdAsync_ShouldReturnNull_WhenUserNotExists()
	{
		var context = DbContextFactory.Create();
		var repo = new UserRepository(context);

		var result = await repo.GetByIdAsync(Guid.NewGuid());

		Assert.Null(result);
	}
}