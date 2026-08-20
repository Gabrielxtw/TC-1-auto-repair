using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TC1.RepairShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceOrderParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderPart_Parts_PartId",
                table: "ServiceOrderPart");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderPart_ServiceOrders_ServiceOrderId",
                table: "ServiceOrderPart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceOrderPart",
                table: "ServiceOrderPart");

            migrationBuilder.RenameTable(
                name: "ServiceOrderPart",
                newName: "ServiceOrderParts");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderPart_ServiceOrderId",
                table: "ServiceOrderParts",
                newName: "IX_ServiceOrderParts_ServiceOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderPart_PartId",
                table: "ServiceOrderParts",
                newName: "IX_ServiceOrderParts_PartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceOrderParts",
                table: "ServiceOrderParts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderParts_Parts_PartId",
                table: "ServiceOrderParts",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderParts_ServiceOrders_ServiceOrderId",
                table: "ServiceOrderParts",
                column: "ServiceOrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderParts_Parts_PartId",
                table: "ServiceOrderParts");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderParts_ServiceOrders_ServiceOrderId",
                table: "ServiceOrderParts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceOrderParts",
                table: "ServiceOrderParts");

            migrationBuilder.RenameTable(
                name: "ServiceOrderParts",
                newName: "ServiceOrderPart");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderParts_ServiceOrderId",
                table: "ServiceOrderPart",
                newName: "IX_ServiceOrderPart_ServiceOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderParts_PartId",
                table: "ServiceOrderPart",
                newName: "IX_ServiceOrderPart_PartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceOrderPart",
                table: "ServiceOrderPart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderPart_Parts_PartId",
                table: "ServiceOrderPart",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderPart_ServiceOrders_ServiceOrderId",
                table: "ServiceOrderPart",
                column: "ServiceOrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
