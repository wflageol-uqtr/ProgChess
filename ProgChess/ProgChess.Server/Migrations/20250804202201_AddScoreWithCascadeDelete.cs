using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreWithCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "58467bb0-29d5-43e7-9573-30415db86b4d", 0, "ca1dfbe2-c7bf-4761-8262-fddc5494332c", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEAtWqS/v+OmVpJyd7vOaSTA8ziHiMluU0xsr0mNqSM7YspFU+K/H8o8tj7ZelZ0wEw==", null, false, null, null, "7477ca2f-7aa9-487e-9d4a-94cc118816f4", false, "math" },
                    { "eded4115-40f7-4f5c-b7ca-6fd9fd9c3e4b", 0, "3d8b7d95-0c24-46ac-8425-b25b1dfae5d8", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAEH5PlJV21LnaLvedPzyiEAH2woeUh8JA21z0q4LaPOR10GHFpv4o2RvQpzwwizlSRw==", null, false, null, null, "f2329174-0f45-42d5-842d-e98cf5c6c732", false, "test" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "58467bb0-29d5-43e7-9573-30415db86b4d");

            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "eded4115-40f7-4f5c-b7ca-6fd9fd9c3e4b");

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0ec58400-e16f-4ddd-9941-322e092780dc", 0, "a73e369b-4e9b-48c6-91d2-6e4a8a9b90ff", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAEGGyARshAIRmLBoIvIXAJnKMiSXUf94XhxQSyIGBATQ60l1nuRJ6KOqM6F+hQ9nyHA==", null, false, null, null, "81e0f74f-5a33-4702-963b-93213ed28744", false, "test" },
                    { "dd4954b3-114b-4b0c-978f-621af8ef5716", 0, "6b0275f4-c4b2-48ac-9119-f93b443fd3ef", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEBxvNE2GPen7Edd30oOSEcPvFi8XAhBgN/06oKHTH7pHPWd33cCd0p7HAlujbFMkSw==", null, false, null, null, "bc11650a-04fa-4ae3-b2d5-bac1632c42ac", false, "math" }
                });
        }
    }
}
