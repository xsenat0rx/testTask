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
    }
}