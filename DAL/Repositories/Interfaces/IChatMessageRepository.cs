using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IChatMessageRepository
    {
        Task CreateMessageAsync(ChatMessage message);
    }
}
