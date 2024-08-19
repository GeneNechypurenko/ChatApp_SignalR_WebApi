using BLL.ModelsDTO;

namespace BLL.Services.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<UserDTO>> GetConnectedUsersAsync();
		Task<UserDTO?> LoginUserAsync(string username, string password);
		Task RegisterUserAsync(UserDTO userDTO);
	}
}
