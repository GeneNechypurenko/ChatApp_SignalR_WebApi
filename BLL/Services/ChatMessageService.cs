using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
    public class ChatMessageService : IChatMessageService
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public ChatMessageService(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;
        public async Task AddMessageAsync(string username, string message)
        {
            var user = await UnitOfWork.UserRepository.GetUserAsync(username);
            var session = await UnitOfWork.UserChatSessionRepository.GetCurrentSessionAsync(user.Id);

            var chatMessage = new ChatMessage
            {
                Message = message,
                Timestamp = DateTime.UtcNow.ToString("o"),
                Session = session,
                SessionId = session.Id
            };

            await UnitOfWork.ChatMessageRepository.CreateMessageAsync(chatMessage);
            await UnitOfWork.SaveAsync();
        }
    }
}
