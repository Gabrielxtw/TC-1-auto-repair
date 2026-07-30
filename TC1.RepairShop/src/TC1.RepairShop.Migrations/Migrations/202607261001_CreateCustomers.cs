using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202607261001)]
public class CreateCustomers : Migration
{
    public override void Up()
    {
        Create.Table("Customers")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Customers")
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("NationalId").AsString(14).NotNullable().Unique("UQ_Customers_NationalId")
            .WithColumn("Phone").AsString(20).NotNullable()
            .WithColumn("Email").AsString(200).NotNullable()
            .WithColumn("RegisteredAt").AsDateTime2().NotNullable()
            .WithColumn("Status").AsString(20).NotNullable().WithDefaultValue("Active");
    }

    public override void Down()
    {
        Delete.Table("Customers");
    }
}
