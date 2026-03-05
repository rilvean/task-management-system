using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

		optionsBuilder.UseSqlServer("Server=82EY;Database=TaskManagement;Trusted_Connection=True");

		return new AppDbContext(optionsBuilder.Options);
	}
}
