public interface IAuthService
{
	public Task<AuthResponse> RegisterAsync(RegisterDto registerDto);
	public Task<AuthResponse> LoginAsync(LoginDto loginDto);
}