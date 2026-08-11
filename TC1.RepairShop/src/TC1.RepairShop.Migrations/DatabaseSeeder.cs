using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TC1.RepairShop.Domain.Clients;
using TC1.RepairShop.Domain.Common;
using TC1.RepairShop.Domain.Quotes;
using TC1.RepairShop.Domain.Registration;
using TC1.RepairShop.Domain.ServiceOrders;
using TC1.RepairShop.Domain.Services;

namespace TC1.RepairShop.Migrations;

public static class DatabaseSeeder
{
    private const string SeedUsername = "admin";
    private const string SeedCustomerNationalId = "01098843371";

    public static void SeedAdminUser(IConfiguration configuration, string connectionString)
    {
        var seedPassword = configuration["SeedAdmin:Password"]
            ?? Environment.GetEnvironmentVariable("SEED_ADMIN_PASSWORD")
            ?? throw new InvalidOperationException(
                "Set the SEED_ADMIN_PASSWORD environment variable (or SeedAdmin:Password) with the initial admin user's password.");

        using var connection = new SqlConnection(connectionString);

        var alreadyExists = connection.ExecuteScalar<int>(
            "SELECT COUNT(1) FROM Users WHERE Username = @Username", new { Username = SeedUsername }) > 0;
        if (alreadyExists)
        {
            Console.WriteLine("Seed admin user already exists, skipping.");
            return;
        }

        var admin = User.Create(SeedUsername, seedPassword, Role.Admin);

        connection.Execute(
            """
            INSERT INTO Users (Id, Username, PasswordHash, Role, Status)
            VALUES (@Id, @Username, @PasswordHash, @Role, @Status)
            """,
            new { admin.Id, admin.Username, admin.PasswordHash, Role = admin.Role.ToString(), Status = admin.Status.ToString() });

        Console.WriteLine("Seed admin user created.");
    }

    public static void SeedSampleCustomer(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        var alreadyExists = connection.ExecuteScalar<int>(
            "SELECT COUNT(1) FROM Customers WHERE NationalId = @NationalId", new { NationalId = SeedCustomerNationalId }) > 0;
        if (alreadyExists)
        {
            Console.WriteLine("Seed sample customer already exists, skipping.");
            return;
        }

        var customer = Customer.Create("Carlos Eduardo Santos", SeedCustomerNationalId, "11987654321", "carlos.santos@example.com.br");
        InsertCustomer(connection, customer);

        var vehicle = Vehicle.Create(customer.Id, "RJK4E12", "Volkswagen", "Gol", 2019);
        InsertVehicle(connection, vehicle);

        var oilChangeService = Service.Create("Troca de óleo e filtro", "Substituição de óleo do motor e filtro de óleo.");
        var alignmentService = Service.Create("Alinhamento e balanceamento", "Alinhamento de direção e balanceamento das quatro rodas.");
        var brakeService = Service.Create("Revisão do sistema de freios", "Inspeção e troca de pastilhas e discos de freio, se necessário.");
        InsertService(connection, oilChangeService);
        InsertService(connection, alignmentService);
        InsertService(connection, brakeService);

        var completedOrder = ServiceOrder.Create(customer.Id, vehicle.Id);
        completedOrder.AdvanceTo(ServiceOrderStatus.Delivered);

        var completedQuote = Quote.Create(completedOrder.Id, totalAmount: 320.00m, discount: 10);
        completedQuote.Approve();
        completedOrder.AttachQuote(completedQuote.Id);

        InsertServiceOrder(connection, completedOrder);
        InsertQuote(connection, completedQuote, completedQuote.QuoteStatusValue);
        InsertServiceOrderService(connection, completedOrder.Id, oilChangeService.Id);
        InsertServiceOrderService(connection, completedOrder.Id, alignmentService.Id);

        var pendingOrder = ServiceOrder.Create(customer.Id, vehicle.Id);
        pendingOrder.AdvanceTo(ServiceOrderStatus.AwaitingApproval);

        var pendingQuote = Quote.Create(pendingOrder.Id, totalAmount: 480.00m, discount: 0);
        pendingOrder.AttachQuote(pendingQuote.Id);

        InsertServiceOrder(connection, pendingOrder);
        InsertQuote(connection, pendingQuote, QuoteStatus.SentToCustomer);
        InsertServiceOrderService(connection, pendingOrder.Id, brakeService.Id);

        Console.WriteLine("Seed sample customer, vehicle, services, and quotes created.");
    }

