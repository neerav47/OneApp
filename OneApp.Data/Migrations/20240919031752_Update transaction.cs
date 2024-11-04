using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace OneApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Updatetransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<long>(
            //    name: "Id",
            //    table: "Transaction",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(Guid),
            //    oldType: "char(36)")
            //    .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AlterColumn<long>(
            //    name: "TransactionId",
            //    table: "InventoryHistory",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(Guid),
            //    oldType: "char(36)");

            //migrationBuilder.AlterColumn<long>(
            //    name: "TransactionId",
            //    table: "Inventory",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(Guid),
            //    oldType: "char(36)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<Guid>(
            //    name: "Id",
            //    table: "Transaction",
            //    type: "char(36)",
            //    nullable: false,
            //    oldClrType: typeof(long),
            //    oldType: "bigint")
            //    .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<long>(
            //    name: "Number",
            //    table: "Transaction",
            //    type: "bigint",
            //    nullable: false,
            //    defaultValue: 0L);

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "TransactionId",
            //    table: "InventoryHistory",
            //    type: "char(36)",
            //    nullable: false,
            //    oldClrType: typeof(long),
            //    oldType: "bigint");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "TransactionId",
            //    table: "Inventory",
            //    type: "char(36)",
            //    nullable: false,
            //    oldClrType: typeof(long),
            //    oldType: "bigint");
        }
    }
}
