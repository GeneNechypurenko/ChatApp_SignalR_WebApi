using BLL.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp_SignalR_WebApi.Hubs
{
    public class ChatHub: Hub
    {
        private readonly IUserChatSessionService _userChatSessionService;
        private readonly IChatMessageService _chatMessageService;
        private readonly Dictionary<string, string> _connections = new Dictionary<string, string>();
        public ChatHub(IUserChatSessionService userChatSessionService, IChatMessageService chatMessageService)
        {
            _userChatSessionService = userChatSessionService;
            _chatMessageService = chatMessageService;
        }

        public async Task Connect(string username)
        {
            var connectionId = Context.ConnectionId;
            _connections[connectionId] = username;
            await Clients.Caller.SendAsync("Connected", connectionId, username);
            await Clients.AllExcept(connectionId).SendAsync("NewUserConnected", connectionId, username);
            await _userChatSessionService.StartNewSessionAsync(username);
        }

        public async Task Send(string username, string message)
        {
            await _chatMessageService.AddMessageAsync(username, message);
            await Clients.All.SendAsync("AddMessage", username, message);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            if (_connections.TryGetValue(connectionId, out var username))
            {
                _connections.Remove(connectionId);
                //await _userChatSessionService.EndSessionAsync(username);
                await Clients.All.SendAsync("UserDisconnected", connectionId, username);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task Disconnect(string username)
        {
            var connectionId = Context.ConnectionId;

            if (_connections.TryGetValue(connectionId, out var _))
            {
                _connections.Remove(connectionId);
                await _userChatSessionService.EndSessionAsync(username);
                await Clients.All.SendAsync("UserDisconnected", connectionId, username);
            }
        }
    }
}
