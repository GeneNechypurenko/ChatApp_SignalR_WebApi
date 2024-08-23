using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp_SignalR_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatMessageService _chatMessageService;

        public ChatController(IChatMessageService chatMessageService)
        {
            _chatMessageService = chatMessageService;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetChatHistory()
        {
            var messages = await _chatMessageService.GetAllMessagesAsync();
            return Ok(messages);
        }
    }
}
