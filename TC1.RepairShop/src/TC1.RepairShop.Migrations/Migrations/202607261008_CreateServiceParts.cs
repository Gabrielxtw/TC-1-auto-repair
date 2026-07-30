using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202607261008)]
public class CreateServiceParts : Migration
{
    public override void Up()
    {
        Create.Table("ServiceParts")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_ServiceParts")
            .WithColumn("ServiceId").AsGuid().NotNullable()
                .ForeignKey("FK_ServiceParts_Services", "Services", "Id")
            .WithColumn("PartId").AsGuid().NotNullable()
                .ForeignKey("FK_ServiceParts_Parts", "Parts", "Id")
            .WithColumn("Quantity").AsInt32().NotNullable().WithDefaultValue(1);

        Create.Index("UQ_ServiceParts_ServiceId_PartId")
            .OnTable("ServiceParts")
            .OnColumn("ServiceId").Ascending()
            .OnColumn("PartId").Ascending()
            .WithOptions().Unique();
    }

    public override void Down()
    {
        Delete.Table("ServiceParts");
    }
}
