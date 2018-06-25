using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class Changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entires_Products_ProductId",
                table: "Entires");

            migrationBuilder.DropForeignKey(
                name: "FK_Entires_Suppliers_SupplierId",
                table: "Entires");

            migrationBuilder.DropForeignKey(
                name: "FK_Entires_Warehouses_WareHouseId",
                table: "Entires");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entires",
                table: "Entires");

            migrationBuilder.RenameTable(
                name: "Entires",
                newName: "Entries");

            migrationBuilder.RenameIndex(
                name: "IX_Entires_WareHouseId",
                table: "Entries",
                newName: "IX_Entries_WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Entires_SupplierId",
                table: "Entries",
                newName: "IX_Entries_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Entires_ProductId",
                table: "Entries",
                newName: "IX_Entries_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entries",
                table: "Entries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Products_ProductId",
                table: "Entries",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Suppliers_SupplierId",
                table: "Entries",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Warehouses_WareHouseId",
                table: "Entries",
                column: "WareHouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Products_ProductId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Suppliers_SupplierId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Warehouses_WareHouseId",
                table: "Entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entries",
                table: "Entries");

            migrationBuilder.RenameTable(
                name: "Entries",
                newName: "Entires");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_WareHouseId",
                table: "Entires",
                newName: "IX_Entires_WareHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_SupplierId",
                table: "Entires",
                newName: "IX_Entires_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_ProductId",
                table: "Entires",
                newName: "IX_Entires_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entires",
                table: "Entires",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Entires_Products_ProductId",
                table: "Entires",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entires_Suppliers_SupplierId",
                table: "Entires",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entires_Warehouses_WareHouseId",
                table: "Entires",
                column: "WareHouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
