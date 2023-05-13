using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PanoramaBackend.Data.Migrations
{
    public partial class _54654654DATA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Announcement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Fullname = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AnnoucementTitle = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BDType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    IsForBenefit = table.Column<bool>(nullable: true),
                    IsForDeduction = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BDType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BDType_BDType_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "BDType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BodyType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BranchAddress = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    BlobFileName = table.Column<string>(nullable: true),
                    BlobURI = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PreferredPaymentMethod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferredPaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Priority",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priority", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SetupClient",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    FirstTime = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetupClient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCompanyInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    LegalStructure = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ContactInformation = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    LogoUrl = table.Column<string>(nullable: true),
                    LogoBase64 = table.Column<string>(nullable: true),
                    PayrollDate = table.Column<DateTime>(nullable: true),
                    VATRegistrationNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    isMigrationRequired = table.Column<bool>(nullable: false),
                    nvarchar100 = table.Column<string>(name: "nvarchar(100)", nullable: true),
                    VatRegisterOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCompanyInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Make = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountDetailType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    AccountTypeId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountDetailType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountDetailType_AccountType_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    AccountId = table.Column<int>(nullable: true),
                    AccountDetailTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DefaultTaxCode = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    IsSubAccount = table.Column<bool>(nullable: true),
                    OpeningBalanceEquity = table.Column<decimal>(nullable: true),
                    AsOf = table.Column<DateTime>(nullable: true),
                    isDeleteApplicable = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountDetailType_AccountDetailTypeId",
                        column: x => x.AccountDetailTypeId,
                        principalTable: "AccountDetailType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accounts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountsMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    FormName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AccountId = table.Column<int>(nullable: true),
                    isMapped = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsMapping_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ExpenseName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ExpenseDate = table.Column<DateTime>(nullable: false),
                    ExpenseCategoryId = table.Column<int>(nullable: false),
                    ExpenseAmount = table.Column<decimal>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    BranchId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expense_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expense_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Expense_ExpenseCategory_ExpenseCategoryId",
                        column: x => x.ExpenseCategoryId,
                        principalTable: "ExpenseCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BranchId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ExpenseAccountId = table.Column<int>(nullable: false),
                    IsRecurring = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payroll_Accounts_ExpenseAccountId",
                        column: x => x.ExpenseAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsCustomer = table.Column<bool>(nullable: true),
                    IsEmployee = table.Column<bool>(nullable: true),
                    IsSupplier = table.Column<bool>(nullable: true),
                    IsInsuranceCompany = table.Column<bool>(nullable: true),
                    IsAgent = table.Column<bool>(nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Suffix = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DisplayNameAs = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Other = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BillWithParent = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    IsSubCustomer = table.Column<bool>(nullable: true),
                    UserDetailId = table.Column<int>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    OpenBalance = table.Column<decimal>(nullable: true),
                    DefaultAccountId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDetails_Accounts_DefaultAccountId",
                        column: x => x.DefaultAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDetails_UserDetails_UserDetailId",
                        column: x => x.UserDetailId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDetails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserDetailId = table.Column<int>(nullable: true),
                    BillingAddress = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_UserDetails_UserDetailId",
                        column: x => x.UserDetailId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 256, nullable: true),
                    UserDetailsId = table.Column<int>(nullable: true),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_UserDetails_UserDetailsId",
                        column: x => x.UserDetailsId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserDetailId = table.Column<int>(nullable: true),
                    AttachmentUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_UserDetails_UserDetailId",
                        column: x => x.UserDetailId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComissionRate",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserDetailId = table.Column<int>(nullable: false),
                    IsTpl = table.Column<bool>(nullable: false),
                    IsNonTpl = table.Column<bool>(nullable: false),
                    Rates = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ActiveDate = table.Column<DateTime>(nullable: true),
                    ExpiredDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComissionRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComissionRate_UserDetails_UserDetailId",
                        column: x => x.UserDetailId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    SalesAgentId = table.Column<int>(nullable: true),
                    InsuranceCompanyId = table.Column<int>(nullable: true),
                    Memo = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    TransactionReferenceNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DepositAccountId = table.Column<int>(nullable: true),
                    CreditAccountId = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    IsPaymentDebit = table.Column<bool>(nullable: false),
                    IsPaymentCredit = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Accounts_CreditAccountId",
                        column: x => x.CreditAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_Accounts_DepositAccountId",
                        column: x => x.DepositAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_UserDetails_InsuranceCompanyId",
                        column: x => x.InsuranceCompanyId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payment_UserDetails_SalesAgentId",
                        column: x => x.SalesAgentId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentAndBilling",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserDetailId = table.Column<int>(nullable: true),
                    PreferredPaymentMethodId = table.Column<int>(nullable: true),
                    PreferredDeliveryMethod = table.Column<int>(nullable: false),
                    TermsId = table.Column<int>(nullable: true),
                    OpeningBalance = table.Column<decimal>(nullable: true),
                    Asof = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentAndBilling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentAndBilling_PreferredPaymentMethod_PreferredPaymentMethodId",
                        column: x => x.PreferredPaymentMethodId,
                        principalTable: "PreferredPaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentAndBilling_Terms_TermsId",
                        column: x => x.TermsId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentAndBilling_UserDetails_UserDetailId",
                        column: x => x.UserDetailId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reconcilation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    SalesAgentId = table.Column<int>(nullable: true),
                    DocumentId = table.Column<int>(nullable: false),
                    GeneratedDate = table.Column<DateTime>(nullable: false),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    AmountDifference = table.Column<decimal>(nullable: false),
                    NoOfSalesMissing = table.Column<int>(nullable: false),
                    InsuranceCompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reconcilation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reconcilation_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reconcilation_UserDetails_InsuranceCompanyId",
                        column: x => x.InsuranceCompanyId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reconcilation_UserDetails_SalesAgentId",
                        column: x => x.SalesAgentId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Refund",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    AgentId = table.Column<int>(nullable: true),
                    InsuranceCompanyId = table.Column<int>(nullable: true),
                    RefundDate = table.Column<DateTime>(nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    PolicyNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    InsuranceTypeId = table.Column<int>(nullable: true),
                    VehilcleId = table.Column<int>(nullable: true),
                    MessageOnReceipt = table.Column<string>(nullable: true),
                    MessageOnStatement = table.Column<string>(nullable: true),
                    AmountForSalesAgent = table.Column<decimal>(nullable: false),
                    AmountForBroker = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refund_UserDetails_AgentId",
                        column: x => x.AgentId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refund_UserDetails_InsuranceCompanyId",
                        column: x => x.InsuranceCompanyId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refund_InsuranceType_InsuranceTypeId",
                        column: x => x.InsuranceTypeId,
                        principalTable: "InsuranceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Refund_Vehicle_VehilcleId",
                        column: x => x.VehilcleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    OtherFieldsAndValues = table.Column<string>(nullable: true),
                    Total = table.Column<decimal>(nullable: true),
                    Gross = table.Column<decimal>(nullable: true),
                    VAT = table.Column<decimal>(nullable: true),
                    Commission = table.Column<decimal>(nullable: true),
                    CommisionRate = table.Column<decimal>(nullable: true),
                    Net = table.Column<decimal>(nullable: true),
                    SalesPrice = table.Column<decimal>(nullable: true),
                    ActualComission = table.Column<decimal>(nullable: true),
                    PolicyNumber = table.Column<string>(nullable: true),
                    VehilcleId = table.Column<int>(nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ChassisNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    InsuranceCompanyName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CustomerDetailId = table.Column<int>(nullable: true),
                    SalesInvoiceDate = table.Column<DateTime>(nullable: false),
                    SalesInvoicePersonId = table.Column<int>(nullable: true),
                    BodyTypeId = table.Column<int>(nullable: true),
                    InsuranceCompanyId = table.Column<int>(nullable: true),
                    InsuranceTypeId = table.Column<int>(nullable: true),
                    BranchId = table.Column<Guid>(nullable: true),
                    UnderWritter = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    PaymentMethodId = table.Column<int>(nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoice_BodyType_BodyTypeId",
                        column: x => x.BodyTypeId,
                        principalTable: "BodyType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesInvoice_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesInvoice_UserDetails_CustomerDetailId",
                        column: x => x.CustomerDetailId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesInvoice_UserDetails_InsuranceCompanyId",
                        column: x => x.InsuranceCompanyId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesInvoice_InsuranceType_InsuranceTypeId",
                        column: x => x.InsuranceTypeId,
                        principalTable: "InsuranceType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesInvoice_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesInvoice_UserDetails_SalesInvoicePersonId",
                        column: x => x.SalesInvoicePersonId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesInvoice_Vehicle_VehilcleId",
                        column: x => x.VehilcleId,
                        principalTable: "Vehicle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskTodo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TaskName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AssignedToId = table.Column<int>(nullable: true),
                    AssignedById = table.Column<int>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    PriorityId = table.Column<int>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    SendUpdate = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTodo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTodo_UserDetails_AssignedById",
                        column: x => x.AssignedById,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskTodo_UserDetails_AssignedToId",
                        column: x => x.AssignedToId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskTodo_Priority_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "Priority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskTodo_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ManagerId = table.Column<int>(nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_UserDetails_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VacationApplication",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    UserDetailId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ApplicantName = table.Column<string>(type: "nvarchar(1200)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    VacationTitle = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    VacationDescription = table.Column<string>(type: "nvarchar(1200)", nullable: true),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    BranchId = table.Column<Guid>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ReconcilationReportId = table.Column<int>(nullable: false),
                    TempId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyInformation_Reconcilation_ReconcilationReportId",
                        column: x => x.ReconcilationReportId,
                        principalTable: "Reconcilation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    Memo = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    UserDetailId = table.Column<int>(nullable: true),
                    SalesInvoiceId = table.Column<int>(nullable: true),
                    PaymentId = table.Column<int>(nullable: true),
                    BranchId = table.Column<Guid>(nullable: true),
                    TransactionReferenceNumber = table.Column<string>(nullable: true),
                    RefundId = table.Column<int>(nullable: true),
                    TransactionType = table.Column<int>(nullable: true),
                    ExpenseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Expense_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expense",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Refund_RefundId",
                        column: x => x.RefundId,
                        principalTable: "Refund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_SalesInvoice_SalesInvoiceId",
                        column: x => x.SalesInvoiceId,
                        principalTable: "SalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_UserDetails_UserDetailId",
                        column: x => x.UserDetailId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmploymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ManagerId = table.Column<int>(nullable: true),
                    UserDetailId = table.Column<int>(nullable: false),
                    Position = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    HiredDate = table.Column<DateTime>(nullable: false),
                    TeamId = table.Column<int>(nullable: true),
                    EmployeeIsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmploymentDetails_UserDetails_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmploymentDetails_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmploymentDetails_UserDetails_UserDetailId",
                        column: x => x.UserDetailId,
                        principalTable: "UserDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LedgarEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    DebitAccountId = table.Column<int>(nullable: true),
                    CreditAccountId = table.Column<int>(nullable: true),
                    EntryNote = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TransactionId = table.Column<int>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgarEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LedgarEntries_Accounts_CreditAccountId",
                        column: x => x.CreditAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LedgarEntries_Accounts_DebitAccountId",
                        column: x => x.DebitAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LedgarEntries_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmploymentDetailId = table.Column<int>(nullable: false),
                    AccountHolderName = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ibanNumber = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    BankType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankDetails_EmploymentDetails_EmploymentDetailId",
                        column: x => x.EmploymentDetailId,
                        principalTable: "EmploymentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BenefitsAndDeduction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmploymentDetailId = table.Column<int>(nullable: false),
                    ApplicableDate = table.Column<DateTime>(nullable: true),
                    Applied = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenefitsAndDeduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenefitsAndDeduction_EmploymentDetails_EmploymentDetailId",
                        column: x => x.EmploymentDetailId,
                        principalTable: "EmploymentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Compensation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmploymentDetailId = table.Column<int>(nullable: false),
                    SalaryAmount = table.Column<decimal>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    Effective = table.Column<bool>(nullable: false),
                    Expired = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compensation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compensation_EmploymentDetails_EmploymentDetailId",
                        column: x => x.EmploymentDetailId,
                        principalTable: "EmploymentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmploymentDetailId = table.Column<int>(nullable: false),
                    DocumentUrl = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeFiles_EmploymentDetails_EmploymentDetailId",
                        column: x => x.EmploymentDetailId,
                        principalTable: "EmploymentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmploymentStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmploymentDetailId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmploymentStatus_EmploymentDetails_EmploymentDetailId",
                        column: x => x.EmploymentDetailId,
                        principalTable: "EmploymentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VacationPolicy",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmploymentDetailId = table.Column<int>(nullable: false),
                    AnnualLeavesCount = table.Column<int>(nullable: false),
                    CasualLeaveCount = table.Column<int>(nullable: false),
                    SickLeaveCount = table.Column<int>(nullable: false),
                    ForYear = table.Column<DateTime>(nullable: false),
                    Applicable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VacationPolicy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VacationPolicy_EmploymentDetails_EmploymentDetailId",
                        column: x => x.EmploymentDetailId,
                        principalTable: "EmploymentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Benefits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    BenefitAndDeductionId = table.Column<int>(nullable: false),
                    BenefitTypeId = table.Column<int>(nullable: false),
                    PayStubLabel = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Occurance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Benefits_BenefitsAndDeduction_BenefitAndDeductionId",
                        column: x => x.BenefitAndDeductionId,
                        principalTable: "BenefitsAndDeduction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Benefits_BDType_BenefitTypeId",
                        column: x => x.BenefitTypeId,
                        principalTable: "BDType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deduction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    BenefitAndDeductionId = table.Column<int>(nullable: false),
                    DeductionTypeId = table.Column<int>(nullable: false),
                    PayStubLabel = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Occurance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deduction_BenefitsAndDeduction_BenefitAndDeductionId",
                        column: x => x.BenefitAndDeductionId,
                        principalTable: "BenefitsAndDeduction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deduction_BDType_DeductionTypeId",
                        column: x => x.DeductionTypeId,
                        principalTable: "BDType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeaveApplication",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmploymentStatusId = table.Column<int>(nullable: false),
                    IsSickLeave = table.Column<bool>(nullable: false),
                    IsCasualLeave = table.Column<bool>(nullable: false),
                    IsAnnualLeave = table.Column<bool>(nullable: false),
                    LastDayOfWork = table.Column<DateTime>(nullable: false),
                    DateOfReturn = table.Column<DateTime>(nullable: false),
                    LeaveAppoved = table.Column<bool>(nullable: true),
                    LeaveRejected = table.Column<bool>(nullable: true),
                    LeaveNoteFromEmployee = table.Column<string>(nullable: true),
                    LeaveIsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveApplication_EmploymentStatus_EmploymentStatusId",
                        column: x => x.EmploymentStatusId,
                        principalTable: "EmploymentStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffOffBoarding",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateUserId = table.Column<Guid>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    EditUserId = table.Column<Guid>(nullable: true),
                    EditTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true),
                    EmploymentStatusId = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    LastDayOfWork = table.Column<DateTime>(nullable: false),
                    DateOfNotice = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffOffBoarding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffOffBoarding_EmploymentStatus_EmploymentStatusId",
                        column: x => x.EmploymentStatusId,
                        principalTable: "EmploymentStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AccountType",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "IsDeleted", "Name", "Type" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, "Accounts Receivable (A/R)", "Assets" },
                    { 15, null, null, null, null, null, "Other Expense", "Expense" },
                    { 14, null, null, null, null, null, "Expense", "Expense" },
                    { 13, null, null, null, null, null, "Cost of Sales Invoice", "Liablity" },
                    { 12, null, null, null, null, null, "Other income", "Revenues/Income" },
                    { 10, null, null, null, null, null, "Non-current liabilities", "Liablity" },
                    { 9, null, null, null, null, null, "Current liabilities", "Liablity" },
                    { 11, null, null, null, null, null, "Owners Equity", "Owner’s equity" },
                    { 7, null, null, null, null, null, "Income", "Revenues/Income" },
                    { 6, null, null, null, null, null, "Accounts Payable (A/P)", "Liablity" },
                    { 5, null, null, null, null, null, "Non-current assetss", "Assets" },
                    { 4, null, null, null, null, null, "Fixed assets", "Assets" },
                    { 3, null, null, null, null, null, "Cash and cash equivalents", "Assets" },
                    { 2, null, null, null, null, null, "Current Assets", "Assets" },
                    { 8, null, null, null, null, null, "Credit Card", "Liablity" }
                });

            migrationBuilder.InsertData(
                table: "AccountsMapping",
                columns: new[] { "Id", "AccountId", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "FormName", "IsDeleted", "isMapped" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, "Sales Agent", null, null },
                    { 2, null, null, null, null, null, "Sales", null, null },
                    { 3, null, null, null, null, null, "Insurance Broker", null, null },
                    { 4, null, null, null, null, null, "Transfer", null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "IsDeleted", "Name", "NormalizedName", "UserDetailsId" },
                values: new object[,]
                {
                    { new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f"), "b9a04050-79e9-4b1d-b00c-efa235ce958c", null, null, null, null, null, "Admin", "Admin", null },
                    { new Guid("a478b39e-4387-4f1f-be58-f27e9b1418e6"), "1eb7bae4-e29a-452b-b5a9-20f3521e9294", null, null, null, null, null, "CompanyAdmin", "CompanyAdmin", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("92043fc4-b79d-4adb-9078-c046ecfe4342"), 0, "c6bf7e3d-0f12-4cef-aa40-aaf9653f1985", null, null, null, null, "admin@nukeslab.com", true, null, false, null, "admin@nukeslab.com", "admin", "AQAAAAEAACcQAAAAEADVcEXnprx4hqBniCTl/O3TdRLD4GifFg++ERbl0W6Xr8MQt8avRKXRWHykTlNIYQ==", "+923400064394", false, "", false, "moid" });

            migrationBuilder.InsertData(
                table: "BDType",
                columns: new[] { "Id", "CategoryId", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "IsDeleted", "IsForBenefit", "IsForDeduction", "Name" },
                values: new object[] { 1, null, null, null, null, null, null, null, true, "Deduction For Leave" });

            migrationBuilder.InsertData(
                table: "BodyType",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "IsDeleted", "Name" },
                values: new object[] { 1, null, null, null, null, null, "NORMAL" });

            migrationBuilder.InsertData(
                table: "InsuranceType",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 2, null, null, null, null, null, "COMP" },
                    { 1, null, null, null, null, null, "TPL" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 4, null, null, null, null, null, "Credit Card" },
                    { 1, null, null, null, null, null, "Cash" },
                    { 2, null, null, null, null, null, "Cheque" },
                    { 3, null, null, null, null, null, "Debit Card" }
                });

            migrationBuilder.InsertData(
                table: "PreferredPaymentMethod",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "IsDeleted", "Text" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, "Cash" },
                    { 2, null, null, null, null, null, "Cheque" },
                    { 3, null, null, null, null, null, "Debit Card" },
                    { 4, null, null, null, null, null, "Credit Card" }
                });

            migrationBuilder.InsertData(
                table: "Priority",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 2, null, null, null, null, null, "medium" },
                    { 1, null, null, null, null, null, "low" },
                    { 3, null, null, null, null, null, "high" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 2, null, null, null, null, null, "inprocess" },
                    { 3, null, null, null, null, null, "pending" },
                    { 1, null, null, null, null, null, "completed" }
                });

            migrationBuilder.InsertData(
                table: "Terms",
                columns: new[] { "Id", "CreateTime", "CreateUserId", "EditTime", "EditUserId", "IsDeleted", "Text" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, "Due on recipt" },
                    { 2, null, null, null, null, null, "Net 15" },
                    { 3, null, null, null, null, null, "Net 30" },
                    { 4, null, null, null, null, null, "Net 60" }
                });

            migrationBuilder.InsertData(
                table: "AccountDetailType",
                columns: new[] { "Id", "AccountTypeId", "CreateTime", "CreateUserId", "Description", "EditTime", "EditUserId", "IsDeleted" },
                values: new object[,]
                {
                    { 1, 1, null, null, "Accounts Receivable (A/R)", null, null, null },
                    { 95, 14, null, null, "Advertising/Promotional", null, null, null },
                    { 94, 13, null, null, "Supplies and materials - COS", null, null, null },
                    { 93, 13, null, null, "Other costs of SalesInvoice - COS", null, null, null },
                    { 92, 13, null, null, "Freight and delivery - COS", null, null, null },
                    { 91, 13, null, null, "Equipment rental - COS", null, null, null },
                    { 90, 13, null, null, "Cost of labour - COS", null, null, null },
                    { 89, 12, null, null, "Unrealised loss on securities, net of tax", null, null, null },
                    { 88, 12, null, null, "Tax-Exempt Interest", null, null, null },
                    { 87, 12, null, null, "Other operating income", null, null, null },
                    { 86, 12, null, null, "Other Miscellaneous Income", null, null, null },
                    { 85, 12, null, null, "Other Investment Income", null, null, null },
                    { 84, 12, null, null, "Dividend income", null, null, null },
                    { 83, 11, null, null, "Treasury Shares", null, null, null },
                    { 97, 14, null, null, "Auto", null, null, null },
                    { 82, 11, null, null, "Share capital", null, null, null },
                    { 80, 11, null, null, "Preferred shares", null, null, null },
                    { 79, 11, null, null, "Partner's Equity", null, null, null },
                    { 78, 11, null, null, "Partner Distributions", null, null, null },
                    { 77, 11, null, null, "Partner Contributions", null, null, null },
                    { 76, 11, null, null, "Paid-in capital or surplus", null, null, null },
                    { 75, 11, null, null, "Owner's Equity", null, null, null },
                    { 74, 11, null, null, "Other comprehensive income", null, null, null },
                    { 73, 11, null, null, "Ordinary shares", null, null, null },
                    { 72, 11, null, null, "Opening Balance Equity", null, null, null },
                    { 71, 11, null, null, "Dividend disbursed", null, null, null },
                    { 70, 11, null, null, "Accumulated adjustment", null, null, null },
                    { 69, 10, null, null, "Shareholder Notes Payable", null, null, null },
                    { 68, 10, null, null, "Other non-current liabilities", null, null, null },
                    { 81, 11, null, null, "Retained Earnings", null, null, null },
                    { 67, 10, null, null, "Notes Payable", null, null, null },
                    { 98, 14, null, null, "Bad debts", null, null, null },
                    { 101, 14, null, null, "Commissions and fees", null, null, null },
                    { 128, 15, null, null, "Penalties and settlements", null, null, null },
                    { 127, 15, null, null, "Other Expense", null, null, null },
                    { 126, 15, null, null, "Exchange Gain or Loss", null, null, null },
                    { 125, 15, null, null, "Depreciation", null, null, null },
                    { 124, 15, null, null, "Amortisation", null, null, null },
                    { 96, 15, null, null, "Amortisation expense", null, null, null },
                    { 123, 14, null, null, "Utilities", null, null, null },
                    { 122, 14, null, null, "Unapplied Cash Bill Payment Expense", null, null, null },
                    { 121, 14, null, null, "Travel expenses - selling expense", null, null, null },
                    { 120, 14, null, null, "Travel expenses - general and admin expenses", null, null, null },
                    { 119, 14, null, null, "Taxes Paid", null, null, null },
                    { 118, 14, null, null, "Shipping and delivery expense", null, null, null },
                    { 117, 14, null, null, "Repair and maintenance", null, null, null },
                    { 100, 14, null, null, "Charitable Contributions", null, null, null },
                    { 116, 14, null, null, "Rent or Lease of Buildings", null, null, null },
                    { 114, 14, null, null, "Other Miscellaneous Service Cost", null, null, null },
                    { 113, 14, null, null, "Office/General Administrative Expenses", null, null, null },
                    { 112, 14, null, null, "Meals and entertainment", null, null, null },
                    { 111, 14, null, null, "Management compensation", null, null, null },
                    { 110, 14, null, null, "Loss on discontinued operations, net of tax", null, null, null },
                    { 109, 14, null, null, "Legal and professional fees", null, null, null },
                    { 108, 14, null, null, "Interest paid", null, null, null },
                    { 107, 14, null, null, "Insurance", null, null, null },
                    { 106, 14, null, null, "Income tax expense", null, null, null },
                    { 105, 14, null, null, "Finance costs", null, null, null },
                    { 104, 14, null, null, "Equipment rental", null, null, null },
                    { 103, 14, null, null, "Dues and Subscriptions", null, null, null },
                    { 102, 14, null, null, "Cost of Labour", null, null, null },
                    { 115, 14, null, null, "Payroll Expenses", null, null, null },
                    { 66, 10, null, null, "Long-term debt", null, null, null },
                    { 99, 14, null, null, "Bank charges", null, null, null },
                    { 64, 10, null, null, "Accrued non-current liabilities", null, null, null },
                    { 29, 5, null, null, "Accumulated amortisation of non-current assets", null, null, null },
                    { 28, 4, null, null, "Vehicles", null, null, null },
                    { 27, 4, null, null, "Other fixed assets", null, null, null },
                    { 26, 4, null, null, "Machinery and equipment", null, null, null },
                    { 25, 4, null, null, "Leasehold Improvements", null, null, null },
                    { 24, 4, null, null, "Furniture and Fixtures", null, null, null },
                    { 23, 4, null, null, "Depletable Assets", null, null, null },
                    { 22, 4, null, null, "Buildings", null, null, null },
                    { 21, 4, null, null, "Accumulated depletion", null, null, null },
                    { 20, 3, null, null, "Savings", null, null, null },
                    { 19, 3, null, null, "Rents Held in Trust", null, null, null },
                    { 18, 3, null, null, "Money Market", null, null, null },
                    { 16, 3, null, null, "Cash on hand", null, null, null },
                    { 30, 5, null, null, "Assets held for sale", null, null, null },
                    { 15, 3, null, null, "Cash and cash equivalents", null, null, null },
                    { 13, 2, null, null, "Undeposited Funds", null, null, null },
                    { 12, 2, null, null, "Retainage", null, null, null },
                    { 11, 2, null, null, "Prepaid Expenses", null, null, null },
                    { 10, 2, null, null, "Loans to Shareholders", null, null, null },
                    { 9, 2, null, null, "Loans to Others", null, null, null },
                    { 8, 2, null, null, "Loans To Officers", null, null, null },
                    { 7, 2, null, null, "Investments - Other", null, null, null },
                    { 6, 2, null, null, "Inventory", null, null, null },
                    { 5, 2, null, null, "Employee Cash Advances", null, null, null },
                    { 4, 2, null, null, "Development Cost", null, null, null },
                    { 3, 2, null, null, "Assets Available for sale", null, null, null },
                    { 2, 2, null, null, "Allowance for bad debits", null, null, null },
                    { 65, 10, null, null, "Liabilities related to assets held for sale", null, null, null },
                    { 14, 3, null, null, "Bank", null, null, null },
                    { 31, 5, null, null, "Deferred tax", null, null, null },
                    { 17, 3, null, null, "Client trust account", null, null, null },
                    { 54, 9, null, null, "Insurance payable", null, null, null },
                    { 63, 10, null, null, "Accrued holiday payable", null, null, null },
                    { 32, 5, null, null, "Goodwill", null, null, null },
                    { 62, 9, null, null, "SalesInvoice and service tax payable", null, null, null },
                    { 61, 9, null, null, "Rents in trust - Liability", null, null, null },
                    { 60, 9, null, null, "Prepaid Expenses Payable", null, null, null },
                    { 59, 9, null, null, "Payroll liabilities", null, null, null },
                    { 58, 9, null, null, "Payroll Clearing", null, null, null },
                    { 57, 9, null, null, "Other current liabilities", null, null, null },
                    { 56, 9, null, null, "Loan Payable", null, null, null },
                    { 55, 9, null, null, "Line of Credit", null, null, null },
                    { 53, 9, null, null, "Income tax payable", null, null, null },
                    { 52, 9, null, null, "Dividends payable", null, null, null },
                    { 51, 9, null, null, "Current portion of obligations under finance leases", null, null, null },
                    { 49, 9, null, null, "Client Trust Accounts - Liabilities", null, null, null },
                    { 48, 9, null, null, "Accrued liabilities", null, null, null },
                    { 50, 9, null, null, "Current Tax Liability", null, null, null },
                    { 47, 8, null, null, "Credit Card", null, null, null },
                    { 33, 5, null, null, "Intangible Assets", null, null, null },
                    { 34, 5, null, null, "Lease Buyout", null, null, null },
                    { 35, 5, null, null, "Licences", null, null, null },
                    { 37, 5, null, null, "Long-term investments", null, null, null },
                    { 38, 5, null, null, "Organisational Costs", null, null, null },
                    { 39, 5, null, null, "Other non-current assets", null, null, null },
                    { 41, 6, null, null, "Accounts Payable (A/P)", null, null, null },
                    { 40, 5, null, null, "Security Deposits", null, null, null },
                    { 43, 7, null, null, "Non-Profit Income", null, null, null },
                    { 44, 7, null, null, "Other Primary Income", null, null, null },
                    { 45, 7, null, null, "SalesInvoice - services", null, null, null },
                    { 46, 7, null, null, "Unapplied Cash Payment Income", null, null, null },
                    { 42, 7, null, null, "Discounts/Refunds Given", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 47, "Accounting", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 46, "Accounting", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 43, "Expenses", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 45, "Expenses", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 44, "Expenses", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 42, "Expenses", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 65, "Reports", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 40, "Documents", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 39, "Documents", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 38, "Documents", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 37, "Documents", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 48, "Accounting", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 36, "Documents", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 41, "Expenses", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 49, "Accounting", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 64, "Reports", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 51, "Workplace", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 52, "Workplace", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 53, "Workplace", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 54, "Workplace", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 55, "Workplace", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 56, "Teams", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 57, "Teams", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 58, "Teams", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 59, "Teams", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 60, "Teams", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 61, "Reports", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 62, "Reports", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 63, "Reports", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 35, "Task", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 50, "Accounting", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 34, "Task", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 2, "Dashboard", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 32, "Task", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 1, "Dashboard", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 3, "Dashboard", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 4, "Dashboard", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 5, "Dashboard", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 6, "Branch", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 7, "Branch", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 8, "Branch", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 9, "Branch", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 10, "Branch", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 11, "Sales Agent", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 12, "Sales Agent", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 13, "Sales Agent", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 14, "Sales Agent", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 15, "Sales Agent", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 33, "Task", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 16, "Insurance Companies", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 18, "Insurance Companies", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 31, "Task", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 30, "Transactions", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 29, "Transactions", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 28, "Transactions", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 27, "Transactions", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 17, "Insurance Companies", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 26, "Transactions", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 24, "Sales", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 23, "Sales", "View", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 22, "Sales", "Edit", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 21, "Sales", "Create", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 20, "Insurance Companies", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 19, "Insurance Companies", "Delete", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") },
                    { 25, "Sales", "Search", new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("92043fc4-b79d-4adb-9078-c046ecfe4342"), new Guid("b3bd6a92-3f1d-4b85-b39c-c0dee4264a5f") });

            migrationBuilder.InsertData(
                table: "UserDetails",
                columns: new[] { "Id", "BillWithParent", "Company", "CreateTime", "CreateUserId", "DefaultAccountId", "DisplayNameAs", "EditTime", "EditUserId", "Email", "Fax", "FirstName", "ImageUrl", "IsAgent", "IsCustomer", "IsDeleted", "IsEmployee", "IsInsuranceCompany", "IsSubCustomer", "IsSupplier", "LastName", "MiddleName", "Mobile", "OpenBalance", "Other", "Phone", "Suffix", "Title", "UserDetailId", "UserId", "Website" },
                values: new object[] { 999999, null, "Systems Limited", null, null, null, "Moid", null, null, null, null, "Muhamamad", "https://pbs.twimg.com/profile_images/633202777695514625/tUVSrLDG.jpg", null, null, null, null, null, null, null, "Shams", "Moid", null, null, null, null, null, "Mr.", null, new Guid("92043fc4-b79d-4adb-9078-c046ecfe4342"), null });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountDetailTypeId", "AccountId", "AsOf", "CreateTime", "CreateUserId", "DefaultTaxCode", "Description", "EditTime", "EditUserId", "IsDeleted", "IsSubAccount", "Name", "Number", "OpeningBalanceEquity", "isDeleteApplicable" },
                values: new object[,]
                {
                    { 3, 1, null, null, null, null, null, "Asset", null, null, null, null, "Accounts Receivable(A/ R)", null, null, null },
                    { 1, 15, null, null, null, null, null, "Asset", null, null, null, null, "Cash Account", null, null, null },
                    { 2, 15, null, null, null, null, null, "Asset", null, null, null, null, "Test Bank Account", null, null, null },
                    { 4, 41, null, null, null, null, null, "Liability", null, null, null, null, "Accounts Payable(A/ P)", null, null, null },
                    { 7, 41, null, null, null, null, null, "Liablity", null, null, null, null, "VAT Payable", null, null, null },
                    { 6, 45, null, null, null, null, null, "Income", null, null, null, null, "Sales Account", null, null, null },
                    { 100, 45, null, null, null, null, null, "Refund Income Collector", null, null, null, null, "Refund Income Account", null, null, null },
                    { 8, 81, null, null, null, null, null, "Owner's Equity", null, null, null, null, "Retained Earning", null, null, null },
                    { 9, 81, null, null, null, null, null, "Owner's Equity", null, null, null, null, "Opening Balance Equity", null, null, null },
                    { 5, 105, null, null, null, null, null, "Expense", null, null, null, null, "Expense", null, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountDetailType_AccountTypeId",
                table: "AccountDetailType",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountDetailTypeId",
                table: "Accounts",
                column: "AccountDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountId",
                table: "Accounts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsMapping_AccountId",
                table: "AccountsMapping",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_UserDetailId",
                table: "Address",
                column: "UserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_UserDetailsId",
                table: "AspNetRoles",
                column: "UserDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UserDetailId",
                table: "Attachments",
                column: "UserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_BankDetails_EmploymentDetailId",
                table: "BankDetails",
                column: "EmploymentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_BDType_CategoryId",
                table: "BDType",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Benefits_BenefitAndDeductionId",
                table: "Benefits",
                column: "BenefitAndDeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_Benefits_BenefitTypeId",
                table: "Benefits",
                column: "BenefitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BenefitsAndDeduction_EmploymentDetailId",
                table: "BenefitsAndDeduction",
                column: "EmploymentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ComissionRate_UserDetailId",
                table: "ComissionRate",
                column: "UserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyInformation_ReconcilationReportId",
                table: "CompanyInformation",
                column: "ReconcilationReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Compensation_EmploymentDetailId",
                table: "Compensation",
                column: "EmploymentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_BenefitAndDeductionId",
                table: "Deduction",
                column: "BenefitAndDeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_DeductionTypeId",
                table: "Deduction",
                column: "DeductionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFiles_EmploymentDetailId",
                table: "EmployeeFiles",
                column: "EmploymentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentDetails_ManagerId",
                table: "EmploymentDetails",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentDetails_TeamId",
                table: "EmploymentDetails",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentDetails_UserDetailId",
                table: "EmploymentDetails",
                column: "UserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentStatus_EmploymentDetailId",
                table: "EmploymentStatus",
                column: "EmploymentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_AccountId",
                table: "Expense",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_BranchId",
                table: "Expense",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ExpenseCategoryId",
                table: "Expense",
                column: "ExpenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplication_EmploymentStatusId",
                table: "LeaveApplication",
                column: "EmploymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgarEntries_CreditAccountId",
                table: "LedgarEntries",
                column: "CreditAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgarEntries_DebitAccountId",
                table: "LedgarEntries",
                column: "DebitAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgarEntries_TransactionId",
                table: "LedgarEntries",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CreditAccountId",
                table: "Payment",
                column: "CreditAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_DepositAccountId",
                table: "Payment",
                column: "DepositAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_InsuranceCompanyId",
                table: "Payment",
                column: "InsuranceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentMethodId",
                table: "Payment",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_SalesAgentId",
                table: "Payment",
                column: "SalesAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAndBilling_PreferredPaymentMethodId",
                table: "PaymentAndBilling",
                column: "PreferredPaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAndBilling_TermsId",
                table: "PaymentAndBilling",
                column: "TermsId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentAndBilling_UserDetailId",
                table: "PaymentAndBilling",
                column: "UserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_BranchId",
                table: "Payroll",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ExpenseAccountId",
                table: "Payroll",
                column: "ExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reconcilation_DocumentId",
                table: "Reconcilation",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reconcilation_InsuranceCompanyId",
                table: "Reconcilation",
                column: "InsuranceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reconcilation_SalesAgentId",
                table: "Reconcilation",
                column: "SalesAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_AgentId",
                table: "Refund",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_InsuranceCompanyId",
                table: "Refund",
                column: "InsuranceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_InsuranceTypeId",
                table: "Refund",
                column: "InsuranceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_VehilcleId",
                table: "Refund",
                column: "VehilcleId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_BodyTypeId",
                table: "SalesInvoice",
                column: "BodyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_BranchId",
                table: "SalesInvoice",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_CustomerDetailId",
                table: "SalesInvoice",
                column: "CustomerDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_InsuranceCompanyId",
                table: "SalesInvoice",
                column: "InsuranceCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_InsuranceTypeId",
                table: "SalesInvoice",
                column: "InsuranceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_PaymentMethodId",
                table: "SalesInvoice",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_SalesInvoicePersonId",
                table: "SalesInvoice",
                column: "SalesInvoicePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_VehilcleId",
                table: "SalesInvoice",
                column: "VehilcleId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffOffBoarding_EmploymentStatusId",
                table: "StaffOffBoarding",
                column: "EmploymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTodo_AssignedById",
                table: "TaskTodo",
                column: "AssignedById");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTodo_AssignedToId",
                table: "TaskTodo",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTodo_PriorityId",
                table: "TaskTodo",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTodo_StatusId",
                table: "TaskTodo",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ManagerId",
                table: "Teams",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BranchId",
                table: "Transaction",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ExpenseId",
                table: "Transaction",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PaymentId",
                table: "Transaction",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_RefundId",
                table: "Transaction",
                column: "RefundId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SalesInvoiceId",
                table: "Transaction",
                column: "SalesInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_UserDetailId",
                table: "Transaction",
                column: "UserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_DefaultAccountId",
                table: "UserDetails",
                column: "DefaultAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserDetailId",
                table: "UserDetails",
                column: "UserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_UserId",
                table: "UserDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationApplication_BranchId",
                table: "VacationApplication",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationApplication_UserDetailId",
                table: "VacationApplication",
                column: "UserDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_VacationPolicy_EmploymentDetailId",
                table: "VacationPolicy",
                column: "EmploymentDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountsMapping");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Announcement");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "BankDetails");

            migrationBuilder.DropTable(
                name: "Benefits");

            migrationBuilder.DropTable(
                name: "ComissionRate");

            migrationBuilder.DropTable(
                name: "CompanyInformation");

            migrationBuilder.DropTable(
                name: "Compensation");

            migrationBuilder.DropTable(
                name: "Deduction");

            migrationBuilder.DropTable(
                name: "EmployeeFiles");

            migrationBuilder.DropTable(
                name: "LeaveApplication");

            migrationBuilder.DropTable(
                name: "LedgarEntries");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "PaymentAndBilling");

            migrationBuilder.DropTable(
                name: "Payroll");

            migrationBuilder.DropTable(
                name: "SetupClient");

            migrationBuilder.DropTable(
                name: "StaffOffBoarding");

            migrationBuilder.DropTable(
                name: "TaskTodo");

            migrationBuilder.DropTable(
                name: "UserCompanyInformation");

            migrationBuilder.DropTable(
                name: "VacationApplication");

            migrationBuilder.DropTable(
                name: "VacationPolicy");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Reconcilation");

            migrationBuilder.DropTable(
                name: "BenefitsAndDeduction");

            migrationBuilder.DropTable(
                name: "BDType");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "PreferredPaymentMethod");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DropTable(
                name: "EmploymentStatus");

            migrationBuilder.DropTable(
                name: "Priority");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Refund");

            migrationBuilder.DropTable(
                name: "SalesInvoice");

            migrationBuilder.DropTable(
                name: "EmploymentDetails");

            migrationBuilder.DropTable(
                name: "ExpenseCategory");

            migrationBuilder.DropTable(
                name: "BodyType");

            migrationBuilder.DropTable(
                name: "Branch");

            migrationBuilder.DropTable(
                name: "InsuranceType");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "UserDetails");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AccountDetailType");

            migrationBuilder.DropTable(
                name: "AccountType");
        }
    }
}
