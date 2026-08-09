using FluentMigrator;

namespace TC1.RepairShop.Migrations.Migrations;

[Migration(202608041100)]
public class AddPasswordHashToCustomers : Migration
{
    public override void Up()
    {
        Alter.Table("Customers")
            .AddColumn("PasswordHash").AsString(200).Nullable();
    }

    public override void Down()
    {
        Delete.Column("PasswordHash").FromTable("Customers");
    }
}
