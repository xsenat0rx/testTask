using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using testTaskHub.Interfaces;
using Microsoft.AspNetCore.SignalR;
using testTaskHub.Hubs;

namespace testTaskHub.Controllers
{
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly ILogger<ChatController> _logger;
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(ILogger<ChatController> logger, IChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _chatService = chatService;
            _hubContext = hubContext;
        }

        // Helper method to extract and validate userId from claims
        private bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return !string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out userId) && userId > 0;
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> CreateNewChat(string chatName)
        {
            if (string.IsNullOrWhiteSpace(chatName))
            {
                return BadRequest("Chat name cannot be empty.");
            }
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var chatId = await _chatService.CreateChatAsync(chatName, userId);
            return Ok(new { ChatId = chatId });
        }

        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> GetUserChats()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var userChats = await _chatService.GetUserChatsAsync(userId);
            return Ok(userChats);
        }

        [HttpPost("{id}/messages")]
        [Authorize]
        public async Task<IActionResult> SendMessage(int id, [FromBody] string messageText)
        {
            if (id <= 0)
                return BadRequest("Invalid chat ID.");

            if (!TryGetUserId(out var userId))
                return Unauthorized("User is not authenticated.");

            await _hubContext.Clients.Group($"chat-{id}")
                .SendAsync("ReceiveMessage", userId.ToString(), messageText);
            await _chatService.SaveMessageAsync(new Message()
            {
                Text = messageText,
                SentAt = DateTime.UtcNow,
                ChatId = id,
                SenderId = userId,
            });
            return Ok();
        }

        [HttpGet("{id}/messages")]
        [Authorize]
        public async Task<IActionResult> GetChatMessages(
            int id,
            int pageNumber = 1,
            int pageSize = 20)
        {
            if (id <= 0)
                return BadRequest("Invalid chat ID.");

            if (!TryGetUserId(out var userId))
                return Unauthorized("User is not authenticated.");

            var messages = await _chatService.GetChatMessagesAsync(id, pageNumber, pageSize);
            return Ok(messages);
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchChats([FromQuery] string query)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User is not authenticated.");

            var chats = await _chatService.SearchChatsAsync(userId, query);
            return Ok(chats);
        }

        [HttpGet("{id}/messages/search")]
        [Authorize]
        public async Task<IActionResult> SearchMessages(int id, [FromQuery] string query)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized("User is not authenticated.");
            var messages = await _chatService.SearchMessagesAsync(id, query);
            return Ok(messages);
        }
    }
}