using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddUserWithHashedPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "progchess",
                table: "Users",
                columns: new[] { "Id", "Email", "Password" },
                values: new object[] { 1, "test@test.com", "test123" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
