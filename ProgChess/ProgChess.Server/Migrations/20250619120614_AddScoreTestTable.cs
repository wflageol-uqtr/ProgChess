using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreTestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoreTest",
                schema: "progchess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScoreId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    Actual = table.Column<string>(type: "text", nullable: true),
                    Expected = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreTest_Scores_ScoreId",
                        column: x => x.ScoreId,
                        principalSchema: "progchess",
                        principalTable: "Scores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8a7dad2a-7e4f-4e73-9e4c-5801a26cf5c4", "AQAAAAIAAYagAAAAELk1097vL/VZivGs+NfX8frnSRFVuH1Ey6YjcZbvf2bYUPzLoh02qe5sML/c+/MYEw==", "4472c57e-802e-4ca9-a1da-7e6614663d52" });

            migrationBuilder.CreateIndex(
                name: "IX_ScoreTest_ScoreId",
                schema: "progchess",
                table: "ScoreTest",
                column: "ScoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreTest",
                schema: "progchess");

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "80e38aac-152b-4771-a30a-e6540f4e2a67", "AQAAAAIAAYagAAAAEBPYYID3eZYortj6rduuqtWmV6c4qKyqGdJSyz55+852cgGwiyUCvdlnIoQg8fCdZw==", "fd487333-3140-491e-8715-f69d07c917af" });
        }
    }
}
