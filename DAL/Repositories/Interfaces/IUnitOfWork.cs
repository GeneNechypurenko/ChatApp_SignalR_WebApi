using DAL.Models;

namespace DAL.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		IUserRepository UserRepository { get; }
		Task SaveAsync();
	}
}
