using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		IConfiguration configuration = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.Development.json")
			.Build();

		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

		optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));

		return new AppDbContext(optionsBuilder.Options);
	}
}
