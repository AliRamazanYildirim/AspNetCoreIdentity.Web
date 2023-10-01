using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreIdentity.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AppBenutzerSamenDaten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bild",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Geburtsdatum",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Geschlecht",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stadt",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bild",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Geburtsdatum",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Geschlecht",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Stadt",
                table: "AspNetUsers");
        }
    }
}
