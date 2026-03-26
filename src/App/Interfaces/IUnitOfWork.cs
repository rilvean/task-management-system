namespace App.Interfaces;

public interface IUnitOfWork
{
	IWorkTaskRepository WorkTaskRepository { get; }
	IUserRepository UserRepository { get; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}