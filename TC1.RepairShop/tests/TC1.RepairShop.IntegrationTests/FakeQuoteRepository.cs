using System.Collections.Concurrent;
using TC1.RepairShop.Domain.Entities.Quotes;
using TC1.RepairShop.Domain.Interfaces;

namespace TC1.RepairShop.IntegrationTests;

public class FakeQuoteRepository : IQuoteRepository
{
    public static readonly ConcurrentDictionary<Guid, Quote> Quotes = new();

    public Task<IEnumerable<Quote>> GetAllAsync() =>
        Task.FromResult(Quotes.Values.AsEnumerable());

    public Task<Quote?> GetByIdAsync(Guid id)
    {
        Quotes.TryGetValue(id, out var quote);
        return Task.FromResult(quote);
    }

    public Task AddAsync(Quote quote)
    {
        Quotes[quote.Id] = quote;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Quote quote)
    {
        Quotes[quote.Id] = quote;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        Quotes.TryRemove(id, out _);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync() => Task.CompletedTask;
    public Task Add(Quote quote) => Task.CompletedTask;
    public Task Update(Quote quote) => Task.CompletedTask;


    public Task<bool> ExistsAsync(Guid id) => Task.FromResult(Quotes.ContainsKey(id));
}
