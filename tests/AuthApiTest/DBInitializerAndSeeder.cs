using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class DBInitializerAndSeeder(ILogger<DBInitializerAndSeeder> logger,
	TestTaskDbContext testTaskDbContext)
{
	private readonly ILogger<DBInitializerAndSeeder> _logger = logger;
	private readonly TestTaskDbContext _context = testTaskDbContext;

	public async Task InitialiseAsync()
	{
		try
		{
			//_context.Database.EnsureDeleted();
			await _context.Database.EnsureCreatedAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while initialising the database");
			throw;
		}
	}
	public async Task SeedDataAsync()
	{
		try
		{
			await TrySeedDataAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred while seeding the database");
			throw;
		}
	}
	private async Task TrySeedDataAsync()
	{
		var user1 = new User { Username = "Andrey", Email = "andrey@yandex.ru", PasswordHash = PasswordHelper.HashPassword("AndreyPassword"), CreatedAt = DateTime.UtcNow };
		var user2 = new User { Username = "Sergey", Email = "sergey@mail.ru", PasswordHash = PasswordHelper.HashPassword("SergeyPassword"), CreatedAt = DateTime.UtcNow };
		var user3 = new User { Username = "Anna", Email = "anna@gmail.com", PasswordHash = PasswordHelper.HashPassword("AnnaPassword"), CreatedAt = DateTime.UtcNow };
		await _context.Users.AddAsync(user1);
		await _context.Users.AddAsync(user2);
		await _context.Users.AddAsync(user3);
		await _context.SaveChangesAsync();

		var chat1 = new Chat { Name = "House", CreatedAt = DateTime.UtcNow, IsGroup = false };
		await _context.Chats.AddAsync(chat1);
		await _context.SaveChangesAsync();

		var message1 = new Message { Text = "Old message text Andrey", SentAt = DateTime.UtcNow, ChatId = 1, SenderId = 1 }; //Andrey
		var message2 = new Message { Text = "Old message text Sergey", SentAt = DateTime.UtcNow, ChatId = 1, SenderId = 2 }; //Sergey
		await _context.Messages.AddAsync(message1);
		await _context.Messages.AddAsync(message2);

		await _context.SaveChangesAsync();
	}
}