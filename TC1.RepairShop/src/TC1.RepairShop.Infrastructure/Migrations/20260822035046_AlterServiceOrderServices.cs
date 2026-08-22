using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TC1.RepairShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterServiceOrderServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderService_ServiceOrders_ServiceOrderId",
                table: "ServiceOrderService");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderService_Services_ServiceId",
                table: "ServiceOrderService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceOrderService",
                table: "ServiceOrderService");

            migrationBuilder.RenameTable(
                name: "ServiceOrderService",
                newName: "ServiceOrderServices");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderService_ServiceOrderId",
                table: "ServiceOrderServices",
                newName: "IX_ServiceOrderServices_ServiceOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderService_ServiceId",
                table: "ServiceOrderServices",
                newName: "IX_ServiceOrderServices_ServiceId");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "ServiceOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ServiceOrderParts",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ServiceOrderServices",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceOrderServices",
                table: "ServiceOrderServices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_ServiceId",
                table: "ServiceOrders",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrders_Services_ServiceId",
                table: "ServiceOrders",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderServices_ServiceOrders_ServiceOrderId",
                table: "ServiceOrderServices",
                column: "ServiceOrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderServices_Services_ServiceId",
                table: "ServiceOrderServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrders_Services_ServiceId",
                table: "ServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderServices_ServiceOrders_ServiceOrderId",
                table: "ServiceOrderServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrderServices_Services_ServiceId",
                table: "ServiceOrderServices");

            migrationBuilder.DropIndex(
                name: "IX_ServiceOrders_ServiceId",
                table: "ServiceOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceOrderServices",
                table: "ServiceOrderServices");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "ServiceOrders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceOrderParts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceOrderServices");

            migrationBuilder.RenameTable(
                name: "ServiceOrderServices",
                newName: "ServiceOrderService");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderServices_ServiceOrderId",
                table: "ServiceOrderService",
                newName: "IX_ServiceOrderService_ServiceOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceOrderServices_ServiceId",
                table: "ServiceOrderService",
                newName: "IX_ServiceOrderService_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceOrderService",
                table: "ServiceOrderService",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderService_ServiceOrders_ServiceOrderId",
                table: "ServiceOrderService",
                column: "ServiceOrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrderService_Services_ServiceId",
                table: "ServiceOrderService",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
