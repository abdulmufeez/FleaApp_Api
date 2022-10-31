using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fleaApi.Data.Migrations
{
    public partial class AddedGeoLocationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoLocation_Markets_MarketId",
                table: "GeoLocation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeoLocation",
                table: "GeoLocation");

            migrationBuilder.RenameTable(
                name: "GeoLocation",
                newName: "GeoLocations");

            migrationBuilder.RenameIndex(
                name: "IX_GeoLocation_MarketId",
                table: "GeoLocations",
                newName: "IX_GeoLocations_MarketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeoLocations",
                table: "GeoLocations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoLocations_Markets_MarketId",
                table: "GeoLocations",
                column: "MarketId",
                principalTable: "Markets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeoLocations_Markets_MarketId",
                table: "GeoLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeoLocations",
                table: "GeoLocations");

            migrationBuilder.RenameTable(
                name: "GeoLocations",
                newName: "GeoLocation");

            migrationBuilder.RenameIndex(
                name: "IX_GeoLocations_MarketId",
                table: "GeoLocation",
                newName: "IX_GeoLocation_MarketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeoLocation",
                table: "GeoLocation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GeoLocation_Markets_MarketId",
                table: "GeoLocation",
                column: "MarketId",
                principalTable: "Markets",
                principalColumn: "Id");
        }
    }
}
