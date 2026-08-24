using System.Net;
using System.Net.Http.Json;
using System.Threading;
using Xunit;

namespace TC1.RepairShop.IntegrationTests;

public class CustomersEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    // Valid, distinct CPFs (correct check digits) reserved for this test class, drawn one per test.
    private static readonly string[] ValidCpfs =
    [
        "92351101812", "68135274793", "44908348618", "19782311561", "85565484405",
        "61349557366", "36122620254", "02895784183", "88679757063", "53452820998",
    ];

    private static int _cpfIndex = -1;

    private readonly HttpClient _client;

    public CustomersEndpointTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task AuthenticateAsStaffAsync()
    {
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new { username = ApiWebApplicationFactory.AdminUsername, password = ApiWebApplicationFactory.AdminPassword });
        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", login!.Token);
    }

    private static string NextCpf() => ValidCpfs[Interlocked.Increment(ref _cpfIndex) % ValidCpfs.Length];

    [Fact]
    public async Task GetCustomers_WithoutToken_ShouldReturn401()
    {
        var response = await _client.GetAsync("/api/users");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCustomers_WithStaffToken_ShouldReturn200()
    {
        await AuthenticateAsStaffAsync();

        var response = await _client.GetAsync("/api/users");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateCustomer_ThenGetById_ShouldReturnCreatedCustomer()
    {
        await AuthenticateAsStaffAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            new { username = $"customer.{Guid.NewGuid():N}", password = "Passw0rd!", document = NextCpf(), email = "john@example.com", role = "Customer", phone = "11988887777" });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<CreateUserResultDto>();
        Assert.NotNull(created);

        var getResponse = await _client.GetAsync($"/api/users/{created!.id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateCustomer_WithDuplicateUsername_ShouldReturn409()
    {
        await AuthenticateAsStaffAsync();

        var username = $"customer.{Guid.NewGuid():N}";
        await _client.PostAsJsonAsync(
            "/api/users",
            new { username, password = "Passw0rd!", document = NextCpf(), email = "john@example.com", role = "Customer", phone = "11988887777" });

        var response = await _client.PostAsJsonAsync(
            "/api/users",
            new { username, password = "Passw0rd!", document = NextCpf(), email = "other@example.com", role = "Customer", phone = "11977776666" });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldChangeUsername()
    {
        await AuthenticateAsStaffAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            new { username = $"customer.{Guid.NewGuid():N}", password = "Passw0rd!", document = NextCpf(), email = "john@example.com", role = "Customer", phone = "11988887777" });
        var created = await createResponse.Content.ReadFromJsonAsync<CreateUserResultDto>();

        var newUsername = $"customer.{Guid.NewGuid():N}";
        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/users/{created!.id}",
            new { username = newUsername, role = "Customer" });

        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/users/{created.id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<GetUserResponseDto>();
        Assert.Equal(newUsername, updated!.Username);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldSoftDeleteAndHideFromGetAndList()
    {
        await AuthenticateAsStaffAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/users",
            new { username = $"customer.{Guid.NewGuid():N}", password = "Passw0rd!", document = NextCpf(), email = "john@example.com", role = "Customer", phone = "11988887777" });
        var created = await createResponse.Content.ReadFromJsonAsync<CreateUserResultDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/users/{created!.id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/users/{created.id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    private record LoginResponseDto(string Token);

    private record CreateUserResultDto(Guid id, string username, string document, string email);

    private record GetUserResponseDto(Guid Id, string Username, string document, string email, string Role, string Status);
}