    private static void InsertCustomer(SqlConnection connection, Customer customer) =>
        connection.Execute(
            """
            INSERT INTO Customers (Id, Name, NationalId, Phone, Email, PasswordHash, RegisteredAt, Status)
            VALUES (@Id, @Name, @NationalId, @Phone, @Email, @PasswordHash, @RegisteredAt, @Status)
            """,
            new
            {
                customer.Id,
                customer.Name,
                customer.NationalId,
                customer.Phone,
                customer.Email,
                customer.PasswordHash,
                customer.RegisteredAt,
                Status = customer.Status.ToString(),
            });

    private static void InsertVehicle(SqlConnection connection, Vehicle vehicle) =>
        connection.Execute(
            """
            INSERT INTO Vehicles (Id, CustomerId, LicensePlate, Brand, Model, Year, Status)
            VALUES (@Id, @CustomerId, @LicensePlate, @Brand, @Model, @Year, @Status)
            """,
            new
            {
                vehicle.Id,
                vehicle.CustomerId,
                vehicle.LicensePlate,
                vehicle.Brand,
                vehicle.Model,
                vehicle.Year,
                Status = vehicle.Status.ToString(),
            });

    private static void InsertService(SqlConnection connection, Service service) =>
        connection.Execute(
            """
            INSERT INTO Services (Id, Name, Description, Status)
            VALUES (@Id, @Name, @Description, @Status)
            """,
            new { service.Id, service.Name, service.Description, Status = service.Status.ToString() });

    private static void InsertServiceOrder(SqlConnection connection, ServiceOrder order) =>
        connection.Execute(
            """
            INSERT INTO ServiceOrders (Id, CustomerId, VehicleId, OrderStatusValue, OpenedAt, CompletedAt, QuoteId, Status)
            VALUES (@Id, @CustomerId, @VehicleId, @OrderStatusValue, @OpenedAt, @CompletedAt, @QuoteId, @Status)
            """,
            new
            {
                order.Id,
                order.CustomerId,
                order.VehicleId,
                OrderStatusValue = order.OrderStatusValue.ToString(),
                order.OpenedAt,
                order.CompletedAt,
                order.QuoteId,
                Status = order.Status.ToString(),
            });

    private static void InsertQuote(SqlConnection connection, Quote quote, QuoteStatus status) =>
        connection.Execute(
            """
            INSERT INTO Quotes (Id, ServiceOrderId, TotalAmount, Discount, FinalPrice, QuoteStatusValue, RejectionCount, Status)
            VALUES (@Id, @ServiceOrderId, @TotalAmount, @Discount, @FinalPrice, @QuoteStatusValue, @RejectionCount, @Status)
            """,
            new
            {
                quote.Id,
                quote.ServiceOrderId,
                quote.TotalAmount,
                quote.Discount,
                quote.FinalPrice,
                QuoteStatusValue = status.ToString(),
                quote.RejectionCount,
                Status = quote.Status.ToString(),
            });

    private static void InsertServiceOrderService(SqlConnection connection, Guid serviceOrderId, Guid serviceId) =>
        connection.Execute(
            """
            INSERT INTO ServiceOrderServices (Id, ServiceOrderId, ServiceId)
            VALUES (@Id, @ServiceOrderId, @ServiceId)
            """,
            new { Id = Guid.NewGuid(), ServiceOrderId = serviceOrderId, ServiceId = serviceId });
}
