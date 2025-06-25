using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scores",
                schema: "progchess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PermanentCode = table.Column<string>(type: "text", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    ScoreValue = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scores_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalSchema: "progchess",
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a77e9266-9b1e-4656-801b-8ee807db11f6", "AQAAAAIAAYagAAAAEC+bHEBD0/THG7gxWjnC9yj6v0fMVEyiiL8sNR+Dc79nKbe7/egmxiFw5C46A62q7w==", "24bdf2fd-d32c-4f1c-b960-063156405a43" });

            migrationBuilder.CreateIndex(
                name: "IX_Scores_ExerciseId",
                schema: "progchess",
                table: "Scores",
                column: "ExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Scores",
                schema: "progchess");

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7e88acca-b169-414d-8540-1570448a8414", "AQAAAAIAAYagAAAAEPZQTC24uJGUIlCaAz+xG9bLK835U9b7t/dwWNA5qhYzqlESvnUE9Of3liUYkCj98Q==", "178c29fe-bfd8-45a1-bcf5-27765bc491de" });
        }
    }
}
