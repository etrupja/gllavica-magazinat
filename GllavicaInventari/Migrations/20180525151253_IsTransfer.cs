using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class IsTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTransfer",
                table: "Exits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTransfer",
                table: "Entries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTransfer",
                table: "Exits");

            migrationBuilder.DropColumn(
                name: "IsTransfer",
                table: "Entries");
        }
    }
}
