using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableImageMakeIdString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_AspNetUsers_UserId1",
                schema: "progchess",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_UserId1",
                schema: "progchess",
                table: "Images");

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
                name: "UserId1",
                schema: "progchess",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "progchess",
                table: "Images",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0ec58400-e16f-4ddd-9941-322e092780dc", 0, "a73e369b-4e9b-48c6-91d2-6e4a8a9b90ff", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAEGGyARshAIRmLBoIvIXAJnKMiSXUf94XhxQSyIGBATQ60l1nuRJ6KOqM6F+hQ9nyHA==", null, false, null, null, "81e0f74f-5a33-4702-963b-93213ed28744", false, "test" },
                    { "dd4954b3-114b-4b0c-978f-621af8ef5716", 0, "6b0275f4-c4b2-48ac-9119-f93b443fd3ef", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEBxvNE2GPen7Edd30oOSEcPvFi8XAhBgN/06oKHTH7pHPWd33cCd0p7HAlujbFMkSw==", null, false, null, null, "bc11650a-04fa-4ae3-b2d5-bac1632c42ac", false, "math" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserId",
                schema: "progchess",
                table: "Images",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AspNetUsers_UserId",
                schema: "progchess",
                table: "Images",
                column: "UserId",
                principalSchema: "progchess",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_AspNetUsers_UserId",
                schema: "progchess",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_UserId",
                schema: "progchess",
                table: "Images");

            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0ec58400-e16f-4ddd-9941-322e092780dc");

            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dd4954b3-114b-4b0c-978f-621af8ef5716");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "progchess",
                table: "Images",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                schema: "progchess",
                table: "Images",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "6e489a30-5579-48b2-9433-9d1235fbbee6", 0, "2e78f55a-0337-439d-8124-091d47dc24ee", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEO+lwTFmMXT7CaGyauyK9YnbFDnjZFD2244UjFbsMSCBgtqR6m/i45jYjkyihEKjBA==", null, false, null, null, "52a67f75-4d6f-43f9-bcd1-1d7b19c3b565", false, "math" },
                    { "d7bec259-dde7-499e-bf07-4c7a14b51563", 0, "a0c3333d-90e9-498a-874b-78fe3c6575d5", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAELAJNPre3hChjgWc7wPpzK5qkYsZHcbCHpYTas/xgEYCPO8bwM+v5/N7R1y8CwweCA==", null, false, null, null, "f4aa4232-7734-48d8-b6d8-a223fabd034c", false, "test" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserId1",
                schema: "progchess",
                table: "Images",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AspNetUsers_UserId1",
                schema: "progchess",
                table: "Images",
                column: "UserId1",
                principalSchema: "progchess",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
