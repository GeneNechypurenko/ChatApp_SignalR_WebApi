using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class UserRepository : IRepository<User>
	{
		private readonly ApplicationDbContext _context;

		public UserRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<User>> GetAllAsync()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<User> GetAsync(int id)
		{
			return await _context.Users.FindAsync(id) ?? throw new ArgumentNullException($"UserRepository with {id} not found");
		}

		public async Task<User> GetAsync(string name)
		{
			return await _context.Users.Where(u => u.UserName == name).FirstOrDefaultAsync()
				?? throw new ArgumentNullException($"UserRepository with {name} not found");
		}

		public async Task CreateAsync(User entity)
		{
			await _context.Users.AddAsync(entity);
		}

		public void Update(User entity)
		{
			_context.Users.Update(entity);
		}

		public async Task DeleteAsync(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user != null) _context.Users.Remove(user);
		}
	}
}
