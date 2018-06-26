using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class TransferModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferCode",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "HasTVSH",
                table: "Exits");

            migrationBuilder.DropColumn(
                name: "HasTVSH",
                table: "Entries");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Transfers",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "BillNumber",
                table: "Transfers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Transfers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillNumber",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Transfers");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "Transfers",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "TransferCode",
                table: "Transfers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasTVSH",
                table: "Exits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasTVSH",
                table: "Entries",
                nullable: false,
                defaultValue: false);
        }
    }
}
