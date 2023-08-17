using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetCoreIdentity.Web.Migrations
{
    /// <inheritdoc />
    public partial class GeschlechtAktualisieren2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Geschlecht",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Geschlecht",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);
        }
    }
}
