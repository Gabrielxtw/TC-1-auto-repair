using TC1.RepairShop.Domain.Entities.Users;

namespace TC1.RepairShop.Application.Clients.UseCases;

public class GetUserUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User?> ExecuteAsync(Guid id) => _userRepository.GetByIdAsync(id);
}
