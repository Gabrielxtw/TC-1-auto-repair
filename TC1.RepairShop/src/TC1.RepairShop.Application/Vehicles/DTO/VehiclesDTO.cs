using TC1.RepairShop.Domain.Entities.Vehicles;

namespace TC1.RepairShop.Application.Vehicles.UseCases
{
    public record VehicleResponse(Guid Id, string Username, string LicensePlate, string Brand, string Model, int Year, string Status);
    public record ListVehiclesResponse(IEnumerable<VehicleResponse> Vehicles);

    public static class VehiclesDTO
    {
        public static VehicleResponse ToVehicleResponse(Vehicle vehicle)
        {
            return new VehicleResponse(vehicle.Id, vehicle.User.Username, vehicle.LicensePlate.Value, vehicle.Brand, vehicle.Model, vehicle.Year, vehicle.Status.ToString());
        }

        public static ListVehiclesResponse ToListVehiclesResponse(IEnumerable<Vehicle> vehicles)
        {
            var responses = vehicles.Select(v => ToVehicleResponse(v)).ToList();
            return new ListVehiclesResponse(responses);
        }
    }
}