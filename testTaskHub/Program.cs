using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using testTaskHub.Hubs;
using Serilog;
using Serilog.Sinks.SQLite;

<<<<<<< HEAD
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TestTaskDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSignalR();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.SQLite(
        sqliteDbPath: "Logs/log.db",  // SQLite database path
        tableName: "Logs",
        storeTimestampInUtc: true)
    .CreateLogger();

builder.Host.UseSerilog();

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
         options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
=======
namespace testTaskHub
>>>>>>> d168b53e1f340dd5220ebefb39ae5ccf8eae4974
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.MapHub<ChatHub>("/chathub");
app.MapControllers();
app.Run();
=======
            builder.Services.AddControllers();
            builder.Services.AddDbContext<TestTaskDbContext>();
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IMessageService, MessageService>();

            // Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        ValidAudience = builder.Configuration["JWT:ValidAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                    };
                });

            builder.Services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "ChatApplicationAPI",
                    Version = "v1"
                });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }});
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }

    }
}
>>>>>>> d168b53e1f340dd5220ebefb39ae5ccf8eae4974
