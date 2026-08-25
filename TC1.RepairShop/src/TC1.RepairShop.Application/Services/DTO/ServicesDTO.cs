using TC1.RepairShop.Domain.Entities.Services;
using TC1.RepairShop.Domain.Enums;

namespace TC1.RepairShop.Application.Services.UseCases
{
    public record ServiceResponse(Guid Id, string Name, string Description, decimal Price, Status Status);
    public record ListServicesResponse(IEnumerable<ServiceResponse> Services);
    public record CreateServiceRequest(string name, string description, decimal price);
    public record DeactiveServiceRequest(Guid Id);

    public static class ServicesDTO
    {

        public static ServiceResponse ToServiceResponse(Service service)
        {
            return new ServiceResponse(service.Id, service.Name, service.Description, service.Price, service.Status);
        }

        public static ListServicesResponse ToListServicesResponse(IEnumerable<Service> services)
        {
            var serviceResponses = services.Select(s => new ServiceResponse(s.Id, s.Name, s.Description, s.Price, s.Status)).ToList();
            return new ListServicesResponse(serviceResponses);
        }
    }
}
