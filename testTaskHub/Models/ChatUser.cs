using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(UserId), nameof(ChatId))]
public class ChatUser
{
	public int UserId { get; set; }
	public User User { get; set; }

	public int ChatId { get; set; }
	public Chat Chat { get; set; }
}