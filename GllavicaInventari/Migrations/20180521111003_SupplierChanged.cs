using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class SupplierChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Suppliers");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Suppliers",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Entires",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Suppliers",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Entires",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
