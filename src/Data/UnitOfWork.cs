using Data.Repositories;
using App.Interfaces;

namespace Data;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
	private readonly AppDbContext _dbContext = dbContext
			?? throw new ArgumentNullException(nameof(dbContext));

	private WorkTaskRepository? _taskRepository;
	private UserRepository? _userRepository;

	public IWorkTaskRepository WorkTaskRepository
		=> _taskRepository ??= new WorkTaskRepository(_dbContext);
	public IUserRepository UserRepository
		=> _userRepository ??= new UserRepository(_dbContext);

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		=> await _dbContext.SaveChangesAsync(cancellationToken);
}