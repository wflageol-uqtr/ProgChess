using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSecondTestUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "test");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "test", 0, "feeb02e1-2151-4997-bbff-e22be4a88f65", "mathy@gmail.com", true, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEFjs1VJu1OK2X8U+Pf8WRNuGUaaZhSwqoxSrS9dOE8NhW7IDuQ4a7HEffRKj7WU7wQ==", null, false, null, null, "0f1129de-a405-4d81-a77d-7dc0b37475ef", false, "math" });
        }
    }
}
