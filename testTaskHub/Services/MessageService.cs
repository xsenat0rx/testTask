public class MessageService : IMessageService
{
	private readonly TestTaskDbContext _context;

	public MessageService(TestTaskDbContext context)
	{
		_context = context;
	}

	/*public async Task<MessageDto> CreateMessageAsync(CreateMessageDto dto, int userId)
	{
		var message = new Message
		{
			Text = dto.Text,
			SenderId = userId,
			ChatId = dto.ChatId,
			SentAt = DateTime.UtcNow
		};

		_context.Messages.Add(message);
		await _context.SaveChangesAsync();

		return MapToDto(message);
	}

	public async Task<MessageDto> UpdateMessageAsync(int messageId, UpdateMessageDto dto, int userId)
	{
		var message = await _context.Messages.FindAsync(messageId);

		if (message == null)
			throw new KeyNotFoundException("Message not found");

		if (message.SenderId != userId)
			throw new UnauthorizedAccessException("You can only edit your own messages");

		message.Text = dto.Text;

		await _context.SaveChangesAsync();

		return MapToDto(message);
	}*/

	public async Task<bool> DeleteMessageAsync(int messageId, int userId)
	{
		var message = await _context.Messages.FindAsync(messageId);

		if (message == null)
			throw new KeyNotFoundException("Message not found");

		if (message.SenderId != userId)
			throw new UnauthorizedAccessException("You can only delete your own messages");

		_context.Messages.Remove(message);
		await _context.SaveChangesAsync();

		return true;
	}

	/*private static MessageDto MapToDto(Message message)
	{
		return new MessageDto
		{
			Id = message.Id,
			Text = message.Text,
			SentAt = message.SentAt,
			ChatId = message.ChatId,
			SenderId = message.SenderId
		};
	}*/
}