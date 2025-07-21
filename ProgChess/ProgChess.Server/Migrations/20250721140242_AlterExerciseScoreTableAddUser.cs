using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterExerciseScoreTableAddUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "progchess",
                table: "Scores",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "progchess",
                table: "Exercises",
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

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_UserId",
                schema: "progchess",
                table: "Exercises",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_AspNetUsers_UserId",
                schema: "progchess",
                table: "Exercises",
                column: "UserId",
                principalSchema: "progchess",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_AspNetUsers_UserId",
                schema: "progchess",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_AspNetUsers_UserId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Scores_UserId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_UserId",
                schema: "progchess",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "progchess",
                table: "Exercises");

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78771d11-2a95-4101-b79b-b7cbfc4e4121", "AQAAAAIAAYagAAAAEAgBnWaYrov7PprrvFZSIm6xss3CXdyUyfSUEKpyFxq5+6+DfAY22LgmewjCxIP8yQ==", "78478dc4-9828-4244-a7a0-f2df903cab5e" });
        }
    }
}
