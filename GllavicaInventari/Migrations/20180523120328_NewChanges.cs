using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class NewChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Warehouses_ToWareHouseId",
                table: "Transfers");

            migrationBuilder.AlterColumn<int>(
                name: "ToWareHouseId",
                table: "Transfers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FromWareHouseId",
                table: "Transfers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_FromWareHouseId",
                table: "Transfers",
                column: "FromWareHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Warehouses_FromWareHouseId",
                table: "Transfers",
                column: "FromWareHouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Warehouses_ToWareHouseId",
                table: "Transfers",
                column: "ToWareHouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Warehouses_FromWareHouseId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Warehouses_ToWareHouseId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_FromWareHouseId",
                table: "Transfers");

            migrationBuilder.AlterColumn<int>(
                name: "ToWareHouseId",
                table: "Transfers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FromWareHouseId",
                table: "Transfers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Warehouses_ToWareHouseId",
                table: "Transfers",
                column: "ToWareHouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
