using BLL.ModelsDTO;

namespace BLL.Services.Interfaces
{
	public interface IUserChatSessionService
	{
		Task StartNewSessionAsync(int userId);
		Task StartNewSessionAsync(string username);
		Task<UserChatSessionDTO?> GetCurrentSessionAsync(int userId);
		Task<UserChatSessionDTO?> GetCurrentSessionAsync(string username);
		Task EndSessionAsync(int userId);
		Task EndSessionAsync(string username);
	}
}
