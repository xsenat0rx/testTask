using Microsoft.AspNetCore.Mvc;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
	{
		var result = await _authService.RegisterAsync(registerDto);

		if (!result.IsSuccess)
			return BadRequest(result);

		return Ok(result);
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
	{
		var result = await _authService.LoginAsync(loginDto);

		if (!result.IsSuccess)
			return Unauthorized(result);

		return Ok(result);
	}
}