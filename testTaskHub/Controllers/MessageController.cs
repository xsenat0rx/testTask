using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/messages")]
[ApiController]
[Authorize]
public class MessagesController : ControllerBase
{
	private readonly IMessageService _messageService;

	public MessagesController(IMessageService messageService)
	{
		_messageService = messageService;
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateMessage(int id, [FromBody] UpdateMessageDto dto)
	{
		try
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var message = await _messageService.UpdateMessageAsync(id, dto, userId);
			return Ok(message);
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (UnauthorizedAccessException ex)
		{
			return Unauthorized(ex.Message);
		}
	}
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteMessage(int id)
	{
		try
		{
			var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
			var result = await _messageService.DeleteMessageAsync(id, userId);
			return Ok(new { Success = result });
		}
		catch (KeyNotFoundException ex)
		{
			return NotFound(ex.Message);
		}
		catch (UnauthorizedAccessException ex)
		{
			return Unauthorized(ex.Message);
		}
	}
}