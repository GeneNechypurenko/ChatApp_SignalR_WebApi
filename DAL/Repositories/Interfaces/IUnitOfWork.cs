using DAL.Models;

namespace DAL.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		IUserRepository UserRepository { get; }
		IChatMessageRepository ChatMessageRepository { get; }
		Task SaveAsync();
	}
}
