using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using TC1.RepairShop.Application.Auth;
using TC1.RepairShop.Domain.Auth;
using Xunit;

namespace TC1.RepairShop.UnitTests.Auth;

public class TokenServiceTests
{
    private static TokenService CreateTokenService() =>
        new(Options.Create(new JwtOptions
        {
            Secret = "unit-test-secret-key-with-at-least-32-chars",
            Issuer = "TC1.RepairShop.Tests",
            Audience = "TC1.RepairShop.Tests.Clients",
            ExpirationMinutes = 30,
        }));

    [Fact]
    public void GenerateToken_ShouldGenerateTokenWithExpectedClaims()
    {
        var tokenService = CreateTokenService();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            PasswordHash = "hash",
            Role = "Admin",
        };

        var token = tokenService.GenerateToken(user);

        Assert.False(string.IsNullOrWhiteSpace(token));

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Equal(user.Id.ToString(), jwt.Subject);
        Assert.Contains(jwt.Claims, c => c.Value == "Admin" && c.Type.EndsWith("role", StringComparison.OrdinalIgnoreCase));
        Assert.Equal("TC1.RepairShop.Tests", jwt.Issuer);
        Assert.Contains("TC1.RepairShop.Tests.Clients", jwt.Audiences);
    }
}
