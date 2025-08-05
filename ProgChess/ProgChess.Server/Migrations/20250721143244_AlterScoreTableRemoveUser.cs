using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterScoreTableRemoveUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_AspNetUsers_UserId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_UserId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "feeb02e1-2151-4997-bbff-e22be4a88f65", "AQAAAAIAAYagAAAAEFjs1VJu1OK2X8U+Pf8WRNuGUaaZhSwqoxSrS9dOE8NhW7IDuQ4a7HEffRKj7WU7wQ==", "0f1129de-a405-4d81-a77d-7dc0b37475ef" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "progchess",
                table: "Scores",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "63692c48-0fdf-4f97-acbb-71f872d91b2c", "AQAAAAIAAYagAAAAED+E4YcKuQqdDziltvtZ4yJ/SYendh971JQCFBNEexMyO9DG+NTJIwqzY5W/H26tkg==", "5426d35a-436b-436a-9977-ace9d4fb9ae6" });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_UserId",
                schema: "progchess",
                table: "Scores",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_AspNetUsers_UserId",
                schema: "progchess",
                table: "Scores",
                column: "UserId",
                principalSchema: "progchess",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
