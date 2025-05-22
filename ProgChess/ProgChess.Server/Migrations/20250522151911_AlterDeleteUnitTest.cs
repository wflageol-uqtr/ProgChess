using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterDeleteUnitTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitTests_Exercices_ExerciceId",
                schema: "progchess",
                table: "UnitTests");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "progchess",
                table: "Exercices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f28ce7a2-a6ea-4463-96d0-e7304c7994f9", "AQAAAAIAAYagAAAAELLM8zwgebS8bfBmEYcCzaNK0Qh6eA80kI5SzwDYhkfqYUjkKVrC1Q4IttqSro5X+A==", "e0bf3768-f3ff-40bd-bc31-ca07b7c312c8" });

            migrationBuilder.AddForeignKey(
                name: "FK_UnitTests_Exercices_ExerciceId",
                schema: "progchess",
                table: "UnitTests",
                column: "ExerciceId",
                principalSchema: "progchess",
                principalTable: "Exercices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitTests_Exercices_ExerciceId",
                schema: "progchess",
                table: "UnitTests");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "progchess",
                table: "Exercices",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "91b1cdd9-bc9c-4f76-afc5-ca813af58c62", "AQAAAAIAAYagAAAAECBn8HfYtTqsLIYFuRGYz/nPLtKrsBciHxUFvHW7USZhWAulOQ4Hw4IqhB9p+0zJmQ==", "7920cb4a-d4c6-414e-9587-e04390a9f653" });

            migrationBuilder.AddForeignKey(
                name: "FK_UnitTests_Exercices_ExerciceId",
                schema: "progchess",
                table: "UnitTests",
                column: "ExerciceId",
                principalSchema: "progchess",
                principalTable: "Exercices",
                principalColumn: "Id");
        }
    }
}
