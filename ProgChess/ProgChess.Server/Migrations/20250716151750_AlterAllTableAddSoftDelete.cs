using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterAllTableAddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "progchess",
                table: "UnitTests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "progchess",
                table: "UnitTests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "progchess",
                table: "Students",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "progchess",
                table: "Students",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "progchess",
                table: "ScoreTest",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "progchess",
                table: "ScoreTest",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "progchess",
                table: "Scores",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "progchess",
                table: "Scores",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "progchess",
                table: "Exercises",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "progchess",
                table: "Exercises",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78771d11-2a95-4101-b79b-b7cbfc4e4121", "AQAAAAIAAYagAAAAEAgBnWaYrov7PprrvFZSIm6xss3CXdyUyfSUEKpyFxq5+6+DfAY22LgmewjCxIP8yQ==", "78478dc4-9828-4244-a7a0-f2df903cab5e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "progchess",
                table: "UnitTests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "progchess",
                table: "UnitTests");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "progchess",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "progchess",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "progchess",
                table: "ScoreTest");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "progchess",
                table: "ScoreTest");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "progchess",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "progchess",
                table: "Exercises");

            migrationBuilder.UpdateData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "56621d27-acc3-4ba9-8c3b-6a0d090a987b", "AQAAAAIAAYagAAAAEGXdvx8azfoY6C5mSDzUjtOK3X3ONTwBgyScugPvzfGFmN6v/L2FWIx0R5p2c+vdHg==", "da89c465-bc41-4387-8bc5-d2a37cd048d6" });
        }
    }
}
