using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableStudentExerciseAddDSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0a90435d-163c-4fe8-a10a-fd11e1a39cac");

            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6c531fe5-13f4-48a6-b1a4-8d6967963017");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "progchess",
                table: "StudentExercises",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "progchess",
                table: "StudentExercises",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "6e489a30-5579-48b2-9433-9d1235fbbee6", 0, "2e78f55a-0337-439d-8124-091d47dc24ee", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEO+lwTFmMXT7CaGyauyK9YnbFDnjZFD2244UjFbsMSCBgtqR6m/i45jYjkyihEKjBA==", null, false, null, null, "52a67f75-4d6f-43f9-bcd1-1d7b19c3b565", false, "math" },
                    { "d7bec259-dde7-499e-bf07-4c7a14b51563", 0, "a0c3333d-90e9-498a-874b-78fe3c6575d5", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAELAJNPre3hChjgWc7wPpzK5qkYsZHcbCHpYTas/xgEYCPO8bwM+v5/N7R1y8CwweCA==", null, false, null, null, "f4aa4232-7734-48d8-b6d8-a223fabd034c", false, "test" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6e489a30-5579-48b2-9433-9d1235fbbee6");

            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d7bec259-dde7-499e-bf07-4c7a14b51563");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "progchess",
                table: "StudentExercises");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "progchess",
                table: "StudentExercises");

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0a90435d-163c-4fe8-a10a-fd11e1a39cac", 0, "25d10fdc-2a74-4d9d-ab4d-b1a91bd0fe84", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAENRUYBluBVSv0SFDv1XjjHo0B7pQSphElb/i9gSDd+M/K0sketnuh+3RvRMSADclUg==", null, false, null, null, "6a542aa1-a356-475e-82c6-a3a39babc8f9", false, "math" },
                    { "6c531fe5-13f4-48a6-b1a4-8d6967963017", 0, "22e0aeaf-48b3-40e7-95ce-fb577345480e", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAELt3snpoIfCjIDNyPtg5iP6YzZw0v+IODMzCx+TfbkxsT2D05F7KhpXOogvkKDNUPQ==", null, false, null, null, "0f8c26e4-4f43-4821-82e8-d360cf268257", false, "test" }
                });
        }
    }
}
