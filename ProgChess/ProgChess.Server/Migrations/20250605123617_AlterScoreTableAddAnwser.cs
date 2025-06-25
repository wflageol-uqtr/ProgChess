using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterScoreTableAddAnwser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer",
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
                values: new object[] { "8dd24956-11a2-4656-b980-0d25b6eaa5c8", "AQAAAAIAAYagAAAAEIUSpEnPNTmoIEjbsxg/x9/+tAqwOHmgVSg7aNh7nLpGOTp6B1mrBm0ZNPoXxrri7w==", "75960a5a-f3aa-4bfd-9124-7d541911dd21" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a77e9266-9b1e-4656-801b-8ee807db11f6", "AQAAAAIAAYagAAAAEC+bHEBD0/THG7gxWjnC9yj6v0fMVEyiiL8sNR+Dc79nKbe7/egmxiFw5C46A62q7w==", "24bdf2fd-d32c-4f1c-b960-063156405a43" });
        }
    }
}
