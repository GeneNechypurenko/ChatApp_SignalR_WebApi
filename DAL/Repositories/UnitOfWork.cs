using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private UserRepository _userRepository;
		private ChatMessageRepository _chatMessageRepository;
		private UserChatSessionRepository _userChatSessionRepository;
		public UnitOfWork(ApplicationDbContext context) => _context = context;
		public IRepository<User> UserRepository => _userRepository ??= new UserRepository(_context);
		public IRepository<ChatMessage> ChatMessageRepository => _chatMessageRepository ??= new ChatMessageRepository(_context);
		public IRepository<UserChatSession> UserChatSessionRepository => _userChatSessionRepository ??= new UserChatSessionRepository(_context);
		public async Task SaveAsync() => await _context.SaveChangesAsync();
	}
}
