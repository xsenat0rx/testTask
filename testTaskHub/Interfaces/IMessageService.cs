public interface IMessageService
{
	public Task<bool> DeleteMessageAsync(int messageId, int userId);
	public Task<MessageDto> UpdateMessageAsync(int messageId, UpdateMessageDto dto, int userId);

}