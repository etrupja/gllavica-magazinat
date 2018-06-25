using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class ContactUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "NIPT",
                table: "Suppliers");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Suppliers",
                newName: "Contact");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Contact",
                table: "Suppliers",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NIPT",
                table: "Suppliers",
                nullable: true);
        }
    }
}
