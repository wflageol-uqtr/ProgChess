using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterUserTableAddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "531a1de9-d56c-48c7-b82d-ec7f9a2c03ae");

            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d314cde8-f82c-46f5-b300-db1d54ec0c73");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "progchess",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "progchess",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "45738887-c5c5-4234-92e5-0a5ce2d506ae", 0, "2f3a3fa5-b1b4-4912-b373-381901de7cf1", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEAHO3nI2qEuUjro0wWCedeYFNAc25gekODKTT2Ju84RzkBXQ+bj6s3C/2nfIq8vntA==", null, false, null, null, "5c7bec92-2cdd-44ed-88db-4e148957f2df", false, "math" },
                    { "aac3c315-2cf9-4607-b307-43a12ffa4f0d", 0, "4852e886-2d60-485e-bff6-fb4de3dd9a5b", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAECL/miaFb9jMT6rwsDK81OU+bzssIdoXUbK2Af1ySav4SYnbqZFmCrdw1fPcKgeLUQ==", null, false, null, null, "f51eece6-e610-4578-a37d-32ef5f699d66", false, "test" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "45738887-c5c5-4234-92e5-0a5ce2d506ae");

            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aac3c315-2cf9-4607-b307-43a12ffa4f0d");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "progchess",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "progchess",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "531a1de9-d56c-48c7-b82d-ec7f9a2c03ae", 0, "426fc240-12b5-49b6-a333-18da73aa6e67", "test@gmail.com", true, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAEK0ZKCs9VO/jZusHygJkFsCzirALsYuR81wvDwDkL3c13kKA1dhafsTw6Q8l08Fwtw==", null, false, null, null, "eb9275e7-9c6e-4c39-ba75-a1c3c59487d1", false, "test" },
                    { "d314cde8-f82c-46f5-b300-db1d54ec0c73", 0, "77aef83d-c958-495b-b307-409d91bd66ce", "mathy@gmail.com", true, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEGHfzIHKlc/K7OlJn2hJHE7z9mxWAnL6Jbjh9Rd0u8M1ekJXsZvSFq6c+CgZfdIoJQ==", null, false, null, null, "fb4b6b98-2b1e-4c27-8162-8005a2bc27d8", false, "math" }
                });
        }
    }
}
