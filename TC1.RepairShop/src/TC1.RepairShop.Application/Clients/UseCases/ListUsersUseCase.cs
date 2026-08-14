using TC1.RepairShop.Application.Clients;
using TC1.RepairShop.Domain.Entities.Clients;

namespace TC1.RepairShop.Application.Clients.UseCases;

public class ListUsersUseCase
{
    private readonly IUserRepository _userRepository;

    public ListUsersUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IEnumerable<User>> ExecuteAsync() => _userRepository.GetAllAsync();
}
