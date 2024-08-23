using BLL.Services.Interfaces;
using BLL.ModelsDTO;
using Microsoft.AspNetCore.SignalR;
using ChatApp_SignalR_WebApi.Models;

namespace ChatApp_SignalR_WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private static List<ChatModel> Users = new List<ChatModel>();
        private readonly IUserService _userService;
        private readonly IChatMessageService _chatMessageService;

        public ChatHub(IUserService userService, IChatMessageService chatMessageService)
        {
            _userService = userService;
            _chatMessageService = chatMessageService;
        }

        public async Task Connect(string username)
        {
            var connectionId = Context.ConnectionId;

            if (!Users.Any(u => u.ConnectionId == connectionId))
            {
                var user = await _userService.GetUserByUsernameAsync(username);
                if (user != null)
                {
                    Users.Add(new ChatModel { ConnectionId = connectionId, Username = username });
                    await Clients.Caller.SendAsync("Connected", connectionId, username, Users);
                    await Clients.AllExcept(connectionId).SendAsync("NewUserConnected", connectionId, username);
                }
            }
        }

        public async Task Send(string username, string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var user = await _userService.GetUserByUsernameAsync(username);
            if (user != null)
            {
                await _chatMessageService.CreateMessageAsync(new ChatMessageDTO
                {
                    Message = message,
                    Timestamp = timestamp,
                    UserId = user.Id,
                    UserName = username
                });

                await Clients.All.SendAsync("AddMessage", username, message, timestamp);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (connection != null)
            {
                Users.Remove(connection);
                await Clients.All.SendAsync("UserDisconnected", connection.ConnectionId, connection.Username);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Disconnect()
        {
            var connection = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (connection != null)
            {
                Users.Remove(connection);
                await Clients.All.SendAsync("UserDisconnected", connection.ConnectionId, connection.Username);
            }
        }
    }
}
