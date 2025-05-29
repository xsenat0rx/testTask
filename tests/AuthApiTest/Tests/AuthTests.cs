using FluentAssertions;
using testTaskHub;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Net;

namespace AuthApiTest;

public class AuthTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly HttpClient _client;
    public AuthTests(CustomWebAppFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }
    [Fact]
    public async void LoginSuccessTest()
    {
        SeederAsync();
        LoginDto loginDto = new LoginDto
        {
            Username = "Andrey",
            Password = "AndreyPassword"
        };
        var jsonContent = JsonSerializer.Serialize(loginDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/auth/login", content);
        if (response.IsSuccessStatusCode)
        {
            response.Should().NotBeNull();
            var userLoginResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            userLoginResponse?.IsSuccess.Should().BeTrue();
            userLoginResponse?.Token.Should().NotBeNull();
            userLoginResponse?.Message.Should().Be("Login successful");
        }
        else
        {
            Assert.Fail("Api Login Failed");
        }
    }
    [Fact]
    public async void LoginWrongPasswordTest()
    {
        SeederAsync();
        LoginDto loginDto = new LoginDto
        {
            Username = "Andrey",
            Password = "AndreyPassword1"   //wrong password
        };
        var jsonContent = JsonSerializer.Serialize(loginDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/auth/login", content);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            response.Should().NotBeNull();
            var userLoginResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            userLoginResponse.Should().NotBeNull();
            userLoginResponse?.IsSuccess.Should().BeFalse();
            userLoginResponse?.Token.Should().BeNull();
            userLoginResponse?.Message.Should().Be("Invalid username or password");
        }
        else
        {
            Assert.Fail("Api Login Failed");
        }
    }
    [Fact]
    public async void LoginWrongUsernameTest()
    {
        SeederAsync();
        LoginDto loginDto = new LoginDto
        {
            Username = "Andreyy", //wrong usernabe
            Password = "AndreyPassword"
        };
        var jsonContent = JsonSerializer.Serialize(loginDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/auth/login", content);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            response.Should().NotBeNull();
            var userLoginResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            userLoginResponse.Should().NotBeNull();
            userLoginResponse?.IsSuccess.Should().BeFalse();
            userLoginResponse?.Token.Should().BeNull();
            userLoginResponse?.Message.Should().Be("Invalid username or password");
        }
        else
        {
            Assert.Fail("Api Login Failed");
        }
    }
    [Fact]
    public async void RegisterSuccessTest()
    {
        SeederAsync();
        var userRegisterRequest = new RegisterDto
        {
            Email = "dmitry@yahoo.com",
            Username = "Dmitry",
            Password = "DmitryPassword"
        };
        var jsonContent = JsonSerializer.Serialize(userRegisterRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/auth/register", content);
        if (response.IsSuccessStatusCode)
        {
            response.Should().NotBeNull();
            var userRegisterResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            userRegisterResponse.Should().NotBeNull();
            userRegisterResponse?.IsSuccess.Should().BeTrue();
            userRegisterResponse?.Token.Should().BeNull();
            userRegisterResponse?.Message.Should().Be("User registered successfully");
        }
        else
        {
            Assert.Fail("Api Register Failed");
        }
    }
    [Fact]
    public async void RegisterExistingUsernameTest()
    {
        SeederAsync();
        var userRegisterRequest = new RegisterDto
        {
            Email = "dmitry@yahoo.com",
            Username = "Andrey",  //existing username
            Password = "DmitryPassword"
        };
        var jsonContent = JsonSerializer.Serialize(userRegisterRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/auth/register", content);
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            response.Should().NotBeNull();
            var userRegisterResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            userRegisterResponse.Should().NotBeNull();
            userRegisterResponse?.IsSuccess.Should().BeFalse();
            userRegisterResponse?.Token.Should().BeNull();
            userRegisterResponse?.Message.Should().Be("Username already exists");
        }
        else
        {
            Assert.Fail("Api Register Fail");
        }
    }
    [Fact]
    public async void RegisterExistingEmailTest()
    {
        SeederAsync();
        var userRegisterRequest = new RegisterDto
        {
            Email = "anna@gmail.com",  //existing username
            Username = "Oleg",
            Password = "OlegPassword"
        };
        var jsonContent = JsonSerializer.Serialize(userRegisterRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/auth/register", content);
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            response.Should().NotBeNull();
            var userRegisterResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            userRegisterResponse.Should().NotBeNull();
            userRegisterResponse?.IsSuccess.Should().BeFalse();
            userRegisterResponse?.Token.Should().BeNull();
            userRegisterResponse?.Message.Should().Be("Email already exists");
        }
        else
        {
            Assert.Fail("Api Register Fail");
        }
    }
    [Fact]
    public async void UpdateMessageSuccessTest()
    {
        SeederAsync();
        string token = await GetTokenAsync();
        UpdateMessageDto updateMessageDto = new UpdateMessageDto
        {
            Text = "Andrey new message text"
        };
        var response = await GetUpdateMessageApiAsync(token, updateMessageDto);
        response.Should().NotBeNull();
        if (response.IsSuccessStatusCode)
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var userUpdateResponse = await response.Content.ReadFromJsonAsync<MessageDto>();
            userUpdateResponse.Should().NotBeNull();
            userUpdateResponse?.Id.Should().Be(1);  //id message
            userUpdateResponse?.Text.Should().Be("Andrey new message text");
        }
        else
        {
            Assert.Fail("Api UdpateUser Failed");
        }

    }
    private async Task<HttpResponseMessage> GetUpdateMessageApiAsync(string accessToken, UpdateMessageDto updateMessageDto)
    {
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, "/api/messages/1");
        httpRequestMessage.Headers.Add("Authorization", "Bearer " + accessToken);

        var jsonContent = JsonSerializer.Serialize(updateMessageDto);
        httpRequestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        return await _client.SendAsync(httpRequestMessage);
    }
    private async Task<string> GetTokenAsync()
    {
        LoginDto loginDto = new LoginDto
        {
            Username = "Andrey",
            Password = "AndreyPassword"
        };
        var jsonContent = JsonSerializer.Serialize(loginDto);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/auth/login", content);
        var userLoginResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
        return userLoginResponse?.Token ?? default!;
    }
    private async void SeederAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var initialiser = scope.ServiceProvider.GetRequiredService<DBInitializerAndSeeder>();
            await initialiser.InitialiseAsync();
            await initialiser.SeedDataAsync();
        }
    }
}