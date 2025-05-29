using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace testTaskHub.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly TestTaskDbContext _context;
        public ChatHub(ILogger<ChatHub> logger, TestTaskDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        private static readonly ConcurrentDictionary<int, HashSet<string>> ChatGroups =
            new ConcurrentDictionary<int, HashSet<string>>();

        public async Task JoinChat(int chatId)
        {
            IsUserAuthenticated();
            await Groups.AddToGroupAsync(Context.ConnectionId, $"chat-{chatId}");
            ChatGroups.AddOrUpdate(chatId,
                new HashSet<string> { Context.ConnectionId },
                (key, existingSet) =>
                {
                    existingSet.Add(Context.ConnectionId);
                    return existingSet;
                });
            _logger.LogInformation($"User {Context.User.Identity.Name} has joined chat {chatId}");   
        }

        public async Task SendMessage(int chatId, string text)
        {
            IsUserAuthenticated();
            await Clients.Group($"chat-{chatId}")
                .SendAsync("ReceiveMessage", 
                    Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown", 
                    text);  
            var message = new Message()
            {
                Text = text,
                SentAt = DateTime.UtcNow,
                ChatId = chatId,
                SenderId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0"),
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
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

        private bool IsUserAuthenticated()
        {
            if (Context.User?.Identity == null || !Context.User.Identity.IsAuthenticated)
            {
                throw new HubException("Unauthorized");
            }
            return true;
        }
    }
}