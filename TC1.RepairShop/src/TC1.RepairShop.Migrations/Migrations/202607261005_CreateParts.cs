using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202607261005)]
public class CreateParts : Migration
{
    public override void Up()
    {
        Create.Table("Parts")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Parts")
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("UnitPrice").AsDecimal(10, 2).NotNullable()
            .WithColumn("StockQuantity").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("MinimumQuantity").AsInt32().NotNullable().WithDefaultValue(0);
    }

    public override void Down()
    {
        Delete.Table("Parts");
    }
}
