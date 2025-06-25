using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterScoreTableAddStudentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PermanentCode",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                schema: "progchess",
                table: "Scores",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "80e38aac-152b-4771-a30a-e6540f4e2a67", "AQAAAAIAAYagAAAAEBPYYID3eZYortj6rduuqtWmV6c4qKyqGdJSyz55+852cgGwiyUCvdlnIoQg8fCdZw==", "fd487333-3140-491e-8715-f69d07c917af" });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_StudentId",
                schema: "progchess",
                table: "Scores",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Students_StudentId",
                schema: "progchess",
                table: "Scores",
                column: "StudentId",
                principalSchema: "progchess",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Students_StudentId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_StudentId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "StudentId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.AddColumn<string>(
                name: "PermanentCode",
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
                values: new object[] { "3d64cb98-c200-4f9a-be9b-6cb33199fa4d", "AQAAAAIAAYagAAAAEFUbxWokNhY6VU41xNe5rDCyAGbMck0XlDhpi7yMFgZ9yKqUyEzmp3mift5NaRcoJw==", "4760c00e-5127-4943-896e-ee21a5cd8022" });
        }
    }
}
