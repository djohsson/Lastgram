using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lastgram.Migrations
{
    public partial class AddSpotifyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "TelegramUserId");

            migrationBuilder.CreateTable(
                name: "SpotifyTracks",
                columns: table => new
                {
                    Md5 = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    ValidUntil = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyTracks", x => x.Md5);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotifyTracks");

            migrationBuilder.RenameColumn(
                name: "TelegramUserId",
                table: "Users",
                newName: "Id");
        }
    }
}
