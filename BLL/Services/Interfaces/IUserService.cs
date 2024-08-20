using BLL.ModelsDTO;

namespace BLL.Services.Interfaces
{
	public interface IUserService
	{
		Task<UserDTO?> LoginUserAsync(string username, string password);
		Task RegisterUserAsync(UserDTO userDTO);
	}
}
