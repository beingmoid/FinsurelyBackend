using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PanoramaBackend.Data.Migrations
{
    public partial class _12292022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VacationApplication",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<string>(maxLength: 100, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<string>(maxLength: 100, nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserDetailId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ApplicantName = table.Column<string>(nullable: true),
                    JobTitle = table.Column<string>(nullable: true),
                    VacationTitle = table.Column<string>(nullable: true),
                    VacationDescription = table.Column<string>(nullable: true),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    BranchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationApplication_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VacationApplication_UserDetails_UserDetailId",
                        column: x => x.UserDetailId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "9c3d2021-9135-438f-8c12-f432f941b425");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e678",
                column: "ConcurrencyStamp",
                value: "84e0123b-dd24-4ce6-8f83-c1b74640378a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bac081fb-2504-49ec-a824-4372b166c9f1", "AQAAAAEAACcQAAAAEM1x8pnrd2PHQZ9ZkOe6QEO3xBZ/w4gUCBsDRsbyPkT1GRcdy92xiSZB1xc+ogmVtw==" });

            migrationBuilder.CreateIndex(
                name: "IX_VacationApplication_BranchId",
                table: "VacationApplication",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationApplication_UserDetailId",
                table: "VacationApplication",
                column: "UserDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VacationApplication");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                column: "ConcurrencyStamp",
                value: "07c30cbd-6c3e-49c7-88e6-8427f98f5978");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e678",
                column: "ConcurrencyStamp",
                value: "413db89d-ecc1-4c36-995b-763bfd0c88dd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7bd7d888-0908-4d3d-9e7d-15818b4f942f", "AQAAAAEAACcQAAAAEEyQlx/p1C39HjhaeAmY6juNdD8SIamNtkKiJZBQSCwq98UH0uJwZxUZT6wF55HGog==" });
        }
    }
}
