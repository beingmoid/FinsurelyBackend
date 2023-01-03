using Microsoft.EntityFrameworkCore.Migrations;

namespace PanoramaBackend.Data.Migrations
{
    public partial class _605122023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountDetailTypeId", "AccountId", "AsOf", "CreateTime", "CreateUserId", "DefaultTaxCode", "Description", "EditTime", "EditUserId", "IsDeleted", "IsSubAccount", "Name", "Number", "OpeningBalanceEquity", "isDeleteApplicable" },
                values: new object[] { 100, 45, null, null, null, null, null, "Refund Income Collector", null, null, false, null, "Refund Income Account", null, null, null });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "7a2e4a24-8b16-4113-a99e-fbc93e01dc8d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e678",
                column: "ConcurrencyStamp",
                value: "4aff202b-da43-4a63-ace8-4a621759f671");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "824298b4-5f4e-43b1-a6fc-570c2373cf30", "AQAAAAEAACcQAAAAELLUsk5oEhKGrSAWQPELttm4RBqLBKA/ZyYXSKEm1yJrnsys6TW8CkBQ56xOENyP5g==" });
        }
    }
}
