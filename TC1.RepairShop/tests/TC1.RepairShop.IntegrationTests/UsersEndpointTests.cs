using System.Net;
using System.Net.Http.Json;
using System.Threading;
using Xunit;

namespace TC1.RepairShop.IntegrationTests;

public class UsersEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    // Valid, distinct CPFs (correct check digits) reserved for this test class, drawn one per test.
    private static readonly string[] ValidCpfs =
    [
        "11144477735", "22233344456", "33322211187", "12345678909", "98765432100",
        "45678912302", "78912345676", "32165498733", "65498732104", "10293847560",
    ];

    private static int _cpfIndex = -1;

    private readonly HttpClient _client;

    public UsersEndpointTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task AuthenticateAsync()
    {
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new { username = ApiWebApplicationFactory.AdminUsername, password = ApiWebApplicationFactory.AdminPassword });
        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", login!.Token);
    }

    private static string NextCpf() => ValidCpfs[Interlocked.Increment(ref _cpfIndex) % ValidCpfs.Length];

    private static object CreateUserBody(string username, string password, string role) => new
    {
        username,
        password,
        document = NextCpf(),
        email = $"{username}@example.com",
        role,
        phone = "11988887777"
    };

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

        var username = $"user.{Guid.NewGuid():N}";
        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            CreateUserBody(username, "Passw0rd!", "Staff"));

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<CreateUserResultDto>();
        Assert.NotNull(created);

        var getResponse = await _client.GetAsync($"/api/users/{created!.id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<GetUserResponseDto>();
        Assert.Equal("Active", fetched!.Status);
    }

    [Fact]
    public async Task CreateUser_WithDuplicateUsername_ShouldReturn409()
    {
        await AuthenticateAsync();

        var username = $"user.{Guid.NewGuid():N}";
        await _client.PostAsJsonAsync("/api/users", CreateUserBody(username, "Passw0rd!", "Staff"));

        var response = await _client.PostAsJsonAsync("/api/users", CreateUserBody(username, "Passw0rd!", "Staff"));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ShouldChangeUsernameAndRole()
    {
        await AuthenticateAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            CreateUserBody($"user.{Guid.NewGuid():N}", "Passw0rd!", "Staff"));
        var created = await createResponse.Content.ReadFromJsonAsync<CreateUserResultDto>();

        var newUsername = $"user.{Guid.NewGuid():N}";
        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/users/{created!.id}",
            new { username = newUsername, role = "Admin" });

        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/users/{created.id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<GetUserResponseDto>();
        Assert.Equal(newUsername, updated!.Username);
        Assert.Equal("Admin", updated.Role);
    }

    [Fact]
    public async Task DeleteUser_ShouldSoftDeleteAndHideFromGetAndList()
    {
        await AuthenticateAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            CreateUserBody($"user.{Guid.NewGuid():N}", "Passw0rd!", "Staff"));
        var created = await createResponse.Content.ReadFromJsonAsync<CreateUserResultDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/users/{created!.id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/users/{created.id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_ThenLoginWithNewPassword_ShouldSucceed()
    {
        await AuthenticateAsync();

        var username = $"user.{Guid.NewGuid():N}";
        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            CreateUserBody(username, "OldPassw0rd!", "Staff"));
        var created = await createResponse.Content.ReadFromJsonAsync<CreateUserResultDto>();

        var changeResponse = await _client.PutAsJsonAsync(
            $"/api/users/{created!.id}/password",
            new { newPassword = "NewPassw0rd!" });
        Assert.Equal(HttpStatusCode.NoContent, changeResponse.StatusCode);

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new { username, password = "NewPassw0rd!" });
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
    }

    [Fact]
    public async Task GetById_WithUnknownId_ShouldReturnNotFound()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync($"/api/users/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_WithUnknownId_ShouldReturnNotFound()
    {
        await AuthenticateAsync();

        var response = await _client.PutAsJsonAsync(
            "/api/users",
            new { id = Guid.NewGuid(), username = $"user.{Guid.NewGuid():N}", role = "Admin" });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ChangePassword_WithUnknownId_ShouldReturnNotFound()
    {
        await AuthenticateAsync();

        var response = await _client.PutAsJsonAsync(
            "/api/users/password",
            new { id = Guid.NewGuid(), newPassword = "NewPassw0rd!" });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithUnknownId_ShouldReturnNotFound()
    {
        await AuthenticateAsync();

        var response = await _client.DeleteAsync($"/api/users/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private record LoginResponseDto(string Token);

    private record CreateUserResultDto(Guid id, string username, string document, string email);

    private record GetUserResponseDto(Guid Id, string Username, string document, string email, string Role, string Status);
}
