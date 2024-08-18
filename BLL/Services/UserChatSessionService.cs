using AutoMapper;
using BLL.ModelsDTO;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
	public class UserChatSessionService: IChatService<UserChatSessionDTO>
	{
		public IUnitOfWork UnitOfWork { get; set; }

		public UserChatSessionService(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;

		public async Task<IEnumerable<UserChatSessionDTO>> GetAllAsync()
		{
			var sessionsDTOList = new Mapper(new MapperConfiguration(config => config.CreateMap<UserChatSession, UserChatSessionDTO>()))
				.Map<IEnumerable<UserChatSession>, IEnumerable<UserChatSessionDTO>>(await UnitOfWork.UserChatSessionRepository.GetAllAsync());

			return sessionsDTOList;
		}

		public async Task<UserChatSessionDTO> GetAsync(int id)
		{
			var userChatSession = await UnitOfWork.UserChatSessionRepository.GetAsync(id);

			var userChatSessionDTO = new UserChatSessionDTO
			{
				Id = id,
				LoginTime = userChatSession.LoginTime,
				LogoutTime = userChatSession.LogoutTime,
				UserId = userChatSession.UserId,
			};

			return userChatSessionDTO;
		}

		public async Task CreateAsync(UserChatSessionDTO modelDTO)
		{
			var userChatSession = new UserChatSession
			{
				LoginTime = modelDTO.LoginTime,
				LogoutTime = modelDTO.LogoutTime,
				UserId = modelDTO.UserId,
			};

			await UnitOfWork.UserChatSessionRepository.CreateAsync(userChatSession);
			await UnitOfWork.SaveAsync();
		}

		public async Task UpdateAsync(UserChatSessionDTO modelDTO)
		{
			var userChatSession = await UnitOfWork.UserChatSessionRepository.GetAsync(modelDTO.Id);

			if(userChatSession != null)
			{
				userChatSession.LoginTime = modelDTO.LoginTime;
				userChatSession.LogoutTime = modelDTO.LogoutTime;

				UnitOfWork.UserChatSessionRepository.Update(userChatSession);
				await UnitOfWork.SaveAsync();
			}
		}

		public async Task DeleteAsync(int id)
		{
			await UnitOfWork.UserChatSessionRepository.DeleteAsync(id);
			await UnitOfWork.SaveAsync();
		}
	}
}
