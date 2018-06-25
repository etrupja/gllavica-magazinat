using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GllavicaInventari.Migrations
{
    public partial class NjesiaUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Unit",
                table: "Products",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Unit",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
