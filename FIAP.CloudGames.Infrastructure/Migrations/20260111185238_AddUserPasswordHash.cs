using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIAP.CloudGames.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Email", "Role", "PasswordHash" },
                values: new object[]
                {
                    new Guid("9f3c2e47-6d4b-4a91-b1f1-8e6c7a2c5d34"),
                    "admin",
                    "admin@admin.com",
                    2,
                    "GQJxJ6tF1YkTaRPTpIqswE6TnxeAYi4IXFls4AkgWgs="
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9f3c2e47-6d4b-4a91-b1f1-8e6c7a2c5d34"));

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");
        }
    }
}
