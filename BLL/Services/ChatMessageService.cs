using AutoMapper;
using BLL.ModelsDTO;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
	public class ChatMessageService : IChatService<ChatMessageDTO>
	{
		public IUnitOfWork UnitOfWork { get; set; }
		public ChatMessageService(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;

		public async Task<IEnumerable<ChatMessageDTO>> GetAllAsync()
		{
			var messagesDTOList = new Mapper(new MapperConfiguration(config => config.CreateMap<ChatMessage, ChatMessageDTO>()))
				.Map<IEnumerable<ChatMessage>, IEnumerable<ChatMessageDTO>>(await UnitOfWork.ChatMessageRepository.GetAllAsync());

			return messagesDTOList;
		}

		public async Task<ChatMessageDTO> GetAsync(int id)
		{
			var chatMessage = await UnitOfWork.ChatMessageRepository.GetAsync(id);

			var chatMessageDTO = new ChatMessageDTO
			{
				Id = chatMessage.Id,
				Message = chatMessage.Message,
				Timestamp = chatMessage.Timestamp,
				UserId = chatMessage.UserId,
			};

			return chatMessageDTO;
		}

		public async Task CreateAsync(ChatMessageDTO modelDTO)
		{
			var chatMessage = new ChatMessage
			{
				Id = modelDTO.Id,
				Message = modelDTO.Message,
				Timestamp = modelDTO.Timestamp,
				UserId = modelDTO.UserId,
			};

			await UnitOfWork.ChatMessageRepository.CreateAsync(chatMessage);
			await UnitOfWork.SaveAsync();
		}

		public async Task UpdateAsync(ChatMessageDTO modelDTO)
		{
			var chatMessage = await UnitOfWork.ChatMessageRepository.GetAsync(modelDTO.Id);

			if (chatMessage != null)
			{
				chatMessage.Message = modelDTO.Message;

				UnitOfWork.ChatMessageRepository.Update(chatMessage);
				await UnitOfWork.SaveAsync();
			}
		}

		public async Task DeleteAsync(int id)
		{
			await UnitOfWork.ChatMessageRepository.DeleteAsync(id);
			await UnitOfWork.SaveAsync();
		}
	}
}
