using TC1.RepairShop.Application.Registration;

namespace TC1.RepairShop.Application.Auth.UseCases;

public record AuthenticateCustomerRequest(string NationalId, string Password);

public record AuthenticateCustomerResult(bool Success, string? Token);

public class AuthenticateCustomerUseCase(ICostumerRepository _customerRepository, ITokenService _tokenService)
{
    public async Task<AuthenticateCustomerResult> ExecuteAsync(AuthenticateCustomerRequest request)
    {
        var customer = await _customerRepository.GetByNationalIdAsync(request.NationalId);

        if (customer is null || !customer.VerifyPassword(request.Password))
        {
            return new AuthenticateCustomerResult(false, null);
        }

        var token = _tokenService.GenerateCustomerToken(customer);
        return new AuthenticateCustomerResult(true, token);
    }
}
