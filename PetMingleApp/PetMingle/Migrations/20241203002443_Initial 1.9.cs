using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetMingle.Migrations
{
    /// <inheritdoc />
    public partial class Initial19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "655d415a-0bac-43b2-a7f8-6dd5ad34d207");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d581b0f8-a461-473e-a33c-412675ebf829");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "17eb68a5-6c57-4b0c-8a28-5ed7a0d0746d", "9871769b-f133-4b43-9aa6-5297ebdb809c", "Admin", "ADMIN" },
                    { "e540647b-cf66-47a2-82d1-22cfbae1c29d", "d41606d1-b910-4f17-a6f6-684b5de20c11", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "17eb68a5-6c57-4b0c-8a28-5ed7a0d0746d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e540647b-cf66-47a2-82d1-22cfbae1c29d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "655d415a-0bac-43b2-a7f8-6dd5ad34d207", "bf653d8c-1c67-47e7-8fde-0e5e282e53a8", "User", "USER" },
                    { "d581b0f8-a461-473e-a33c-412675ebf829", "7883d567-ae81-4e30-9b16-0b598e8a35d4", "Admin", "ADMIN" }
                });
        }
    }
}
