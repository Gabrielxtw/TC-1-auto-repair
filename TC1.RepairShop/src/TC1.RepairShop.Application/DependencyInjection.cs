using Microsoft.Extensions.DependencyInjection;
using TC1.RepairShop.Application.Auth;
using TC1.RepairShop.Application.Auth.UseCases;
using TC1.RepairShop.Application.Users.UseCases;
using TC1.RepairShop.Application.Parts.UseCases;
using TC1.RepairShop.Application.Vehicles.UseCases;
using TC1.RepairShop.Application.Services.UseCases;
using TC1.RepairShop.Application.Quotes.UseCases;
using TC1.RepairShop.Application.ServiceOrders.UseCases;

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

            services.AddScoped<CreateServiceUseCase>();
            services.AddScoped<GetAllServiceUseCase>();
            services.AddScoped<GetServiceByIdUseCase>();
            services.AddScoped<DeactiveServiceUseCase>();
            services.AddScoped<DeleteServiceUseCase>();

            services.AddScoped<CreatePartUseCase>();
            services.AddScoped<GetAllPartUseCase>();
            services.AddScoped<DeletePartUseCase>();
            services.AddScoped<ReceiveStockUseCase>();
            services.AddScoped<ConsumeStockUseCase>();
            services.AddScoped<UpdatePartUseCase>();
            services.AddScoped<GetPartByIdUseCase>();
            services.AddScoped<DeactivatePartUseCase>();

            services.AddScoped<CreateQuoteUseCase>();
            services.AddScoped<ApproveQuoteUseCase>();
            services.AddScoped<RejectQuoteUseCase>();

            services.AddScoped<CreateServiceOrderUseCase>();
            services.AddScoped<AdvanceServiceOrderUseCase>();
            services.AddScoped<CancelServiceOrderUseCase>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<AuthenticateUserUseCase>();

            return services;
        }
    }
}
