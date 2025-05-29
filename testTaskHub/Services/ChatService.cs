using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using testTaskHub.Interfaces;

namespace testTaskHub.Services
{
    public class ChatService : IChatService
    {
        private readonly TestTaskDbContext _context;

        public ChatService(TestTaskDbContext context)
        {
            _context = context;
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
            return chat.Id;  //return chat id to the client
        }

        public async Task<List<Message>> GetChatMessagesAsync(int chatId)
        {
            return await _context.Messages
            .Where(m => m.ChatId == chatId)
            .ToListAsync();
        }

        public async Task<List<int>> GetUserChatsAsync(int userId)
        {
            var userChats = await _context.Chats
                .Where(x => x.Users.Any(u => u.UserId == userId))
                .Select(x => x.Id)
                .ToListAsync();
            return userChats;   // return list of chat ids for the user
        }

        public async Task SaveMessageAsync(Message message)
        {
            if (message == null || message.ChatId <= 0)
            {
                throw new ArgumentException("Invalid message data.");
            }
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }
    }
}