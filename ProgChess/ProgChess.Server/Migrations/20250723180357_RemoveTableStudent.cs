using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProChess.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTableStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Students_StudentId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentExercises_Students_StudentId",
                schema: "progchess",
                table: "StudentExercises");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "progchess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentExercises",
                schema: "progchess",
                table: "StudentExercises");

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

            migrationBuilder.RenameColumn(
                name: "StudentId",
                schema: "progchess",
                table: "StudentExercises",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                schema: "progchess",
                table: "Scores",
                newName: "StudentExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_StudentId",
                schema: "progchess",
                table: "Scores",
                newName: "IX_Scores_StudentExerciseId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "progchess",
                table: "StudentExercises",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "StudentPermanentCode",
                schema: "progchess",
                table: "StudentExercises",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentExercises",
                schema: "progchess",
                table: "StudentExercises",
                column: "Id");

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "139b64bf-2d4e-4c35-8db3-bc1232374530", 0, "c77402b3-6d8d-48c5-851a-add8e1de6b24", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEDt/M9EUsWY14fi1hWuwIT3/qlZwWtT3Ihq1MjwdB6S1OR9lmVFkF5xvdkV9gyyPQQ==", null, false, null, null, "4ed81812-8915-4c5a-a2c4-7d60653625c8", false, "math" },
                    { "76573c4d-1c95-4803-9478-9a6cfbca866b", 0, "f0213fc7-635c-407e-9667-2638cd1ba621", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAEKa+YWJcYCb9vyTe03FgJktA5PpTylRfDNl3zVE6WWa1qmMyOea37JN890b2yxUuDg==", null, false, null, null, "2c72e50b-7434-4c3b-84bb-a7938d82941f", false, "test" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentExercises_StudentPermanentCode_ExerciseId",
                schema: "progchess",
                table: "StudentExercises",
                columns: new[] { "StudentPermanentCode", "ExerciseId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_StudentExercises_StudentExerciseId",
                schema: "progchess",
                table: "Scores",
                column: "StudentExerciseId",
                principalSchema: "progchess",
                principalTable: "StudentExercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scores_StudentExercises_StudentExerciseId",
                schema: "progchess",
                table: "Scores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentExercises",
                schema: "progchess",
                table: "StudentExercises");

            migrationBuilder.DropIndex(
                name: "IX_StudentExercises_StudentPermanentCode_ExerciseId",
                schema: "progchess",
                table: "StudentExercises");

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

            migrationBuilder.DropColumn(
                name: "StudentPermanentCode",
                schema: "progchess",
                table: "StudentExercises");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "progchess",
                table: "StudentExercises",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "StudentExerciseId",
                schema: "progchess",
                table: "Scores",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Scores_StudentExerciseId",
                schema: "progchess",
                table: "Scores",
                newName: "IX_Scores_StudentId");

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                schema: "progchess",
                table: "StudentExercises",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentExercises",
                schema: "progchess",
                table: "StudentExercises",
                columns: new[] { "StudentId", "ExerciseId" });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "progchess",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    PermanentCode = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "progchess",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DeletedAt", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiry", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "45738887-c5c5-4234-92e5-0a5ce2d506ae", 0, "2f3a3fa5-b1b4-4912-b373-381901de7cf1", null, "mathy@gmail.com", true, false, false, null, "mathy@gmail.com", "math", "AQAAAAIAAYagAAAAEAHO3nI2qEuUjro0wWCedeYFNAc25gekODKTT2Ju84RzkBXQ+bj6s3C/2nfIq8vntA==", null, false, null, null, "5c7bec92-2cdd-44ed-88db-4e148957f2df", false, "math" },
                    { "aac3c315-2cf9-4607-b307-43a12ffa4f0d", 0, "4852e886-2d60-485e-bff6-fb4de3dd9a5b", null, "test@gmail.com", true, false, false, null, "test@gmail.com", "test", "AQAAAAIAAYagAAAAECL/miaFb9jMT6rwsDK81OU+bzssIdoXUbK2Af1ySav4SYnbqZFmCrdw1fPcKgeLUQ==", null, false, null, null, "f51eece6-e610-4578-a37d-32ef5f699d66", false, "test" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Students_StudentId",
                schema: "progchess",
                table: "Scores",
                column: "StudentId",
                principalSchema: "progchess",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExercises_Students_StudentId",
                schema: "progchess",
                table: "StudentExercises",
                column: "StudentId",
                principalSchema: "progchess",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
