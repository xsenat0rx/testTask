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

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Настройка составного первичного ключа для ChatUser
		modelBuilder.Entity<ChatUser>()
			.HasKey(cu => new { cu.UserId, cu.ChatId });

		// Настройка связи между User и ChatUser
		modelBuilder.Entity<ChatUser>()
			.HasOne(cu => cu.User)
			.WithMany(u => u.Chats)
			.HasForeignKey(cu => cu.UserId);

		// Настройка связи между Chat и ChatUser
		modelBuilder.Entity<ChatUser>()
			.HasOne(cu => cu.Chat)
			.WithMany(c => c.Users)
			.HasForeignKey(cu => cu.ChatId);
	}
}