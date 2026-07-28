using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202607261004)]
public class CreateQuotes : Migration
{
    public override void Up()
    {
        Create.Table("Quotes")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Quotes")
            .WithColumn("ServiceOrderId").AsGuid().NotNullable()
                .ForeignKey("FK_Quotes_ServiceOrders", "ServiceOrders", "Id")
            .WithColumn("TotalAmount").AsDecimal(10, 2).NotNullable()
            .WithColumn("Status").AsString(30).NotNullable()
            .WithColumn("RejectionCount").AsInt32().NotNullable().WithDefaultValue(0);

        Create.ForeignKey("FK_ServiceOrders_Quotes")
            .FromTable("ServiceOrders").ForeignColumn("QuoteId")
            .ToTable("Quotes").PrimaryColumn("Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_ServiceOrders_Quotes").OnTable("ServiceOrders");
        Delete.Table("Quotes");
    }
}
