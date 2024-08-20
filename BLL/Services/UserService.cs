using AutoMapper;
using BLL.ModelsDTO;
using BLL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
	public class UserService : IUserService
	{
		public IUnitOfWork UnitOfWork { get; set; }

		public UserService(IUnitOfWork unitOfWork) => UnitOfWork = unitOfWork;

		public async Task<UserDTO?> LoginUserAsync(string username, string password)
		{
			var user = await UnitOfWork.UserRepository.GetUserAsync(username);

			if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
			{
				return new UserDTO
				{
					Id = user.Id,
					UserName = user.UserName,
				};
			}
			return null;
		}

		public async Task RegisterUserAsync(UserDTO userDTO)
		{
			var user = new User
			{
				UserName = userDTO.UserName,
				Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),
			};

			await UnitOfWork.UserRepository.CreateUserAsync(user);
			await UnitOfWork.SaveAsync();
		}
	}
}
