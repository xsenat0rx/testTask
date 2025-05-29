using Microsoft.EntityFrameworkCore;
using testTaskHub.Hubs;

public class AuthService : IAuthService
{
	private readonly TestTaskDbContext _context;
	private readonly IConfiguration _configuration;
	private readonly ILogger<ChatHub> _logger;

	public AuthService(TestTaskDbContext context, IConfiguration configuration, ILogger<ChatHub> logger)
	{
		_context = context;
		_configuration = configuration;
		_logger = logger;
	}

	public async Task<AuthResponse> RegisterAsync(RegisterDto registerDto)
	{
		if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
		{
			_logger.LogError($"Attempt to register with existing username {registerDto.Username}");
			return new AuthResponse { Message = "Username already exists", IsSuccess = false };
		}

		if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
		{
			_logger.LogError($"Attempt to register with existing email {registerDto.Email}");
			return new AuthResponse { Message = "Email already exists", IsSuccess = false };
		}

		var user = new User
		{
			Username = registerDto.Username,
			Email = registerDto.Email,
			PasswordHash = PasswordHelper.HashPassword(registerDto.Password),
			CreatedAt = DateTime.UtcNow
		};

		await _context.Users.AddAsync(user);
		await _context.SaveChangesAsync();

		_logger.LogInformation($"User {user.Username} registered in app with email {user.Email}");
		return new AuthResponse { Message = "User registered successfully", IsSuccess = true };
	}

	public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);

		if (user == null || !PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
		{
			_logger.LogError($"Incorrect login attempt with username {loginDto.Username}");
			return new AuthResponse { Message = "Invalid username or password", IsSuccess = false };
		}

		var token = JwtHelper.GenerateToken(user, _configuration);

		_logger.LogInformation($"User {loginDto.Username} login to app");
		return new AuthResponse
		{
			IsSuccess = true,
			Token = token,
			Message = "Login successful"
		};
	}
}