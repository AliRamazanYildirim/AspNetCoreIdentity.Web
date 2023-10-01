using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

//#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AspNetCoreIdentity.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialSamen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Bild", "ConcurrencyStamp", "Email", "EmailConfirmed", "Geburtsdatum", "Geschlecht", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Stadt", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, null, "f85b5d98-ad57-48c1-9cb2-2aed0c53f625", "eliflamrayildirim@gmail.com", false, new DateTime(2024, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)2, false, null, null, null, null, "015126267282", false, "2b46ff78-d5d1-4222-9f73-39616ecaea6d", "Frankfurt", false, "eliflamra" },
                    { "2", 0, null, "15e330e2-f268-408a-a92d-564355b79dd4", "muhammedalparslanyildirim@gmail.com", false, new DateTime(2025, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)1, false, null, null, null, null, "015126267217", false, "8c43e092-6a48-4aab-85b7-798dfaaa123a", "Frankfurt", false, "muhammedalparslan" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2");
        }
    }
}
