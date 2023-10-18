using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Downloader.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TempName = table.Column<string>(type: "text", nullable: false),
                    MaxTries = table.Column<int>(type: "integer", nullable: true),
                    CurrentTry = table.Column<int>(type: "integer", nullable: false),
                    Retry = table.Column<bool>(type: "boolean", nullable: false),
                    BytesDownloaded = table.Column<long>(type: "bigint", nullable: false),
                    BytesTotal = table.Column<long>(type: "bigint", nullable: false),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2023, 8, 26, 11, 33, 31, 175, DateTimeKind.Utc).AddTicks(6274)),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2023, 8, 26, 11, 33, 31, 175, DateTimeKind.Utc).AddTicks(6458))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Id",
                table: "Requests",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
