using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class UserChatSessionRepository : IUserChatSessionRepository
	{
		private readonly ApplicationDbContext _context;
		public UserChatSessionRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task CreateNewSessionAsync(UserChatSession session)
		{
			await _context.UsersChatSessions.AddAsync(session);
		}

		public async Task<UserChatSession> GetCurrentSessionAsync(int userId)
		{
			return await _context.UsersChatSessions.Where(x => x.UserId == userId && x.LogoutTime == null).FirstOrDefaultAsync();
		}

		public void UpdateCurrentSession(UserChatSession session)
		{
			_context.UsersChatSessions.Update(session);
		}
	}
}
