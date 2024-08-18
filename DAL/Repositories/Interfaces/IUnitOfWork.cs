using DAL.Models;

namespace DAL.Repositories.Interfaces
{
	public interface IUnitOfWork
	{
		IRepository<User> UserRepository { get; }
		IRepository<ChatMessage> ChatMessageRepository { get; }
		IRepository<UserChatSession> UserChatSessionRepository { get; }
		Task SaveAsync();
	}
}
