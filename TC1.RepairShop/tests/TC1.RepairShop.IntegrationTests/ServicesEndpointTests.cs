using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace TC1.RepairShop.IntegrationTests;

public class ServicesEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ServicesEndpointTests(ApiWebApplicationFactory factory)
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

    [Fact]
    public async Task GetAll_WithoutToken_ShouldReturn401()
    {
        var response = await _client.GetAsync("/api/services");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateService_ThenGetById_ShouldReturnCreatedService()
    {
        await AuthenticateAsync();

        var name = $"Oil Change {Guid.NewGuid():N}";
        var createResponse = await _client.PostAsJsonAsync(
            "/api/services",
            new { name, description = "Change engine oil", price = 59.99m });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var allServices = (await (await _client.GetAsync("/api/services")).Content.ReadFromJsonAsync<ListServicesResponseDto>())!.Services;
        var created = allServices.Single(s => s.name == name);

        var getResponse = await _client.GetAsync($"/api/services/{created.id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateService_WithDuplicateName_ShouldReturnFailure()
    {
        await AuthenticateAsync();

        var name = $"Tire Rotation {Guid.NewGuid():N}";
        await _client.PostAsJsonAsync("/api/services", new { name, description = "Rotate tires", price = 29.99m });

        var response = await _client.PostAsJsonAsync("/api/services", new { name, description = "Rotate tires", price = 29.99m });

        Assert.NotEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeactivateService_ThenDeactivateAgain_ShouldFail()
    {
        await AuthenticateAsync();

        var name = $"Wheel Alignment {Guid.NewGuid():N}";
        await _client.PostAsJsonAsync("/api/services", new { name, description = "Align wheels", price = 39.99m });
        var allServices = (await (await _client.GetAsync("/api/services")).Content.ReadFromJsonAsync<ListServicesResponseDto>())!.Services;
        var created = allServices.Single(s => s.name == name);

        var firstDeactivate = await _client.PutAsJsonAsync("/api/services/Deactive", new { id = created.id });
        Assert.Equal(HttpStatusCode.OK, firstDeactivate.StatusCode);

        var secondDeactivate = await _client.PutAsJsonAsync("/api/services/Deactive", new { id = created.id });
        Assert.NotEqual(HttpStatusCode.OK, secondDeactivate.StatusCode);
    }

    [Fact]
    public async Task GetById_WithUnknownId_ShouldReturnNotFound()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync($"/api/services/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ThenGetById_ShouldReturnNotFound()
    {
        await AuthenticateAsync();

        var name = $"Brake Fluid Flush {Guid.NewGuid():N}";
        await _client.PostAsJsonAsync("/api/services", new { name, description = "Flush brake fluid", price = 49.99m });
        var allServices = (await (await _client.GetAsync("/api/services")).Content.ReadFromJsonAsync<ListServicesResponseDto>())!.Services;
        var created = allServices.Single(s => s.name == name);

        var deleteResponse = await _client.DeleteAsync($"/api/services/{created.id}");
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/services/{created.id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    private record LoginResponseDto(string Token);

    private record ServiceViewModelDto(Guid id, string name, string description);

    private record ListServicesResponseDto(List<ServiceViewModelDto> Services);
}
