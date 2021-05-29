using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Migrations
{
    public partial class GuidId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Artists",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpotifyTracks",
                table: "SpotifyTracks");

            migrationBuilder.DropColumn(
                name: "Md5",
                table: "SpotifyTracks");

            migrationBuilder.AlterColumn<int>(
                name: "TelegramUserId",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "SpotifyTracks",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "IdGuid",
                table: "Artists",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.AddColumn<Guid>(
                name: "ArtistIdGuid",
                table: "SpotifyTracks",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            migrationBuilder.Sql("UPDATE \"SpotifyTracks\" AS track SET \"ArtistIdGuid\" = artist.\"IdGuid\" FROM \"Artists\" AS artist WHERE track.\"ArtistId\" = artist.\"Id\"");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "SpotifyTracks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Artists");

            migrationBuilder.RenameColumn(
                name: "IdGuid",
                table: "Artists",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ArtistIdGuid",
                table: "SpotifyTracks",
                newName: "ArtistId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Artists",
                type: "timestamp without time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpotifyTracks",
                table: "SpotifyTracks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artists",
                table: "Artists",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TelegramUserId",
                table: "Users",
                column: "TelegramUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpotifyTracks_Track",
                table: "SpotifyTracks",
                column: "Track");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_Name",
                table: "Artists",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_SpotifyTracks_Artists_Id",
                table: "SpotifyTracks",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("UPDATE \"Artists\" SET \"CreatedAt\" = now()");
            migrationBuilder.Sql("UPDATE \"Users\" SET \"CreatedAt\" = now()");

            migrationBuilder.AddUniqueConstraint(
                name: "UQ_Artists_Name",
                table: "Artists",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TelegramUserId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpotifyTracks",
                table: "SpotifyTracks");

            migrationBuilder.DropIndex(
                name: "IX_SpotifyTracks_Track",
                table: "SpotifyTracks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SpotifyTracks");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Artists");

            migrationBuilder.AlterColumn<int>(
                name: "TelegramUserId",
                table: "Users",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ArtistId",
                table: "SpotifyTracks",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "Md5",
                table: "SpotifyTracks",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Artists",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "TelegramUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpotifyTracks",
                table: "SpotifyTracks",
                column: "Md5");
        }
    }
}
