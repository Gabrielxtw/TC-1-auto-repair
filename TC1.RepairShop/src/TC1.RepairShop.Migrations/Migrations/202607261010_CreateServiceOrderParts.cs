using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202607261010)]
public class CreateServiceOrderParts : Migration
{
    public override void Up()
    {
        Create.Table("ServiceOrderParts")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ServiceOrderParts")
            .WithColumn("ServiceOrderId").AsGuid().NotNullable()
                .ForeignKey("FK_ServiceOrderParts_ServiceOrders", "ServiceOrders", "Id")
            .WithColumn("PartId").AsGuid().NotNullable()
                .ForeignKey("FK_ServiceOrderParts_Parts", "Parts", "Id")
            .WithColumn("Quantity").AsInt32().NotNullable().WithDefaultValue(1)
            .WithColumn("UnitPrice").AsDecimal(10, 2).NotNullable()
            .WithColumn("SuppliedByCustomer").AsBoolean().NotNullable().WithDefaultValue(false);
    }

    public override void Down()
    {
        Delete.Table("ServiceOrderParts");
    }
}
