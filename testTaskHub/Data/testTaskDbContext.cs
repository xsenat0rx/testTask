using Microsoft.EntityFrameworkCore;

public class TestTaskDbContext : DbContext
{
	public DbSet<Billing> Billings { get; set; } = null!;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.SetBasePath(Directory.GetCurrentDirectory())
			.Build();
		optionsBuilder.UseNpgsql(config.GetConnectionString("TestTaskConnection"));
	}
}