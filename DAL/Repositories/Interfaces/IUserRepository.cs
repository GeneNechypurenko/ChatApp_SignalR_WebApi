using DAL.Models;

namespace DAL.Repositories.Interfaces
{
	public interface IUserRepository
	{
		Task<User> GetUserAsync(int id);
		Task<User> GetUserAsync(string username);
		Task CreateUserAsync(User user);
	}
}
