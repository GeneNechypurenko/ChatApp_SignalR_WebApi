using BLL.ModelsDTO;

namespace BLL.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task<IEnumerable<ChatMessageDTO>> GetAllMessagesAsync();
        Task CreateMessageAsync(ChatMessageDTO chatMessageDto);
    }
}
