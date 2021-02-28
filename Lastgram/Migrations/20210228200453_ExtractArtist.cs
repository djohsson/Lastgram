using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Lastgram.Migrations
{
    public partial class ExtractArtist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"SpotifyTracks\"", true);

            migrationBuilder.DropColumn(
                name: "Artist",
                table: "SpotifyTracks");

            migrationBuilder.AddColumn<int>(
                name: "ArtistId",
                table: "SpotifyTracks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpotifyTracks_ArtistId",
                table: "SpotifyTracks",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpotifyTracks_Artists_ArtistId",
                table: "SpotifyTracks",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpotifyTracks_Artists_ArtistId",
                table: "SpotifyTracks");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropIndex(
                name: "IX_SpotifyTracks_ArtistId",
                table: "SpotifyTracks");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "SpotifyTracks");

            migrationBuilder.AddColumn<string>(
                name: "Artist",
                table: "SpotifyTracks",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
