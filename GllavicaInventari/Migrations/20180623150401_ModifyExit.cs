using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class ModifyExit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Exits");

            migrationBuilder.AddColumn<bool>(
                name: "HasTVSH",
                table: "Exits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Exits",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalValueWithTVSH",
                table: "Exits",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasTVSH",
                table: "Exits");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Exits");

            migrationBuilder.DropColumn(
                name: "TotalValueWithTVSH",
                table: "Exits");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Exits",
                nullable: false,
                defaultValue: "");
        }
    }
}
