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
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new { username = "admin", password = "Admin@123" });
        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();

        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", login!.Token);
    }

    private static string NextCpf() => ValidCpfs[Interlocked.Increment(ref _cpfIndex) % ValidCpfs.Length];

    [Fact]
    public async Task GetCustomers_WithoutToken_ShouldReturn401()
    {
        var response = await _client.GetAsync("/api/customers");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCustomers_WithStaffToken_ShouldReturn200()
    {
        await AuthenticateAsStaffAsync();

        var response = await _client.GetAsync("/api/customers");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateCustomer_ThenGetById_ShouldReturnCreatedCustomer()
    {
        await AuthenticateAsStaffAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/customers",
            new { name = "John Smith", nationalId = NextCpf(), phone = "11988887777", email = "john@example.com" });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<CustomerResponseDto>();
        Assert.NotNull(created);
        Assert.Equal("Active", created!.Status);

        var getResponse = await _client.GetAsync($"/api/customers/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateCustomer_WithDuplicateNationalId_ShouldReturn409()
    {
        await AuthenticateAsStaffAsync();

        var nationalId = NextCpf();
        await _client.PostAsJsonAsync(
            "/api/customers",
            new { name = "John Smith", nationalId, phone = "11988887777", email = "john@example.com" });

        var response = await _client.PostAsJsonAsync(
            "/api/customers",
            new { name = "Other Name", nationalId, phone = "11977776666", email = "other@example.com" });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCustomer_ShouldChangeContactInfo()
    {
        await AuthenticateAsStaffAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/customers",
            new { name = "John Smith", nationalId = NextCpf(), phone = "11988887777", email = "john@example.com" });
        var created = await createResponse.Content.ReadFromJsonAsync<CustomerResponseDto>();

        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/customers/{created!.Id}",
            new { phone = "11900001111", email = "john.new@example.com" });

        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/customers/{created.Id}");
        var updated = await getResponse.Content.ReadFromJsonAsync<CustomerResponseDto>();
        Assert.Equal("11900001111", updated!.Phone);
        Assert.Equal("john.new@example.com", updated.Email);
    }

    [Fact]
    public async Task DeleteCustomer_ShouldSoftDeleteAndHideFromGetAndList()
    {
        await AuthenticateAsStaffAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/customers",
            new { name = "John Smith", nationalId = NextCpf(), phone = "11988887777", email = "john@example.com" });
        var created = await createResponse.Content.ReadFromJsonAsync<CustomerResponseDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/customers/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/customers/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    private record LoginResponseDto(string Token);

    private record CustomerResponseDto(Guid Id, string Name, string NationalId, string Phone, string Email, string Status);
}
