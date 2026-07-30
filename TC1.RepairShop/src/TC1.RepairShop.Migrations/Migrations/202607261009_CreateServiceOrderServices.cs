using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202607261009)]
public class CreateServiceOrderServices : Migration
{
    public override void Up()
    {
        Create.Table("ServiceOrderServices")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ServiceOrderServices")
            .WithColumn("ServiceOrderId").AsGuid().NotNullable()
                .ForeignKey("FK_ServiceOrderServices_ServiceOrders", "ServiceOrders", "Id")
            .WithColumn("ServiceId").AsGuid().NotNullable()
                .ForeignKey("FK_ServiceOrderServices_Services", "Services", "Id");

        Create.Index("UQ_ServiceOrderServices_ServiceOrderId_ServiceId")
            .OnTable("ServiceOrderServices")
            .OnColumn("ServiceOrderId").Ascending()
            .OnColumn("ServiceId").Ascending()
            .WithOptions().Unique();
    }

    public override void Down()
    {
        Delete.Table("ServiceOrderServices");
    }
}
