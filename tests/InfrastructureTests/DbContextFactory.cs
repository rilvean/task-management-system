using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureTests;

public static class DbContextFactory
{
	public static AppDbContext Create()
	{
		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;

		return new AppDbContext(options);
	}
}