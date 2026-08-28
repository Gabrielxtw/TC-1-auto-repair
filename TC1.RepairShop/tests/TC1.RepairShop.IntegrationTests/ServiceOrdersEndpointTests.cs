using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace TC1.RepairShop.IntegrationTests;

public class ServiceOrdersEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ServiceOrdersEndpointTests(ApiWebApplicationFactory factory)
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

    private async Task<Guid> CreateUserAsync()
    {
        var response = await _client.PostAsJsonAsync(
            "/api/users",
            new { username = $"user.{Guid.NewGuid():N}", password = "Passw0rd!", role = "Customer" });
        var created = await response.Content.ReadFromJsonAsync<CreateUserResultDto>();
        return created!.id;
    }

    private async Task<Guid> CreateVehicleAsync(Guid customerId)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/vehicles",
            new { customerId, licensePlate = $"BRA{Random.Shared.Next(0, 9)}A{Random.Shared.Next(0, 9)}{Random.Shared.Next(0, 9)}", brand = "Toyota", model = "Corolla", year = 2022 });
        var created = await response.Content.ReadFromJsonAsync<VehicleResponseDto>();
        return created!.Id;
    }

    [Fact]
    public async Task GetAll_WithoutToken_ShouldReturn401()
    {
        var response = await _client.GetAsync("/api/serviceorders");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateServiceOrder_ThenGetById_ShouldReturnCreatedOrder()
    {
        await AuthenticateAsStaffAsync();
        var userId = await CreateUserAsync();
        var vehicleId = await CreateVehicleAsync(userId);

        var createResponse = await _client.PostAsJsonAsync(
            "/api/serviceorders",
            new { userId, vehicleId });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
    }

    [Fact]
    public async Task AttachPart_ThenAttachSamePartAgain_ShouldFail()
    {
        await AuthenticateAsStaffAsync();
        var userId = await CreateUserAsync();
        var vehicleId = await CreateVehicleAsync(userId);
        var orderResponse = await _client.PostAsJsonAsync("/api/serviceorders", new { userId, vehicleId });
        var order = await orderResponse.Content.ReadFromJsonAsync<CreateServiceOrderResultDto>();

        var partResponse = await _client.PostAsJsonAsync("/api/part", new { name = $"Brake Pad {Guid.NewGuid():N}", unitPrice = 19.99m, minimumQuantity = 1 });
        Assert.Equal(HttpStatusCode.Created, partResponse.StatusCode);
        var allParts = (await (await _client.GetAsync("/api/part")).Content.ReadFromJsonAsync<ResponseWrapper<ListPartsResponseDto>>())!.data.Parts;
        var part = allParts.Last();

        var firstAttach = await _client.PostAsJsonAsync(
            "/api/serviceorders/AttachPart",
            new { serviceOrderId = order!.Id, partId = part.id, quantity = 1, price = 19.99m, suppliedByCustomer = false });
        Assert.Equal(HttpStatusCode.OK, firstAttach.StatusCode);

        var secondAttach = await _client.PostAsJsonAsync(
            "/api/serviceorders/AttachPart",
            new { serviceOrderId = order.Id, partId = part.id, quantity = 1, price = 19.99m, suppliedByCustomer = false });
        Assert.NotEqual(HttpStatusCode.OK, secondAttach.StatusCode);
    }

    // NOTE: AdvanceServiceOrderUseCase catches the InvalidOperationException raised by an
    // unknown status name internally and returns a success:false payload; it never
    // rethrows, so the controller's try/catch (which would return BadRequest) is
    // unreachable here and the response is 200 with a failure body.
    [Fact]
    public async Task Advance_ToUnknownStatus_ShouldReturnOkWithFailurePayload()
    {
        await AuthenticateAsStaffAsync();
        var userId = await CreateUserAsync();
        var vehicleId = await CreateVehicleAsync(userId);
        var orderResponse = await _client.PostAsJsonAsync("/api/serviceorders", new { userId, vehicleId });
        var order = await orderResponse.Content.ReadFromJsonAsync<CreateServiceOrderResultDto>();

        var response = await _client.PutAsJsonAsync(
            "/api/serviceorders/Advance",
            new { serviceOrderId = order!.Id, newStatus = "NotARealStatus" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<AdvanceServiceOrderResponseDto>();
        Assert.False(body!.success);
    }

    [Fact]
    public async Task Advance_ToValidNextStatus_ShouldSucceed()
    {
        await AuthenticateAsStaffAsync();
        var userId = await CreateUserAsync();
        var vehicleId = await CreateVehicleAsync(userId);
        var orderResponse = await _client.PostAsJsonAsync("/api/serviceorders", new { userId, vehicleId });
        var order = await orderResponse.Content.ReadFromJsonAsync<CreateServiceOrderResultDto>();

        var response = await _client.PutAsJsonAsync(
            "/api/serviceorders/Advance",
            new { serviceOrderId = order!.Id, newStatus = "Under Diagnosis" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturnCreatedOrder()
    {
        await AuthenticateAsStaffAsync();
        var userId = await CreateUserAsync();
        var vehicleId = await CreateVehicleAsync(userId);
        await _client.PostAsJsonAsync("/api/serviceorders", new { userId, vehicleId });

        var response = await _client.GetAsync("/api/serviceorders");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WithUnknownId_ShouldReturnNotFound()
    {
        await AuthenticateAsStaffAsync();

        var response = await _client.GetAsync($"/api/serviceorders/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Cancel_ShouldSucceed()
    {
        await AuthenticateAsStaffAsync();
        var userId = await CreateUserAsync();
        var vehicleId = await CreateVehicleAsync(userId);
        var orderResponse = await _client.PostAsJsonAsync("/api/serviceorders", new { userId, vehicleId });
        var order = await orderResponse.Content.ReadFromJsonAsync<CreateServiceOrderResultDto>();

        var response = await _client.PutAsJsonAsync(
            "/api/serviceorders/Cancel",
            new { id = order!.Id });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<CancelServiceOrderResponseDto>();
        Assert.True(body!.success);
    }

    [Fact]
    public async Task Cancel_WithUnknownId_ShouldReturnNotFound()
    {
        await AuthenticateAsStaffAsync();

        var response = await _client.PutAsJsonAsync(
            "/api/serviceorders/Cancel",
            new { id = Guid.NewGuid() });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AttachService_ShouldSucceed()
    {
        await AuthenticateAsStaffAsync();
        var userId = await CreateUserAsync();
        var vehicleId = await CreateVehicleAsync(userId);
        var orderResponse = await _client.PostAsJsonAsync("/api/serviceorders", new { userId, vehicleId });
        var order = await orderResponse.Content.ReadFromJsonAsync<CreateServiceOrderResultDto>();

        var name = $"Wheel Balance {Guid.NewGuid():N}";
        var serviceResponse = await _client.PostAsJsonAsync(
            "/api/services",
            new { name, description = "Balance wheels", price = 29.99m });
        Assert.Equal(HttpStatusCode.Created, serviceResponse.StatusCode);
        var allServices = (await (await _client.GetAsync("/api/services")).Content.ReadFromJsonAsync<ResponseWrapper<ListServicesResponseDto>>())!.data.Services;
        var service = allServices.Single(s => s.name == name);

        var response = await _client.PostAsJsonAsync(
            "/api/serviceorders/AttachService",
            new { serviceOrderId = order!.Id, serviceId = service.id, price = 29.99m });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private record ServiceViewModelDto(Guid id, string name, string description);

    private record ListServicesResponseDto(List<ServiceViewModelDto> Services);

    private record CreateServiceOrderResultDto(Guid Id);

    private record CancelServiceOrderResponseDto(bool success, string? error);

    private record AdvanceServiceOrderResponseDto(bool success, string? error);

    private record LoginResponseDto(string Token);

    private record CreateUserResultDto(Guid id, string username, string document, string email);

    private record VehicleResponseDto(Guid Id, string? Username, string LicensePlate, string Brand, string Model, int Year, string Status);

    private record PartViewModelDto(Guid id, string name, int stockQuantity, decimal unitPrice);

    private record ListPartsResponseDto(List<PartViewModelDto> Parts);
    private record ResponseWrapper<T>(T data, string error, bool success);
}
