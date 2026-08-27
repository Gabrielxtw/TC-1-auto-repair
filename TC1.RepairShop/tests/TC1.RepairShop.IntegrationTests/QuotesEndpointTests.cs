using System.Net;
using System.Net.Http.Json;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Enums;
using Xunit;

namespace TC1.RepairShop.IntegrationTests;

public class QuotesEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public QuotesEndpointTests(ApiWebApplicationFactory factory)
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
    public async Task GetMyQuotes_WithoutToken_ShouldReturn401()
    {
        var response = await _client.GetAsync("/api/quotes");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetMyQuotes_DoesNotFilterByCaller_ReturnsAllQuotes()
    {
        await AuthenticateAsync();
        var quote = Quote.Create(Guid.NewGuid(), 500m);
        FakeQuoteRepository.Quotes[quote.Id] = quote;

        var response = await _client.GetAsync("/api/quotes");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<BaseResponseDto>();
        Assert.True(body!.success);
        // Documents current behavior: GetMyQuotes has a "//TODO get records from path"
        // comment and does not scope results to the authenticated caller.
        Assert.Contains(body.data!.Quotes, q => q.Id == quote.Id);
    }

    [Fact]
    public async Task ApproveQuote_ShouldSucceed_WhenQuoteExists()
    {
        await AuthenticateAsync();
        var quote = Quote.Create(Guid.NewGuid(), 500m);
        FakeQuoteRepository.Quotes[quote.Id] = quote;

        var response = await _client.PutAsync($"/api/quotes/Approve/{quote.Id}", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(QuoteStatus.Approved, quote.QuoteStatusValue);
    }

    [Fact]
    public async Task RejectQuote_ShouldSucceed_WhenQuoteExists()
    {
        await AuthenticateAsync();
        var quote = Quote.Create(Guid.NewGuid(), 500m);
        FakeQuoteRepository.Quotes[quote.Id] = quote;

        var response = await _client.PutAsync($"/api/quotes/Reject/{quote.Id}", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(QuoteStatus.UnderReview, quote.QuoteStatusValue);
    }

    [Fact]
    public async Task ApproveQuote_ShouldReturnOkWithFailurePayload_WhenQuoteDoesNotExist()
    {
        await AuthenticateAsync();

        var response = await _client.PutAsync($"/api/quotes/Approve/{Guid.NewGuid()}", null);

        // NOTE: QuotesController always returns Ok(...) regardless of use case
        // success/failure (it never inspects result.success), so a missing quote
        // still yields 200 with a failure payload embedded in the body.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<BaseResponseDto>();
        Assert.False(body!.success);
    }

    private record LoginResponseDto(string Token);

    private record QuoteDto(Guid Id);

    private record QuoteListDataDto(List<QuoteDto> Quotes);

    private record BaseResponseDto(QuoteListDataDto? data, bool success, string? error);
}
