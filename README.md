# Chat Application

A real-time chat application built with ASP.NET Core, SignalR, JWT authentication, and PostgreSQL.  
Includes REST API, real-time messaging, and is ready for Docker deployment.

---

## Features

- User registration and JWT authentication
- Real-time chat with SignalR (`/chathub`)
- RESTful API for chats and messages
- PostgreSQL database (via Entity Framework Core)
- Swagger API documentation
- Logging with Serilog (to SQLite)
- Unit and integration tests
- Docker Compose support

---

## Getting Started

### 1. Clone the repository

```sh
git clone <your-repo-url>
cd <your-repo-folder>
```

### 2. Configuration

Create `appsettings.Development.json` in the `testTaskHub` folder:

```json
{
  "ConnectionStrings": {
    "TestTaskConnection": "Host=localhost;Port=5432;Database=test_db;Username=postgres;Password=yourpassword"
  },
  "JWT": {
    "ValidIssuer": "your_issuer",
    "ValidAudience": "your_audience",
    "Secret": "your_secret_key"
  }
}
```

### 3. Run with Docker

```sh
docker compose up --build
```

The app will be available at [http://localhost:5000](http://localhost:5000).

### 4. Run migrations (if needed)

```sh
dotnet ef database update --project testTaskHub
```

---

## API Endpoints

| Method | Endpoint                 | Description              |
| ------ | ------------------------ | ------------------------ |
| POST   | /api/auth/register       | Register a new user      |
| POST   | /api/auth/login          | User login (JWT)         |
| GET    | /api/chats               | Get user chats           |
| POST   | /api/chats               | Create a new chat        |
| GET    | /api/chats/{id}/messages | Get chat message history |
| POST   | /api/chats/{id}/messages | Send a message           |

### Real-time

- **SignalR Hub:** `/chathub`
  - `JoinChat(int chatId)`
  - `SendMessage(int chatId, string text)`

---

## Testing

Run all tests:

```sh
dotnet test
```

---

## Logging

Logs are stored in `Logs/log.db` (SQLite database) using Serilog.

---

## Development

- .NET 8
- ASP.NET Core Web API
- SignalR
- Entity Framework Core
- PostgreSQL
- Serilog
- xUnit

---

## Useful Commands

- **Run the app:**  
  `dotnet run --project testTaskHub`
- **Apply migrations:**  
  `dotnet ef database update --project testTaskHub`
- **Run tests:**  
  `dotnet test`

---

## API Documentation

Swagger UI is available at `/swagger` in development mode.

---

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/) (if not using Docker Compose)

---

## License

MIT

---

## Author

[Your Name]  
[Your Contact or GitHub]
