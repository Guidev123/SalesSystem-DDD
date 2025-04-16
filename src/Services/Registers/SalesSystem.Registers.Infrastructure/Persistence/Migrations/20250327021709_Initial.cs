using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesSystem.Register.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "register");

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "register",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(160)", unicode: false, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(160)", nullable: false),
                    Document = table.Column<string>(type: "VARCHAR(160)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "register",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(160)", unicode: false, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    NormalizedName = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    Discriminator = table.Column<string>(type: "varchar(160)", unicode: false, maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                schema: "register",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    ClaimType = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    ClaimValue = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "register",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(160)", unicode: false, nullable: false),
                    UserName = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    Email = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    SecurityStamp = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    PhoneNumber = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "register",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street = table.Column<string>(type: "varchar(200)", nullable: false),
                    Number = table.Column<string>(type: "varchar(50)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "varchar(250)", nullable: false),
                    Neighborhood = table.Column<string>(type: "varchar(100)", nullable: false),
                    ZipCode = table.Column<string>(type: "varchar(20)", nullable: false),
                    City = table.Column<string>(type: "varchar(100)", nullable: false),
                    State = table.Column<string>(type: "varchar(50)", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "register",
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Claim",
                schema: "register",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(160)", unicode: false, nullable: false),
                    ClaimType = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true),
                    ClaimValue = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claim_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "register",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                schema: "register",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(160)", unicode: false, nullable: false),
                    LoginProvider = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: false),
                    ProviderKey = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserLogin_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "register",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "register",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(160)", unicode: false, nullable: false),
                    RoleId = table.Column<string>(type: "varchar(160)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "register",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                schema: "register",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(160)", unicode: false, nullable: false),
                    LoginProvider = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: false),
                    Value = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserToken_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "register",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CustomerId",
                schema: "register",
                table: "Addresses",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claim_UserId",
                schema: "register",
                table: "Claim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_NormalizedName",
                schema: "register",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedEmail",
                schema: "register",
                table: "Users",
                column: "NormalizedEmail",
                unique: true,
                filter: "[NormalizedEmail] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedUserName",
                schema: "register",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "register");

            migrationBuilder.DropTable(
                name: "Claim",
                schema: "register");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "register");

            migrationBuilder.DropTable(
                name: "RoleClaim",
                schema: "register");

            migrationBuilder.DropTable(
                name: "UserLogin",
                schema: "register");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "register");

            migrationBuilder.DropTable(
                name: "UserToken",
                schema: "register");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "register");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "register");
        }
    }
}