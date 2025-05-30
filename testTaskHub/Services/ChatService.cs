using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using testTaskHub.Hubs;
using testTaskHub.Interfaces;

namespace testTaskHub.Services
{
    public class ChatService : IChatService
    {
        private readonly TestTaskDbContext _context;
        private readonly ILogger<ChatService> _logger;

        public ChatService(TestTaskDbContext context, ILogger<ChatService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> CreateChatAsync(string chatName, int userId)
        {
            var chat = new Chat()
            {
                Name = chatName,
                IsGroup = true,
                CreatedAt = DateTime.UtcNow,
                Users = new List<ChatUser>
                {
                    new ChatUser { UserId = userId }
                },
                Messages = new List<Message>()
            };
            await _context.AddAsync(chat);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"User {userId} created chat {chatName}");
            return chat.Id;  //return chat id to the client
        }

        public async Task<List<Message>> GetChatMessagesAsync(int chatId)
        {
            _logger.LogInformation($"Requested messages history from chat {chatId}");
            return await _context.Messages
            .Where(m => m.ChatId == chatId)
            .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetChatMessagesAsync(int chatId, int pageNumber, int pageSize)
        {
            return await _context.Messages
                .Where(m => m.ChatId == chatId)
                .OrderByDescending(m => m.SentAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<int>> GetUserChatsAsync(int userId)
        {
            var userChats = await _context.Chats
                .Where(x => x.Users.Any(u => u.UserId == userId))
                .Select(x => x.Id)
                .ToListAsync();
            _logger.LogInformation($"Requested chats list for user {userId}");
            return userChats;   // return list of chat ids for the user
        }

        public async Task SaveMessageAsync(Message message)
        {
            if (message == null || message.ChatId <= 0)
            {
                throw new ArgumentException("Invalid message data.");
            }
            await _context.Messages.AddAsync(message);
            _logger.LogInformation($"Sent message {message}");
            await _context.SaveChangesAsync();
        }
        public async Task<List<Chat>> SearchChatsAsync(int userId, string query)
        {
            _logger.LogInformation($"Requested search for chat's names {query} from user {userId}");
            return await _context.Chats
                .Where(c => c.Users.Any(u => u.UserId == userId) && c.Name.Contains(query))
                .ToListAsync();
        }

        public async Task<List<Message>> SearchMessagesAsync(int chatId, string query)
        {
            _logger.LogInformation($"Requested search for messages in chat {chatId} with meaning {query}");
            return await _context.Messages
                .Where(m => m.ChatId == chatId && m.Text.ToLower().Contains(query.ToLower()))
                .ToListAsync();
        }
    }
}