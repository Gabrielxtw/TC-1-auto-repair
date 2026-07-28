using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace TC1.RepairShop.IntegrationTests;

public class AuthEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnJwtToken()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new { username = "admin", password = "Admin@123" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        Assert.False(string.IsNullOrWhiteSpace(body?.Token));
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturn401()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new { username = "admin", password = "wrong-password" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCustomers_WithoutToken_ShouldReturn401()
    {
        var response = await _client.GetAsync("/api/customers");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCustomers_WithValidToken_ShouldReturn200()
    {
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new { username = "admin", password = "Admin@123" });
        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", login!.Token);

        var response = await _client.GetAsync("/api/customers");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private record LoginResponseDto(string Token);
}
