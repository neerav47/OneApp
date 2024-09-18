using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OneApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantIdtoProductandProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "ProductType",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "Product",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ProductType");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Product");
        }
    }
}
