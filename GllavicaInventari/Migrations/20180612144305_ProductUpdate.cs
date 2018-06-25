using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class ProductUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoggedInUserId",
                table: "Products",
                newName: "LoggedInUserFullName");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LoggedInUserFullName",
                table: "Products",
                newName: "LoggedInUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Products",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Products",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
