using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202607261000)]
public class CreateUsers : Migration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey("PK_Users")
            .WithColumn("Username").AsString(100).NotNullable().Unique("UQ_Users_Username")
            .WithColumn("PasswordHash").AsString(200).NotNullable()
            .WithColumn("Role").AsString(20).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Users");
    }
}
