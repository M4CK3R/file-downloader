using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Downloader.Migrations
{
    /// <inheritdoc />
    public partial class requestfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Requests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 26, 12, 27, 33, 269, DateTimeKind.Utc).AddTicks(9266),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 8, 26, 11, 33, 31, 175, DateTimeKind.Utc).AddTicks(6458));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Requests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 26, 12, 27, 33, 269, DateTimeKind.Utc).AddTicks(9098),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 8, 26, 11, 33, 31, 175, DateTimeKind.Utc).AddTicks(6274));

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "Requests",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "Requests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Requests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 26, 11, 33, 31, 175, DateTimeKind.Utc).AddTicks(6458),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 8, 26, 12, 27, 33, 269, DateTimeKind.Utc).AddTicks(9266));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Requests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 8, 26, 11, 33, 31, 175, DateTimeKind.Utc).AddTicks(6274),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 8, 26, 12, 27, 33, 269, DateTimeKind.Utc).AddTicks(9098));
        }
    }
}
