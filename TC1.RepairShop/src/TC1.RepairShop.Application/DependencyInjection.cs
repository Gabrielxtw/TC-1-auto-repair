using Microsoft.Extensions.DependencyInjection;
using TC1.RepairShop.Application.Auth;
using TC1.RepairShop.Application.Auth.UseCases;
using TC1.RepairShop.Application.Clients.UseCases;
using TC1.RepairShop.Application.Registration.UseCases;

namespace TC1.RepairShop.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateUserUseCase>();
            services.AddScoped<GetUserUseCase>();
            services.AddScoped<ListUsersUseCase>();
            services.AddScoped<UpdateUserUseCase>();
            services.AddScoped<ChangeUserPasswordUseCase>();
            services.AddScoped<DeleteUserUseCase>();

            services.AddScoped<CreateVehicleUseCase>();
            services.AddScoped<GetVehicleUseCase>();
            services.AddScoped<ListVehiclesUseCase>();
            services.AddScoped<ListVehiclesByCustomerUseCase>();
            services.AddScoped<DeleteVehicleUseCase>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<AuthenticateUserUseCase>();

            return services;
        }
    }
}
