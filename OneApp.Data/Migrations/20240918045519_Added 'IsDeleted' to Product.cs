﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsDeletedtoProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Product",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Product");
        }
    }
}
