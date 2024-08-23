using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ChatApp_SignalR_WebApi.Hubs;
using BLL.ModelsDTO;

namespace ChatApp_SignalR_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHubContext<ChatHub> _chatHubContext;

        public AccountController(IUserService userService, IHubContext<ChatHub> chatHubContext)
        {
            _userService = userService;
            _chatHubContext = chatHubContext;
        }

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
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] UserDTO logoutDTO)
        {
            try
            {
                await _chatHubContext.Clients.User(logoutDTO.UserName).SendAsync("Disconnect");

                return Ok(new { Message = "Logout successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
