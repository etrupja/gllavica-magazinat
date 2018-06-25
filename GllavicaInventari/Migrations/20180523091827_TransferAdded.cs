using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class TransferAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Exits",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<int>(nullable: false),
                    DateTranfer = table.Column<DateTime>(nullable: false),
                    FromWareHouseId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LoggedInUserId = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ToWareHouseId = table.Column<int>(nullable: false),
                    TotalValue = table.Column<double>(nullable: false),
                    TransferCode = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transfers_Warehouses_ToWareHouseId",
                        column: x => x.ToWareHouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ProductId",
                table: "Transfers",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ToWareHouseId",
                table: "Transfers",
                column: "ToWareHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Exits",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
