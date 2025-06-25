using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentExerciseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentCodes",
                schema: "progchess",
                table: "Exercises");

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "progchess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PermanentCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentExercise",
                schema: "progchess",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    IsComplete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExercise", x => new { x.StudentId, x.ExerciseId });
                    table.ForeignKey(
                        name: "FK_StudentExercise_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalSchema: "progchess",
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentExercise_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "progchess",
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bcbcdbc1-3e45-4962-8bdc-008a9ad856fe", "AQAAAAIAAYagAAAAENcd0vqoTyKOI2yuEiTg+ba0gM5uVhIjaSfo+BM0lW0n3IQM5ihmUMmCTJ8uRcsOgg==", "72ee796e-c638-4bfd-93dd-d32d356b20db" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentExercise_ExerciseId",
                schema: "progchess",
                table: "StudentExercise",
                column: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentExercise",
                schema: "progchess");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "progchess");

            migrationBuilder.AddColumn<List<string>>(
                name: "StudentCodes",
                schema: "progchess",
                table: "Exercises",
                type: "text[]",
                nullable: false);

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8dd24956-11a2-4656-b980-0d25b6eaa5c8", "AQAAAAIAAYagAAAAEIUSpEnPNTmoIEjbsxg/x9/+tAqwOHmgVSg7aNh7nLpGOTp6B1mrBm0ZNPoXxrri7w==", "75960a5a-f3aa-4bfd-9124-7d541911dd21" });
        }
    }
}
