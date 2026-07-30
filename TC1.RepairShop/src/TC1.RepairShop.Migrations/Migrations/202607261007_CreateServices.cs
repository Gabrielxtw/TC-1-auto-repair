using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202607261007)]
public class CreateServices : Migration
{
    public override void Up()
    {
        Create.Table("Services")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Services")
            .WithColumn("Name").AsString(200).NotNullable()
            .WithColumn("Description").AsString(1000).NotNullable()
            .WithColumn("Status").AsString(20).NotNullable().WithDefaultValue("Active");
    }

    public override void Down()
    {
        Delete.Table("Services");
    }
}
