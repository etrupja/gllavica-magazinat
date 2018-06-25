using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class EntryUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Entries");

            migrationBuilder.AddColumn<string>(
                name: "BillNumber",
                table: "Entries",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasTVSH",
                table: "Entries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Entries",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalValueWithTVSH",
                table: "Entries",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillNumber",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "HasTVSH",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "TotalValueWithTVSH",
                table: "Entries");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Entries",
                nullable: false,
                defaultValue: "");
        }
    }
}
