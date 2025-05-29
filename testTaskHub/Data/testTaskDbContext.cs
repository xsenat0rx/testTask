using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

public class TestTaskDbContext : DbContext
{
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Message> Messages { get; set; } = null!;
	public DbSet<Chat> Chats { get; set; } = null!;
	public DbSet<ChatUser> ChatUsers { get; set; } = null!;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.SetBasePath(Directory.GetCurrentDirectory())
			.Build();
		optionsBuilder.UseNpgsql(config.GetConnectionString("TestTaskConnection"));
	}
}