using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class UserChatSessionRepository : IRepository<UserChatSession>
	{
		private readonly ApplicationDbContext _context;
		public UserChatSessionRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task CreateAsync(UserChatSession entity)
		{
			await _context.AddAsync(entity);
		}

		public async Task DeleteAsync(int id)
		{
			var userChatSession = await _context.FindAsync<UserChatSession>(id);
			if (userChatSession != null) _context.Update(userChatSession);
		}

		public async Task<IEnumerable<UserChatSession>> GetAllAsync()
		{
			return await _context.UsersChatSessions.ToListAsync();
		}

		public async Task<UserChatSession> GetAsync(int id)
		{
			return await _context.UsersChatSessions.FindAsync(id) ?? throw new ArgumentNullException($"Session with {id} not found");
		}

		public void Update(UserChatSession entity)
		{
			_context.UsersChatSessions.Update(entity);
		}
	}
}
