public class Chat
{
	public int Id { get; set; }
	public string Name { get; set; }
	public bool IsGroup { get; set; }
	public DateTime CreatedAt { get; set; }
	public ICollection<ChatUser> Users { get; set; }
	public ICollection<Message> Messages { get; set; }
}