using Data.Configurations;
using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Submodels;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
	: DbContext(options)
{
	public DbSet<WorkTask> Tasks => Set<WorkTask>();
	public DbSet<User> Users => Set<User>();
	public DbSet<Assignment> Assignments => Set<Assignment>();

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
		modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
	}

	private void UpdateShadowProperties()
	{
		var entries = ChangeTracker.Entries<IAuditable>()
			.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

		foreach (var entry in entries)
		{
			var now = DateTimeOffset.UtcNow;

			if (entry.State == EntityState.Added)
				entry.Property("CreatedAt").CurrentValue = now;

			entry.Property("UpdatedAt").CurrentValue = now;
		}
	}
}