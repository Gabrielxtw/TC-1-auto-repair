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

        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

        var allServices = await (await _client.GetAsync("/api/services")).Content.ReadFromJsonAsync<List<ServiceViewModelDto>>();
        var created = allServices!.Single(s => s.name == name);

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
        var allServices = await (await _client.GetAsync("/api/services")).Content.ReadFromJsonAsync<List<ServiceViewModelDto>>();
        var created = allServices!.Single(s => s.name == name);

        var firstDeactivate = await _client.PutAsJsonAsync("/api/services/Deactive", new { id = created.id });
        Assert.Equal(HttpStatusCode.OK, firstDeactivate.StatusCode);

        var secondDeactivate = await _client.PutAsJsonAsync("/api/services/Deactive", new { id = created.id });
        Assert.NotEqual(HttpStatusCode.OK, secondDeactivate.StatusCode);
    }

    private record LoginResponseDto(string Token);

    private record ServiceViewModelDto(Guid id, string name, string description);
}
