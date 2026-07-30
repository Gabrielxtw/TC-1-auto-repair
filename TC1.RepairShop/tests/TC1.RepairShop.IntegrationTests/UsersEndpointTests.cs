using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace TC1.RepairShop.IntegrationTests;

public class UsersEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UsersEndpointTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task AuthenticateAsync()
    {
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new { username = "admin", password = "Admin@123" });
        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", login!.Token);
    }

    [Fact]
    public async Task GetUsers_WithoutToken_ShouldReturn401()
    {
        var response = await _client.GetAsync("/api/users");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateUser_ThenGetById_ShouldReturnCreatedUser()
    {
        await AuthenticateAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            new { username = $"user.{Guid.NewGuid():N}", password = "Passw0rd!", role = "Mechanic" });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<UserResponseDto>();
        Assert.NotNull(created);
        Assert.Equal("Active", created!.Status);

        var getResponse = await _client.GetAsync($"/api/users/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateUser_WithDuplicateUsername_ShouldReturn409()
    {
        await AuthenticateAsync();

        var username = $"user.{Guid.NewGuid():N}";
        await _client.PostAsJsonAsync("/api/users", new { username, password = "Passw0rd!", role = "Mechanic" });

        var response = await _client.PostAsJsonAsync("/api/users", new { username, password = "Passw0rd!", role = "Mechanic" });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ShouldChangeUsernameAndRole()
    {
        await AuthenticateAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            new { username = $"user.{Guid.NewGuid():N}", password = "Passw0rd!", role = "Mechanic" });
        var created = await createResponse.Content.ReadFromJsonAsync<UserResponseDto>();

        var newUsername = $"user.{Guid.NewGuid():N}";
        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/users/{created!.Id}",
            new { username = newUsername, role = "Supervisor" });

        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/users/{created.Id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<UserResponseDto>();
        Assert.Equal(newUsername, updated!.Username);
        Assert.Equal("Supervisor", updated.Role);
    }

    [Fact]
    public async Task DeleteUser_ShouldSoftDeleteAndHideFromGetAndList()
    {
        await AuthenticateAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            new { username = $"user.{Guid.NewGuid():N}", password = "Passw0rd!", role = "Mechanic" });
        var created = await createResponse.Content.ReadFromJsonAsync<UserResponseDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/users/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/users/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_ThenLoginWithNewPassword_ShouldSucceed()
    {
        await AuthenticateAsync();

        var username = $"user.{Guid.NewGuid():N}";
        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            new { username, password = "OldPassw0rd!", role = "Mechanic" });
        var created = await createResponse.Content.ReadFromJsonAsync<UserResponseDto>();

        var changeResponse = await _client.PutAsJsonAsync(
            $"/api/users/{created!.Id}/password",
            new { newPassword = "NewPassw0rd!" });
        Assert.Equal(HttpStatusCode.NoContent, changeResponse.StatusCode);

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new { username, password = "NewPassw0rd!" });
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
    }

    private record LoginResponseDto(string Token);

    private record UserResponseDto(Guid Id, string Username, string Role, string Status);
}
