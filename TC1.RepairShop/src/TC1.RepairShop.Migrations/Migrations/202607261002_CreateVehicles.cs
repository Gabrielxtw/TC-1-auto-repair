using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202607261002)]
public class CreateVehicles : Migration
{
    public override void Up()
    {
        Create.Table("Vehicles")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Vehicles")
            .WithColumn("CustomerId").AsGuid().NotNullable()
                .ForeignKey("FK_Vehicles_Customers", "Customers", "Id")
            .WithColumn("LicensePlate").AsString(7).NotNullable().Unique("UQ_Vehicles_LicensePlate")
            .WithColumn("Brand").AsString(100).NotNullable()
            .WithColumn("Model").AsString(100).NotNullable()
            .WithColumn("Year").AsInt32().NotNullable()
            .WithColumn("Status").AsString(20).NotNullable().WithDefaultValue("Active");
    }

    public override void Down()
    {
        Delete.Table("Vehicles");
    }
}
