using AutoMapper;
using BLL.ModelsDTO;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
	public class UserService : IChatService<UserDTO>
	{
		public IUnitOfWork UnitOfWork { get; set; }

		public UserService(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;

		public async Task<IEnumerable<UserDTO>> GetAllAsync()
		{
			var usersDTOList = new Mapper(new MapperConfiguration(config => config.CreateMap<User, UserDTO>()))
			.Map<IEnumerable<User>, IEnumerable<UserDTO>>(await UnitOfWork.UserRepository.GetAllAsync());

			return usersDTOList;
		}

		public async Task<UserDTO> GetAsync(int id)
		{
			var user = await UnitOfWork.UserRepository.GetAsync(id);

			var userDTO = new UserDTO
			{
				UserName = user.UserName,
				Password = user.Password,
				PasswordHash = user.PasswordHash,
				Messages = user.Messages,
			};

			return userDTO;
		}

		public async Task CreateAsync(UserDTO modelDTO)
		{
			var user = new User
			{
				UserName = modelDTO.UserName,
				Password = modelDTO.Password,
				PasswordHash = modelDTO.PasswordHash,
				Messages = new List<ChatMessage>(),
			};

			await UnitOfWork.UserRepository.CreateAsync(user);
			await UnitOfWork.SaveAsync();
		}

		public async Task UpdateAsync(UserDTO modelDTO)
		{
			var user = await UnitOfWork.UserRepository.GetAsync(modelDTO.Id);

			if (user != null)
			{
				user.UserName = modelDTO.UserName;
				user.Password = modelDTO.Password;
				user.PasswordHash = modelDTO.PasswordHash;

				UnitOfWork.UserRepository.Update(user);
				await UnitOfWork.SaveAsync();
			}
		}

		public async Task DeleteAsync(int id)
		{
			await UnitOfWork.UserRepository.DeleteAsync(id);
			await UnitOfWork.SaveAsync();
		}
	}
}
