using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fleaApi.Data.Migrations
{
    public partial class AddedPointEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Point_Markets_MarketId",
                table: "Point");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Point",
                table: "Point");

            migrationBuilder.RenameTable(
                name: "Point",
                newName: "Points");

            migrationBuilder.RenameIndex(
                name: "IX_Point_MarketId",
                table: "Points",
                newName: "IX_Points_MarketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Points",
                table: "Points",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Markets_MarketId",
                table: "Points",
                column: "MarketId",
                principalTable: "Markets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Markets_MarketId",
                table: "Points");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Points",
                table: "Points");

            migrationBuilder.RenameTable(
                name: "Points",
                newName: "Point");

            migrationBuilder.RenameIndex(
                name: "IX_Points_MarketId",
                table: "Point",
                newName: "IX_Point_MarketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Point",
                table: "Point",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Point_Markets_MarketId",
                table: "Point",
                column: "MarketId",
                principalTable: "Markets",
                principalColumn: "Id");
        }
    }
}
