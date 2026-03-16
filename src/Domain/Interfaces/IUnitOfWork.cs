namespace Domain.Interfaces;

public interface IUnitOfWork
{
	IWorkTaskRepository TaskRepository { get; }
	IUserRepository UserRepository { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}