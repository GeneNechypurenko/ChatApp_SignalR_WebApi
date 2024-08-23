using BLL.ModelsDTO;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
	public class UserChatSessionService : IUserChatSessionService
	{
		public IUnitOfWork UnitOfWork { get; set; }
		public UserChatSessionService(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;

		public async Task StartNewSessionAsync(int userId)
		{
			var session = new UserChatSession
			{
				UserId = userId,
				LoginTime = DateTime.Now.ToString("o")
			};

			await UnitOfWork.UserChatSessionRepository.CreateNewSessionAsync(session);
			await UnitOfWork.SaveAsync();
		}

        public async Task StartNewSessionAsync(string username)
        {
			var user = await UnitOfWork.UserRepository.GetUserAsync(username);

			var session = new UserChatSession
			{
				UserId = user.Id,
				LoginTime = DateTime.Now.ToString("o")
			};

			await UnitOfWork.UserChatSessionRepository.CreateNewSessionAsync(session);
			await UnitOfWork.SaveAsync();
        }

        public async Task<UserChatSessionDTO?> GetCurrentSessionAsync(int userId)
		{
			var session = await UnitOfWork.UserChatSessionRepository.GetCurrentSessionAsync(userId);
			if (session != null)
			{
				var currentSessionDTO = new UserChatSessionDTO
				{
					UserId = session.UserId,
					LoginTime = session.LoginTime,
					LogoutTime = session.LogoutTime
				};
				return currentSessionDTO;
			}
			return null;
		}

        public async Task<UserChatSessionDTO?> GetCurrentSessionAsync(string username)
        {
			var user = await UnitOfWork.UserRepository.GetUserAsync(username);
            var session = await UnitOfWork.UserChatSessionRepository.GetCurrentSessionAsync(user.Id);
            if (session != null)
            {
                var currentSessionDTO = new UserChatSessionDTO
                {
                    UserId = session.UserId,
                    LoginTime = session.LoginTime,
                    LogoutTime = session.LogoutTime
                };
                return currentSessionDTO;
            }
            return null;
        }

        public async Task EndSessionAsync(int userId)
		{
			var session = await UnitOfWork.UserChatSessionRepository.GetCurrentSessionAsync(userId);
			if (session != null)
			{
				session.LogoutTime = DateTime.Now.ToString("o");
				UnitOfWork.UserChatSessionRepository.UpdateCurrentSession(session);
				await UnitOfWork.SaveAsync();
			}
		}

        public async Task EndSessionAsync(string username)
        {
            var user = await UnitOfWork.UserRepository.GetUserAsync(username);
			var session = await UnitOfWork.UserChatSessionRepository.GetCurrentSessionAsync(user.Id);
            if (session != null)
            {
                session.LogoutTime = DateTime.Now.ToString("o");
                UnitOfWork.UserChatSessionRepository.UpdateCurrentSession(session);
                await UnitOfWork.SaveAsync();
            }
        }
    }
}
