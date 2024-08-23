using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ApplicationDbContext _context;
        public ChatMessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateMessageAsync(ChatMessage message)
        {
            await _context.AddAsync(message);
        }

        public async Task<IEnumerable<ChatMessage>> GetAllMessagesAsync()
        {
            return await _context.ChatMessages.Include(m => m.User).ToListAsync();
        }
    }
}
