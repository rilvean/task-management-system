using Data.Repositories;
using Domain.Interfaces;

namespace Data;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
	private readonly AppDbContext _dbContext = dbContext
			?? throw new ArgumentNullException(nameof(dbContext));

	private WorkTaskRepository? _taskRepository = new WorkTaskRepository(dbContext);
	private UserRepository? _userRepository = new UserRepository(dbContext);

	public IWorkTaskRepository TaskRepository
		=> _taskRepository ??= new WorkTaskRepository(_dbContext);
	public IUserRepository UserRepository
		=> _userRepository ??= new UserRepository(_dbContext);

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		=> await _dbContext.SaveChangesAsync(cancellationToken);
}