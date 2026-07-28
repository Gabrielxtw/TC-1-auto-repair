using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202607261003)]
public class CreateServiceOrders : Migration
{
    public override void Up()
    {
        Create.Table("ServiceOrders")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ServiceOrders")
            .WithColumn("CustomerId").AsGuid().NotNullable()
                .ForeignKey("FK_ServiceOrders_Customers", "Customers", "Id")
            .WithColumn("VehicleId").AsGuid().NotNullable()
                .ForeignKey("FK_ServiceOrders_Vehicles", "Vehicles", "Id")
            .WithColumn("Status").AsString(30).NotNullable()
            .WithColumn("OpenedAt").AsDateTime2().NotNullable()
            .WithColumn("CompletedAt").AsDateTime2().Nullable()
            .WithColumn("QuoteId").AsGuid().Nullable();
    }

    public override void Down()
    {
        Delete.Table("ServiceOrders");
    }
}
