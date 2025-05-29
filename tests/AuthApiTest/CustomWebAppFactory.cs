using System.ComponentModel;
using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

//inherit from WebApplicationFactory<TProgram> to configurate own in-memory database
public class CustomWebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		base.ConfigureWebHost(builder);
		builder.UseEnvironment("Test");
		builder.ConfigureServices(services =>
		{
			var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AuthentificationDBContext>));
			if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);
			var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
			if (dbConnectionDescriptor != null) services.Remove(dbConnectionDescriptor);

			services.AddSingleton<DbConnection>(container =>
			{
				var connection = new SqliteConnection("DataSource=:memory:");
				connection.Open();
				return connection;
			});
			services.AddDbContext<AuthentificationDBContext>((container, options) =>
			{
				var connection = container.GetRequiredService<DbConnection>();
				options.UseSqlite(connection);
			});
			services.AddScoped<DBInitializerAndSeeder>();
		});
	}
}