using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterStudentExerciseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentExercise_Exercises_ExerciseId",
                schema: "progchess",
                table: "StudentExercise");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentExercise_Students_StudentId",
                schema: "progchess",
                table: "StudentExercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentExercise",
                schema: "progchess",
                table: "StudentExercise");

            migrationBuilder.RenameTable(
                name: "StudentExercise",
                schema: "progchess",
                newName: "StudentExercises",
                newSchema: "progchess");

            migrationBuilder.RenameIndex(
                name: "IX_StudentExercise_ExerciseId",
                schema: "progchess",
                table: "StudentExercises",
                newName: "IX_StudentExercises_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentExercises",
                schema: "progchess",
                table: "StudentExercises",
                columns: new[] { "StudentId", "ExerciseId" });

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3d64cb98-c200-4f9a-be9b-6cb33199fa4d", "AQAAAAIAAYagAAAAEFUbxWokNhY6VU41xNe5rDCyAGbMck0XlDhpi7yMFgZ9yKqUyEzmp3mift5NaRcoJw==", "4760c00e-5127-4943-896e-ee21a5cd8022" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExercises_Exercises_ExerciseId",
                schema: "progchess",
                table: "StudentExercises",
                column: "ExerciseId",
                principalSchema: "progchess",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExercises_Students_StudentId",
                schema: "progchess",
                table: "StudentExercises",
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
                name: "FK_StudentExercises_Exercises_ExerciseId",
                schema: "progchess",
                table: "StudentExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentExercises_Students_StudentId",
                schema: "progchess",
                table: "StudentExercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentExercises",
                schema: "progchess",
                table: "StudentExercises");

            migrationBuilder.RenameTable(
                name: "StudentExercises",
                schema: "progchess",
                newName: "StudentExercise",
                newSchema: "progchess");

            migrationBuilder.RenameIndex(
                name: "IX_StudentExercises_ExerciseId",
                schema: "progchess",
                table: "StudentExercise",
                newName: "IX_StudentExercise_ExerciseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentExercise",
                schema: "progchess",
                table: "StudentExercise",
                columns: new[] { "StudentId", "ExerciseId" });

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bcbcdbc1-3e45-4962-8bdc-008a9ad856fe", "AQAAAAIAAYagAAAAENcd0vqoTyKOI2yuEiTg+ba0gM5uVhIjaSfo+BM0lW0n3IQM5ihmUMmCTJ8uRcsOgg==", "72ee796e-c638-4bfd-93dd-d32d356b20db" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExercise_Exercises_ExerciseId",
                schema: "progchess",
                table: "StudentExercise",
                column: "ExerciseId",
                principalSchema: "progchess",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExercise_Students_StudentId",
                schema: "progchess",
                table: "StudentExercise",
                column: "StudentId",
                principalSchema: "progchess",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
