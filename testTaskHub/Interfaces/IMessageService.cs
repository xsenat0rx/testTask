public interface IMessageService
{
	//public Task<AuthResponse> RegisterAsync(RegisterDto registerDto);
	public Task<bool> DeleteMessageAsync(int messageId, int userId);

}