using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "139b64bf-2d4e-4c35-8db3-bc1232374530");

            migrationBuilder.DeleteData(
                schema: "progchess",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "76573c4d-1c95-4803-9478-9a6cfbca866b");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "progchess",
                table: "StudentExercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "progchess",
                table: "StudentExercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Images",
                schema: "progchess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserId1 = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalSchema: "progchess",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "0a90435d-163c-4fe8-a10a-fd11e1a39cac", 0, "25d10fdc-2a74-4d9d-ab4d-b1a91bd0fe84", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAENRUYBluBVSv0SFDv1XjjHo0B7pQSphElb/i9gSDd+M/K0sketnuh+3RvRMSADclUg==", null, false, null, null, "6a542aa1-a356-475e-82c6-a3a39babc8f9", false, "math" },
                    { "6c531fe5-13f4-48a6-b1a4-8d6967963017", 0, "22e0aeaf-48b3-40e7-95ce-fb577345480e", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAELt3snpoIfCjIDNyPtg5iP6YzZw0v+IODMzCx+TfbkxsT2D05F7KhpXOogvkKDNUPQ==", null, false, null, null, "0f8c26e4-4f43-4821-82e8-d360cf268257", false, "test" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_UserId1",
                schema: "progchess",
                table: "Images",
                column: "UserId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images",
                schema: "progchess");

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

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "progchess",
                table: "StudentExercises");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "progchess",
                table: "StudentExercises");

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "139b64bf-2d4e-4c35-8db3-bc1232374530", 0, "c77402b3-6d8d-48c5-851a-add8e1de6b24", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEDt/M9EUsWY14fi1hWuwIT3/qlZwWtT3Ihq1MjwdB6S1OR9lmVFkF5xvdkV9gyyPQQ==", null, false, null, null, "4ed81812-8915-4c5a-a2c4-7d60653625c8", false, "math" },
                    { "76573c4d-1c95-4803-9478-9a6cfbca866b", 0, "f0213fc7-635c-407e-9667-2638cd1ba621", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAEKa+YWJcYCb9vyTe03FgJktA5PpTylRfDNl3zVE6WWa1qmMyOea37JN890b2yxUuDg==", null, false, null, null, "2c72e50b-7434-4c3b-84bb-a7938d82941f", false, "test" }
                });
        }
    }
}
