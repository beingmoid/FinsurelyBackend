using Microsoft.EntityFrameworkCore.Migrations;

namespace PanoramaBackend.Data.Migrations
{
    public partial class _13202311 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PremiumPrice",
                table: "SaleLineItem");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "431011e3-f1ce-4958-aa9e-938ee91464c0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e678",
                column: "ConcurrencyStamp",
                value: "6ab5309e-1f62-4ae8-987c-ea83c12d8554");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "0374b5e4-7d20-4209-a488-197ad2ab6073", "AQAAAAEAACcQAAAAEL9OULB0KXHcyWzUlTftQ/QgJ3u/JLC6/HmZFujJ9q91dRQJlgvSJsVJCpHZmkHAyQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PremiumPrice",
                table: "SaleLineItem",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "9facc9e6-c955-4c6d-acc0-2c958a1a0471");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e678",
                column: "ConcurrencyStamp",
                value: "b67404e3-0be0-4b41-a043-328ec0b51555");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "51b35a57-b05b-426b-be40-a74c3db34596", "AQAAAAEAACcQAAAAEMoGm+BI6R0cpT55Uo0GT9crZsJG7sk1GDoGDxs7er65NxYbzH4YgXIrPv05bEZXGQ==" });
        }
    }
}
