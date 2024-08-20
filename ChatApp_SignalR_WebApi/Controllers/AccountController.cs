using BLL.ModelsDTO;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp_SignalR_WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IUserService _userService;
		public AccountController(IUserService userService) => _userService = userService;

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			await _userService.RegisterUserAsync(userDTO);
			return Ok(new { Message = "Registration successful" });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] UserDTO loginDTO)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var user = await _userService.LoginUserAsync(loginDTO.UserName, loginDTO.Password);
				return Ok(user);
			}
			catch
			{
				return Unauthorized(new { Message = "Invalid username or password" });
			}

			// -- to do create new user session 
			//await _userService.UpdateUserSessionAsync(user.Id, true);
		}

		// -- to do
		//[HttpGet] 
		//public async Task<IActionResult> GetConnectedUsers()
		//{
		//	var users = await _userService.GetConnectedUsersAsync();
		//	return Ok(users);
		//}
	}
}
