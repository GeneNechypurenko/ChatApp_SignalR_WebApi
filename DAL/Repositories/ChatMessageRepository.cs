using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
	public class ChatMessageRepository
	{
		private readonly ApplicationDbContext _context;
		public ChatMessageRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<ChatMessage>> GetAllAsync()
		{
			return await _context.ChatMessages.ToListAsync();
		}

		public async Task<ChatMessage> GetAsync(int id)
		{
			return await _context.ChatMessages.FindAsync(id) ?? throw new ArgumentNullException($"Message with {id} not found");
		}

		public async Task CreateAsync(ChatMessage entity)
		{
			await _context.ChatMessages.AddAsync(entity);
		}

		public void Update(ChatMessage entity)
		{
			_context.ChatMessages.Update(entity);
		}

		public async Task DeleteAsync(int id)
		{
			var message = await _context.ChatMessages.FindAsync(id);
			if (message != null) _context.Remove(message);
		}
	}
}
