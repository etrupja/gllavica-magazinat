using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LoggedInUserId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Unit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    LoggedInUserId = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    NIPT = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entires",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    DateEntry = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LoggedInUserId = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    TotalValue = table.Column<double>(nullable: false),
                    WareHouseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entires_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entires_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entires_Warehouses_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    DateExit = table.Column<DateTime>(nullable: false),
                    LoggedInUserId = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    TotalValue = table.Column<double>(nullable: false),
                    WareHouseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exits_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exits_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exits_Warehouses_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entires_ProductId",
                table: "Entires",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Entires_SupplierId",
                table: "Entires",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Entires_WareHouseId",
                table: "Entires",
                column: "WareHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Exits_ProductId",
                table: "Exits",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Exits_SupplierId",
                table: "Exits",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Exits_WareHouseId",
                table: "Exits",
                column: "WareHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entires");

            migrationBuilder.DropTable(
                name: "Exits");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}
