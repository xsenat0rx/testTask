using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

public class TestTaskDbContext : DbContext
{
	public TestTaskDbContext(DbContextOptions<TestTaskDbContext> options)
	: base(options)
	{
	}
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Message> Messages { get; set; } = null!;
	public DbSet<Chat> Chats { get; set; } = null!;
	public DbSet<ChatUser> ChatUsers { get; set; } = null!;

}