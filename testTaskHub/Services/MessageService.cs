using testTaskHub.Hubs;

public class MessageService : IMessageService
{
	private readonly TestTaskDbContext _context;
	private readonly ILogger<ChatHub> _logger;

	public MessageService(TestTaskDbContext context, ILogger<ChatHub> logger)
	{
		_context = context;
		_logger = logger;
	}

	public async Task<MessageDto> UpdateMessageAsync(int messageId, UpdateMessageDto dto, int userId)
	{
		var message = await _context.Messages.FindAsync(messageId);

		if (message == null)
		{
			_logger.LogError($"User {userId} tried update message {messageId} to {dto.Text}, but message not found");
			throw new KeyNotFoundException("Message not found");
		}

		if (message.SenderId != userId)
		{
			_logger.LogError($"User {userId} tried update message {messageId} to {dto.Text}, but the message does not belong to him");
			throw new UnauthorizedAccessException("You can only edit your own messages");
		}

		string oldMessage = message.Text;
		message.Text = dto.Text;

		await _context.SaveChangesAsync();
		_logger.LogInformation($"User {userId} updated message {messageId} from {oldMessage} to {dto.Text}");

		return MapToDto(message);
	}

	public async Task<bool> DeleteMessageAsync(int messageId, int userId)
	{
		var message = await _context.Messages.FindAsync(messageId);

		if (message == null)
		{
			_logger.LogError($"User {userId} tried delete message {messageId}, but message not found");
			throw new KeyNotFoundException("Message not found");
		}

		if (message.SenderId != userId)
		{
			_logger.LogError($"User {userId} tried delete message {messageId}, but the message does not belong to him");
			throw new UnauthorizedAccessException("You can only delete your own messages");
		}

		string messageText = message.Text;
		_context.Messages.Remove(message);
		await _context.SaveChangesAsync();
		_logger.LogInformation($"User {userId} deleted message {messageId} with text {messageText}");

		return true;
	}

	private static MessageDto MapToDto(Message message)
	{
		return new MessageDto
		{
			Id = message.Id,
			Text = message.Text
		};
	}
}