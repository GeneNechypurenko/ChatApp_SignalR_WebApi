using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task CreateUserAsync(User user)
		{
			await _context.Users.AddAsync(user);
		}

		public async Task<IEnumerable<User>> GetConectedUsersAsync()
		{
			var connectedUserIds = await _context.UsersChatSessions.Where(ucs => ucs.IsConnected).Select(ucs => ucs.UserId)
				.Distinct().ToListAsync();
			return await _context.Users.Where(u => connectedUserIds.Contains(u.Id)).ToListAsync();
		}

		public async Task<User> GetUserAsync(int id)
		{
			return await _context.Users.FindAsync(id) ?? throw new ArgumentException($"User with Id {id} not found", nameof(id));
		}

		public async Task<User> GetUserAsync(string username)
		{
			return await _context.Users.Where(u => u.UserName == username).FirstOrDefaultAsync() 
				?? throw new ArgumentException($"User with {username} not found", nameof(username));
		}
	}
}
