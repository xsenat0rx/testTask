using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace testTaskHub.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        private static readonly ConcurrentDictionary<int, HashSet<string>> ChatGroups =
            new ConcurrentDictionary<int, HashSet<string>>();

        public async Task JoinChat(int chatId)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                throw new HubException("Unauthorized");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat-{chatId}");
            ChatGroups.AddOrUpdate(chatId,
                new HashSet<string> { Context.ConnectionId },
                (key, existingSet) =>
                {
                    existingSet.Add(Context.ConnectionId);
                    return existingSet;
                });
            _logger.LogInformation($"User {Context.ConnectionId} has joined chat {chatId}");
            await Clients.Caller.SendAsync("UserJoinedChat", chatId); //client-side method
        }

        public async Task SendMessage(int chatId, string text)
        {
            if (!Context.User.Identity.IsAuthenticated)
            {
                throw new HubException("Unauthorized");
            }
            await Clients.Group($"chat-{chatId}")
                .SendAsync("ReceiveMessage", Context.ConnectionId, text);   //client-side method
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var (chatId, connections) in ChatGroups)
            {
                if (connections.Contains(Context.ConnectionId))
                {
                    connections.Remove(Context.ConnectionId);
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"chat-{chatId}");
                }
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}