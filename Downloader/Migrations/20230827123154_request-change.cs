using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Downloader.Migrations
{
    /// <inheritdoc />
    public partial class requestchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TempName",
                table: "Requests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TempName",
                table: "Requests",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
