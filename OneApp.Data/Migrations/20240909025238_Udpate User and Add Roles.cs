using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OneApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class UdpateUserandAddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1706a615-1a92-4218-a5fa-1342a671fe8b", null, "Global admin", "GLOBAL ADMIN" },
                    { "18bcb22b-b211-4031-bbe7-0b5a0b6170b3", null, "Basic user", "BASIC USER" },
                    { "58c49c6a-e192-4fa6-a040-b4db3a87f22b", null, "System admin", "SYSTEM ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1706a615-1a92-4218-a5fa-1342a671fe8b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "18bcb22b-b211-4031-bbe7-0b5a0b6170b3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58c49c6a-e192-4fa6-a040-b4db3a87f22b");
        }
    }
}
