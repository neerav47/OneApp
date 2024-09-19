using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Updatetransactioncolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LastUpdatedBy",
                table: "Transaction",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Transaction",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedBy",
                table: "Transaction",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedBy",
                table: "Transaction",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)");
        }
    }
}
