using Microsoft.EntityFrameworkCore.Migrations;

namespace PanoramaBackend.Data.Migrations
{
    public partial class Init354als : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionReferenceNumber",
                table: "Transaction",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "56ee1736-753c-4ac4-affe-4fe2cd0810ab");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e678",
                column: "ConcurrencyStamp",
                value: "b94fd7aa-bb98-41d0-9ed2-b6bac152d1b8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8598a50c-13ee-4726-8afd-8cdfafb1c4d0", "AQAAAAEAACcQAAAAEKUkpciN7ASAB/MCWWhqnpcOM0lFH6C0YvdIryKKHLA16dcdC3dPNylpsv5RdFAOLg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionReferenceNumber",
                table: "Transaction");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "1cd4a3c5-b176-4d1c-bc1d-0c78cc0143e5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e678",
                column: "ConcurrencyStamp",
                value: "f7512c5d-baee-401f-9aa8-f9f620ba6cb9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9b4508e9-3cef-4283-bb78-97167c6d9bf3", "AQAAAAEAACcQAAAAEP+v33Tc4Xp44xTHV3+c8EyaF0fiyXcXf9WPcXzI1WI9P8fFE68X1W6k3Ak39s944Q==" });
        }
    }
}
