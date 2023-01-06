using Microsoft.EntityFrameworkCore.Migrations;

namespace PanoramaBackend.Data.Migrations
{
    public partial class _132023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceType",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "SalesInvoice");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceType",
                table: "SalesInvoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "SalesInvoice",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "41ab2eef-1fca-4d22-9eeb-4125159af303");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e678",
                column: "ConcurrencyStamp",
                value: "3dbe1e9c-cb04-49e6-a985-128d43839a08");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "97cbfa3a-32d7-4829-9251-3728c6fdfcca", "AQAAAAEAACcQAAAAEKEKUiNGUY59Jx1UVK4YNvQQ/NfTX7ay7eBfWOL1y/Ix/KeVgo4+V5zFFIZhFH3hLQ==" });
        }
    }
}
