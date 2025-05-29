using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql; // Add this at the top
using Microsoft.Data.Sqlite; // Add this at the top

//inherit from WebApplicationFactory<TProgram> to configurate own in-memory database
public class CustomWebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		base.ConfigureWebHost(builder);
		builder.UseEnvironment("Test");
		builder.ConfigureServices(services =>
		{
			services.RemoveAll(typeof(DbContextOptions<TestTaskDbContext>));
			services.RemoveAll(typeof(TestTaskDbContext));
			services.RemoveAll(typeof(DbConnection));
			services.RemoveAll(typeof(IDbContextFactory<TestTaskDbContext>));
			// If you have other context-related services, remove them too

			services.AddSingleton<DbConnection>(container =>
			{
				var connection = new SqliteConnection("DataSource=:memory:");
				connection.Open();
				return connection;
			});
			services.AddDbContext<TestTaskDbContext>((container, options) =>
			{
				var connection = container.GetRequiredService<DbConnection>();
				options.UseSqlite(connection);
			});
			services.AddScoped<DBInitializerAndSeeder>();
		});
	}
}