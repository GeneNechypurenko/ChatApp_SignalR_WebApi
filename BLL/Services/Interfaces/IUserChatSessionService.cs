using BLL.ModelsDTO;

namespace BLL.Services.Interfaces
{
	public interface IUserChatSessionService
	{
		Task StartNewSessionAsync(int userId);
		Task<UserChatSessionDTO?> GetCurrentSessionAsync(int userId);
		Task EndSessionAsync(int userId);
	}
}
