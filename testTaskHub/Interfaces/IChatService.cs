using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testTaskHub.Interfaces
{
    public interface IChatService
    {
        public Task<int> CreateChatAsync(string chatName, int userId);
        public Task<List<Message>> GetChatMessagesAsync(int chatId);
        public Task<List<int>> GetUserChatsAsync(int userId);
        public Task SaveMessageAsync(Message message);
        Task<List<Chat>> SearchChatsAsync(int userId, string query);
        Task<List<Message>> SearchMessagesAsync(int chatId, string query);
    }
}