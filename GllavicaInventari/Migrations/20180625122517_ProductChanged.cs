using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class ProductChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "HasTVSH",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasTVSH",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Products",
                nullable: true);
        }
    }
}
