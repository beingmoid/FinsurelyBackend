using Microsoft.EntityFrameworkCore.Migrations;

namespace PanoramaBackend.Data.Migrations
{
    public partial class _728PM122023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InsuranceCompanyName",
                table: "SalesInvoice",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceCompanyName",
                table: "SalesInvoice");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "cbe16cad-0c8e-499d-8e5c-2135edacc0b5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e678",
                column: "ConcurrencyStamp",
                value: "4ac2c0fe-7e1c-489f-9fb8-b3dfb4f4bc13");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "fcfc7d10-7385-4c7c-b77f-3c08d4bdf523", "AQAAAAEAACcQAAAAEBEakDiSvAx/OweUkt6ieGJ8BXYQOc0YTA8go6JsCebAMqnBgqdWoPY744EEragvpw==" });
        }
    }
}
