using Data.Configurations;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
	: DbContext(options), IUnitOfWork
{
	public DbSet<MyTask> Tasks => Set<MyTask>();
	public DbSet<User> Users => Set<User>();

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		UpdateShadowProperties();
		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new UserConfiguration());
		modelBuilder.ApplyConfiguration(new TaskConfiguration());
	}

	private void UpdateShadowProperties()
	{
		var entries = ChangeTracker.Entries()
			.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

		foreach (var entry in entries)
		{
			var now = DateTime.UtcNow;

			if (entry.State == EntityState.Added)
				entry.Property("CreatedAt").CurrentValue = now;

			entry.Property("UpdatedAt").CurrentValue = now;
		}
	}
}