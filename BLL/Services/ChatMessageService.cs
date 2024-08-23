using BLL.ModelsDTO;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
    public class ChatMessageService : IChatMessageService
    {
        public readonly IUnitOfWork _unitOfWork;

        public ChatMessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetAllMessagesAsync()
        {
            var messages = await _unitOfWork.ChatMessageRepository.GetAllMessagesAsync();

            return messages.Select(m => new ChatMessageDTO
            {
                Id = m.Id,
                Message = m.Message,
                Timestamp = m.Timestamp,
                UserId = m.UserId,
                UserName = m.User?.UserName
            });
        }

        public async Task CreateMessageAsync(ChatMessageDTO chatMessageDto)
        {
            var chatMessage = new ChatMessage
            {
                Message = chatMessageDto.Message,
                Timestamp = chatMessageDto.Timestamp,
                UserId = chatMessageDto.UserId
            };

            await _unitOfWork.ChatMessageRepository.CreateMessageAsync(chatMessage);
            await _unitOfWork.SaveAsync();
        }
    }
}
