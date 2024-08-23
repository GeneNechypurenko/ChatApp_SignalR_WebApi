using BLL.Services.Interfaces;
using ChatApp_SignalR_WebApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp_SignalR_WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private static List<ChatModel> Users = new List<ChatModel>();
        private readonly IUserChatSessionService _userChatSessionService;
        private readonly IChatMessageService _chatMessageService;

        public ChatHub(IUserChatSessionService userChatSessionService, IChatMessageService chatMessageService)
        {
            _userChatSessionService = userChatSessionService;
            _chatMessageService = chatMessageService;
        }

        public async Task Connect(string username)
        {
            var connectId = Context.ConnectionId;

            if (!Users.Any(u => u.ConnectionId == connectId))
            {
                Users.Add(new ChatModel { ConnectionId = connectId, Username = username });

                await _userChatSessionService.StartNewSessionAsync(username);

                await Clients.Caller.SendAsync("Connected", connectId, username, Users);
                await Clients.AllExcept(connectId).SendAsync("NewUserConnected", connectId, username);
            }
        }

        public async Task Send(string username, string message)
        {
            await _chatMessageService.AddMessageAsync(username, message);
            await Clients.All.SendAsync("AddMessage", username, message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (connection != null)
            {
                Users.Remove(connection);
                var connectId = Context.ConnectionId;
                await _userChatSessionService.EndSessionAsync(connection.Username!);
                await Clients.All.SendAsync("UserDisconnected", connectId, connection.Username);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
