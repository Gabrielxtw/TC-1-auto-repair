using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace TC1.RepairShop.IntegrationTests;

public class PartsEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public PartsEndpointTests(ApiWebApplicationFactory factory)
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
        var response = await _client.GetAsync("/api/part");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreatePart_ThenGetById_ShouldReturnCreatedPart()
    {
        await AuthenticateAsync();

        var createResponse = await _client.PostAsJsonAsync(
            "/api/part",
            new { name = $"Brake Pad {Guid.NewGuid():N}", unitPrice = 19.99m, minimumQuantity = 1 });

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var getAllResponse = await _client.GetAsync("/api/part");
        Assert.Equal(HttpStatusCode.OK, getAllResponse.StatusCode);

        var wrapper = await getAllResponse.Content.ReadFromJsonAsync<ResponseWrapper<ListPartsResponseDto>>();
        var parts = wrapper!.data.Parts;
        Assert.Contains(parts, p => p.stockQuantity == 0);
    }

    [Fact]
    public async Task ReceiveStock_ShouldIncreaseStockQuantity()
    {
        await AuthenticateAsync();

        var name = $"Oil Filter {Guid.NewGuid():N}";
        await _client.PostAsJsonAsync("/api/part", new { name, unitPrice = 9.99m, minimumQuantity = 1 });

        var allParts = (await (await _client.GetAsync("/api/part")).Content.ReadFromJsonAsync<ResponseWrapper<ListPartsResponseDto>>())!.data.Parts;
        var created = allParts.Single(p => p.name == name);

        var receiveResponse = await _client.PutAsJsonAsync("/api/part/ReceiveStock", new { id = created.id, quantity = 10 });
        Assert.Equal(HttpStatusCode.OK, receiveResponse.StatusCode);

        var afterReceive = (await (await _client.GetAsync("/api/part")).Content.ReadFromJsonAsync<ResponseWrapper<ListPartsResponseDto>>())!.data.Parts;
        Assert.Contains(afterReceive, p => p.id == created.id && p.stockQuantity == 10);
    }

    [Fact]
    public async Task CreatePart_WithDuplicateName_ShouldReturnFailure()
    {
        await AuthenticateAsync();

        var name = $"Spark Plug {Guid.NewGuid():N}";
        await _client.PostAsJsonAsync("/api/part", new { name, unitPrice = 4.99m, minimumQuantity = 1 });

        var response = await _client.PostAsJsonAsync("/api/part", new { name, unitPrice = 4.99m, minimumQuantity = 1 });

        Assert.NotEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WithUnknownId_ShouldReturnNotFound()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync($"/api/part/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Deactivate_ThenGetAll_ShouldStillReturnPart()
    {
        await AuthenticateAsync();

        var name = $"Air Filter {Guid.NewGuid():N}";
        await _client.PostAsJsonAsync("/api/part", new { name, unitPrice = 14.99m, minimumQuantity = 1 });
        var allParts = (await (await _client.GetAsync("/api/part")).Content.ReadFromJsonAsync<ResponseWrapper<ListPartsResponseDto>>())!.data.Parts;
        var created = allParts.Single(p => p.name == name);

        var deactivateResponse = await _client.PutAsJsonAsync("/api/part/Deactive", new { id = created.id });

        Assert.Equal(HttpStatusCode.OK, deactivateResponse.StatusCode);
    }

    [Fact]
    public async Task Delete_ThenGetById_ShouldReturnNotFound()
    {
        await AuthenticateAsync();

        var name = $"Timing Belt {Guid.NewGuid():N}";
        await _client.PostAsJsonAsync("/api/part", new { name, unitPrice = 24.99m, minimumQuantity = 1 });
        var allParts = (await (await _client.GetAsync("/api/part")).Content.ReadFromJsonAsync<ResponseWrapper<ListPartsResponseDto>>())!.data.Parts;
        var created = allParts.Single(p => p.name == name);

        var deleteResponse = await _client.DeleteAsync($"/api/part/{created.id}");
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var getResponse = await _client.GetAsync($"/api/part/{created.id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    private record LoginResponseDto(string Token);

    private record PartViewModelDto(Guid id, string name, int stockQuantity, decimal unitPrice);

    private record ListPartsResponseDto(List<PartViewModelDto> Parts);
    private record ResponseWrapper<T>(T data, string error, bool success);
}
