using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterScoreTableRemoveScoreValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScoreValue",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "56621d27-acc3-4ba9-8c3b-6a0d090a987b", "AQAAAAIAAYagAAAAEGXdvx8azfoY6C5mSDzUjtOK3X3ONTwBgyScugPvzfGFmN6v/L2FWIx0R5p2c+vdHg==", "da89c465-bc41-4387-8bc5-d2a37cd048d6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScoreValue",
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
                values: new object[] { "a6901295-8724-4540-b32d-bc7d4b131565", "AQAAAAIAAYagAAAAEN53ti69eycFedki7kY8J5Ap0kULskk+Tk/FlKFwBAXTUIn83lsn2xxvuJhheo8BnA==", "c65f2db0-779a-4d38-b46e-f1cfc865439b" });
        }
    }
}
