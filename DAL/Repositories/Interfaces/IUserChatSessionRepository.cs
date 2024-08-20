using DAL.Models;

namespace DAL.Repositories.Interfaces
{
	public interface IUserChatSessionRepository
	{
		Task CreateNewSessionAsync(UserChatSession session);
		Task<UserChatSession> GetCurrentSessionAsync(int userId);
		void UpdateCurrentSession(UserChatSession session);
	}
}
