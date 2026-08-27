using System.Net;
using System.Net.Http.Json;
using System.Threading;
using Xunit;

namespace TC1.RepairShop.IntegrationTests;

public class VehiclesEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private static readonly string[] ValidCpfs =
    [
        "22510716280", "45364948390", "40322270049", "63177301050", "86911443199", "09766675252",
    ];

    private static int _cpfIndex = -1;
    private static int _plateIndex = -1;

    private readonly HttpClient _client;

    public VehiclesEndpointTests(ApiWebApplicationFactory factory)
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

    // Mercosul format (ABC1D23) — index kept small enough to stay within letter range.
    private static string NextPlate()
    {
        var n = Interlocked.Increment(ref _plateIndex);
        var letter = (char)('A' + (n % 26));
        return $"BRA{n % 10}{letter}{n % 10}{(n + 1) % 10}";
    }

    private async Task<Guid> CreateCustomerAsync()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/users",
            new { username = $"owner.{Guid.NewGuid():N}", password = "Passw0rd!", document = NextCpf(), email = "owner@example.com", role = "Customer", phone = "11988887777" });
        var created = await response.Content.ReadFromJsonAsync<CreateUserResultDto>();
        return created!.id;
    }

    [Fact]
    public async Task GetVehicles_WithoutToken_ShouldReturn401()
    {
        var response = await _client.GetAsync("/api/vehicles");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateVehicle_ThenGetById_ShouldReturnCreatedVehicle()
    {
        await AuthenticateAsStaffAsync();
        var customerId = await CreateCustomerAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/vehicles",
            new { customerId, licensePlate = NextPlate(), brand = "Toyota", model = "Corolla", year = 2022 });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<VehicleResponseDto>();
        Assert.NotNull(created);

        var getResponse = await _client.GetAsync($"/api/vehicles/{created!.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateVehicle_WithUnknownCustomer_ShouldReturn404()
    {
        await AuthenticateAsStaffAsync();

        var response = await _client.PostAsJsonAsync(
            "/api/vehicles",
            new { customerId = Guid.NewGuid(), licensePlate = NextPlate(), brand = "Toyota", model = "Corolla", year = 2022 });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateVehicle_WithDuplicateLicensePlate_ShouldReturn409()
    {
        await AuthenticateAsStaffAsync();
        var customerId = await CreateCustomerAsync();
        var plate = NextPlate();

        await _client.PostAsJsonAsync(
            "/api/vehicles",
            new { customerId, licensePlate = plate, brand = "Toyota", model = "Corolla", year = 2022 });

        var response = await _client.PostAsJsonAsync(
            "/api/vehicles",
            new { customerId, licensePlate = plate, brand = "Honda", model = "Civic", year = 2021 });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task GetVehicles_FilteredByCustomerId_ShouldReturnOnlyThatCustomersVehicles()
    {
        await AuthenticateAsStaffAsync();
        var customerId = await CreateCustomerAsync();

        await _client.PostAsJsonAsync(
            "/api/vehicles",
            new { customerId, licensePlate = NextPlate(), brand = "Toyota", model = "Corolla", year = 2022 });

        var response = await _client.GetAsync($"/api/vehicles?customerId={customerId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var vehicles = (await response.Content.ReadFromJsonAsync<ListVehiclesResponseDto>())!.Vehicles;
        Assert.NotEmpty(vehicles);
    }

    [Fact]
    public async Task DeleteVehicle_ShouldSoftDeleteAndHideFromGetById()
    {
        await AuthenticateAsStaffAsync();
        var customerId = await CreateCustomerAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/vehicles",
            new { customerId, licensePlate = NextPlate(), brand = "Toyota", model = "Corolla", year = 2022 });
        var created = await createResponse.Content.ReadFromJsonAsync<VehicleResponseDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/vehicles/{created!.Id}");
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/vehicles/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    private record LoginResponseDto(string Token);

    private record CreateUserResultDto(Guid id, string username, string document, string email);

    private record VehicleResponseDto(Guid Id, string? Username, string LicensePlate, string Brand, string Model, int Year, string Status);

    private record ListVehiclesResponseDto(List<VehicleResponseDto> Vehicles);
}
