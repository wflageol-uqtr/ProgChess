using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterAllTabbleCreatedAtUpdatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "progchess",
                table: "UnitTests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "progchess",
                table: "UnitTests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "progchess",
                table: "Students",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "progchess",
                table: "Students",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "progchess",
                table: "ScoreTest",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "progchess",
                table: "ScoreTest",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "progchess",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "progchess",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "progchess",
                table: "Exercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "progchess",
                table: "Exercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a6901295-8724-4540-b32d-bc7d4b131565", "AQAAAAIAAYagAAAAEN53ti69eycFedki7kY8J5Ap0kULskk+Tk/FlKFwBAXTUIn83lsn2xxvuJhheo8BnA==", "c65f2db0-779a-4d38-b46e-f1cfc865439b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "progchess",
                table: "UnitTests");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "progchess",
                table: "UnitTests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "progchess",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "progchess",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "progchess",
                table: "ScoreTest");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "progchess",
                table: "ScoreTest");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "progchess",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "progchess",
                table: "Exercises");

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8a7dad2a-7e4f-4e73-9e4c-5801a26cf5c4", "AQAAAAIAAYagAAAAELk1097vL/VZivGs+NfX8frnSRFVuH1Ey6YjcZbvf2bYUPzLoh02qe5sML/c+/MYEw==", "4472c57e-802e-4ca9-a1da-7e6614663d52" });
        }
    }
}
