# Chat Application

Чат-приложение с поддержкой real-time обмена сообщениями, JWT-аутентификацией, поиском по чатам и сообщениям, интеграционными тестами и полной поддержкой Docker.

---

## 🚀 Возможности

- Регистрация и аутентификация пользователей (JWT)
- Real-time чат через SignalR (`/chathub`)
- RESTful API для чатов и сообщений
- Поиск по чатам и сообщениям
- Пагинация сообщений
- PostgreSQL (Entity Framework Core)
- Swagger API документация
- Логирование событий через Serilog (в SQLite)
- Интеграционные и unit-тесты (xUnit)
- Docker Compose для быстрого запуска

---

## ⚡ Быстрый старт

### 1. Клонирование репозитория

```sh
git clone https://github.com/xsenat0rx/testTask.git
cd testTask
```

### 2. Запуск через Docker Compose

```sh
docker compose up --build
```

Приложение будет доступно по адресу [http://localhost:8080](http://localhost:8080).

> **ℹ️ Окружение сервиса `web_api` (строка подключения к БД, параметры JWT и др.) можно изменить прямо в файле `docker-compose.yml` в секции `environment`.**

---

## 📝 API Endpoints

| Method | Endpoint                                  | Описание                  |
| ------ | ----------------------------------------- | ------------------------- |
| POST   | /api/auth/register                        | Регистрация пользователя  |
| POST   | /api/auth/login                           | Вход (JWT)                |
| GET    | /api/chats                                | Список чатов пользователя |
| POST   | /api/chats                                | Создать новый чат         |
| GET    | /api/chats/{id}/messages                  | История сообщений в чате  |
| POST   | /api/chats/{id}/messages                  | Отправить сообщение       |
| GET    | /api/chats/search?query=...               | Поиск чатов по названию   |
| GET    | /api/chats/{id}/messages/search?query=... | Поиск сообщений в чате    |

### SignalR Hub

- **/chathub**
  - `JoinChat(int chatId)`
  - `SendMessage(int chatId, string text)`

---

## 🔍 Поиск

- **Чаты:**  
  `GET /api/chats/search?query=текст`
- **Сообщения:**  
  `GET /api/chats/{chatId}/messages/search?query=текст`

---

## 📚 Документация API (Swagger)

Swagger UI доступен после запуска приложения по адресу:  
[http://localhost:8080/swagger](http://localhost:8080/swagger)

### Примеры запросов

#### Аутентификация

**POST `/api/auth/register`**  
Регистрация нового пользователя.

```json
{
  "username": "Anna",
  "email": "anna@yandex.ru",
  "password": "AnnaPassword"
}
```

**POST `/api/auth/login`**  
Аутентификация пользователя, получение JWT.

```json
{
  "email": "anna@yandex.ru",
  "password": "AnnaPassword"
}
```

**Пример ответа:**

```json
{
  "isSuccess": true,
  "token": "jwt-token",
  "message": "Login successful"
}
```

---

#### Чаты

**GET `/api/chats`**  
Получить список чатов пользователя.

**POST `/api/chats`**  
Создать новый чат.

```json
{
  "name": "New Chat"
}
```

**GET `/api/chats/{id}/messages`**  
Получить сообщения в чате.

**POST `/api/chats/{id}/messages`**  
Отправить сообщение в чат.

```json
{
  "text": "Привет!"
}
```

---

#### Поиск

**GET `/api/chats/search?query=текст`**  
Поиск чатов по названию.

**GET `/api/chats/{chatId}/messages/search?query=текст`**  
Поиск сообщений в чате.

---

#### SignalR Hub

- **/chathub** — real-time обмен сообщениями.
- Для авторизации в SignalR используйте JWT-токен в query-параметре `access_token`.

---

#### Авторизация

Для доступа к защищённым эндпоинтам используйте JWT-токен в заголовке:

```
Authorization: Bearer <ваш токен>
```

В Swagger UI нажмите "Authorize" и вставьте ваш JWT-токен для выполнения защищённых запросов.

---

#### Конфиг

**GET `/config`**  
Получить текущие параметры окружения и строки подключения (для отладки).

---

## 🛠️ Разработка и тестирование

- .NET 8
- ASP.NET Core Web API
- SignalR
- Entity Framework Core + PostgreSQL
- Serilog (логи в `Logs/log.db`)
- xUnit (интеграционные и unit-тесты)

### Запуск тестов

```sh
dotnet test
```

---

## 🐳 Требования

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/) (если не использовать Docker Compose)

---

## 📄 Лицензия

MIT

---

## 👥 Авторы

- xsenat0rx
- skyd4emon

---
