using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesSystem.Register.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IdentityUserTableFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "register",
                table: "Users",
                type: "varchar(160)",
                unicode: false,
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "register",
                table: "Users");
        }
    }
}