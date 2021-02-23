using Microsoft.EntityFrameworkCore.Migrations;

namespace Lastgram.Migrations
{
    public partial class AddArtistAndTrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Artist",
                table: "SpotifyTracks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Track",
                table: "SpotifyTracks",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Artist",
                table: "SpotifyTracks");

            migrationBuilder.DropColumn(
                name: "Track",
                table: "SpotifyTracks");
        }
    }
}
