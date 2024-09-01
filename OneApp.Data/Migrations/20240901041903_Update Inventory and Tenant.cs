using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInventoryandTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tenant",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_Name",
                table: "Tenant",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Id_ProductId_Unique",
                table: "Inventory",
                columns: new[] { "Id", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionId_Unique",
                table: "Inventory",
                column: "TransactionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenant_Name",
                table: "Tenant");

            migrationBuilder.DropIndex(
                name: "IX_Id_ProductId_Unique",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_TransactionId_Unique",
                table: "Inventory");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tenant",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

        }
    }
}
