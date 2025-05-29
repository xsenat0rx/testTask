using FluentAssertions;
using testTaskHub;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

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
    /*[Fact]
    public async void RegisterExistingEmailTest()
    {
        SeederAsync();
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = "user1@funflex.com",
            UserName = "SixthUser",
            Password = "dfrggfgfg!1AAAA"
        };
        var jsonContent = JsonSerializer.Serialize(userRegisterRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/User/Register", content);
        if (response.IsSuccessStatusCode)
        {
            response.Should().NotBeNull();
            var userRegisterResponse = await response.Content.ReadFromJsonAsync<PlatformUsersResponseTest<bool>>();
            userRegisterResponse.Should().NotBeNull();
            userRegisterResponse?.IsSucceed.Should().BeFalse();
            userRegisterResponse?.Data.Should().BeFalse();
            userRegisterResponse?.Messages.Any(m => m.Key == "DuplicateEmail").Should().BeTrue();
        }
        else
        {
            Assert.Fail("Api Register Fail");
        }
    }
    [Fact]
    public async void RegisterInvalidUserNameTest()
    {
        SeederAsync();
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = "user7@funflex.com",
            UserName = "юзер7",
            Password = "dfrggfgfg!1AAAA"
        };
        var jsonContent = JsonSerializer.Serialize(userRegisterRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/User/Register", content);
        if (response.IsSuccessStatusCode)
        {
            response.Should().NotBeNull();
            var userRegisterResponse = await response.Content.ReadFromJsonAsync<PlatformUsersResponseTest<bool>>();
            userRegisterResponse.Should().NotBeNull();
            userRegisterResponse?.IsSucceed.Should().BeFalse();
            userRegisterResponse?.Data.Should().BeFalse();
            userRegisterResponse?.Messages.Any(m => m.Key == "InvalidUserName").Should().BeTrue();
        }
        else
        {
            Assert.Fail("Api Register Fail");
        }
    }
    [Fact]
    public async void RegisterInvalidEmailTest()
    {
        SeederAsync();
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = "user8",
            UserName = "EightUser",
            Password = "dfrggfgfg!1AAAA"
        };
        var jsonContent = JsonSerializer.Serialize(userRegisterRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/User/Register", content);
        if (response.IsSuccessStatusCode)
        {
            response.Should().NotBeNull();
            var userRegisterResponse = await response.Content.ReadFromJsonAsync<PlatformUsersResponseTest<bool>>();
            userRegisterResponse.Should().NotBeNull();
            userRegisterResponse?.IsSucceed.Should().BeFalse();
            userRegisterResponse?.Data.Should().BeFalse();
            userRegisterResponse?.Messages.Any(m => m.Key == "InvalidEmail").Should().BeTrue();
        }
        else
        {
            Assert.Fail("Api Register Fail");
        }
    }
    [Fact]
    public async void RegisterBadPasswordFourConditionTest()
    {
        SeederAsync();
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = "user9@funflex.com",
            UserName = "NinthUser",
            Password = "fff"
        };
        var jsonContent = JsonSerializer.Serialize(userRegisterRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/User/Register", content);
        if (response.IsSuccessStatusCode)
        {
            response.Should().NotBeNull();
            var userRegisterResponse = await response.Content.ReadFromJsonAsync<PlatformUsersResponseTest<bool>>();
            userRegisterResponse.Should().NotBeNull();
            userRegisterResponse?.IsSucceed.Should().BeFalse();
            userRegisterResponse?.Data.Should().BeFalse();
            userRegisterResponse?.Messages.Any(m => m.Key == "PasswordTooShort").Should().BeTrue();
            userRegisterResponse?.Messages.Any(m => m.Key == "PasswordRequiresNonAlphanumeric").Should().BeTrue();
            userRegisterResponse?.Messages.Any(m => m.Key == "PasswordRequiresDigit").Should().BeTrue();
            userRegisterResponse?.Messages.Any(m => m.Key == "PasswordRequiresUpper").Should().BeTrue();
        }
        else
        {
            Assert.Fail("Api Register Fail");
        }
    }
    [Fact]
    public async void RegisterBadPasswordLastConditionTest()
    {
        SeederAsync();
        var userRegisterRequest = new UserRegisterRequest
        {
            Email = "user10@funflex.com",
            UserName = "TenthUser",
            Password = "DFRGGFGFG!1AAAA"
        };
        var jsonContent = JsonSerializer.Serialize(userRegisterRequest);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/User/Register", content);
        if (response.IsSuccessStatusCode)
        {
            response.Should().NotBeNull();
            var userRegisterResponse = await response.Content.ReadFromJsonAsync<PlatformUsersResponseTest<bool>>();
            userRegisterResponse.Should().NotBeNull();
            userRegisterResponse?.IsSucceed.Should().BeFalse();
            userRegisterResponse?.Data.Should().BeFalse();
            userRegisterResponse?.Messages.Any(m => m.Key == "PasswordRequiresLower").Should().BeTrue();
        }
        else
        {
            Assert.Fail("Api Register Fail");
        }
    }*/
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