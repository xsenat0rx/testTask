# Chat Application

Чат-приложение, работающее в режиме реального времени. Находится в стадии разработки.

---

## Features

- Регистрация пользователей и JWT-аутентификация
- Real-time чат с использованием SignalR (`/chathub`)
- RESTful API для чатов и сообщений
- База данных PostgreSQL (через Entity Framework Core)
- Документация Swagger API
- Логирование с использованием Serilog (в БД SQLite)
- Интеграционные тесты
- Поддержка Docker Compose

---

## Getting Started

### 1. Clone the repository

```sh
git clone https://github.com/xsenat0rx/testTask.git
cd testTaskHub
```

### 2. Configuration

Создайте `appsettings.Development.json` в директории `testTaskHub`:

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
docker compose up
```

Приложение будет доступно по адресу [http://localhost:5000](http://localhost:5000).

### 4. Run migrations

```sh
dotnet ef database update --project testTaskHub
```

---

## API Endpoints

| Method | Endpoint                 | Description                      |
| ------ | ------------------------ | -------------------------------- |
| POST   | /api/auth/register       | Регистрация нового пользователя  |
| POST   | /api/auth/login          | Аутентификация (JWT)             |
| GET    | /api/chats               | Получение чатов пользователя     |
| POST   | /api/chats               | Создание нового чата             |
| GET    | /api/chats/{id}/messages | ПОлучение истории сообщений чата |
| POST   | /api/chats/{id}/messages | Отправить сообщение              |

### Real-time

- **SignalR Hub:** `/chathub`
  - `JoinChat(int chatId)`
  - `SendMessage(int chatId, string text)`

---

---

## Logging

Логи хранятся в БД SQLite `Logs/log.db`.

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

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/) (if not using Docker Compose)

---

## License

MIT

---

## Authors

xsenat0rx
skyd4emon
