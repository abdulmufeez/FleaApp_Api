using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fleaApi.Data.Migrations
{
    public partial class AddedBarrierEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarketBarrierId",
                table: "Points",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MarketBarrier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MarketId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketBarrier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketBarrier_Markets_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Markets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_MarketBarrierId",
                table: "Points",
                column: "MarketBarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketBarrier_MarketId",
                table: "MarketBarrier",
                column: "MarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_MarketBarrier_MarketBarrierId",
                table: "Points",
                column: "MarketBarrierId",
                principalTable: "MarketBarrier",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_MarketBarrier_MarketBarrierId",
                table: "Points");

            migrationBuilder.DropTable(
                name: "MarketBarrier");

            migrationBuilder.DropIndex(
                name: "IX_Points_MarketBarrierId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "MarketBarrierId",
                table: "Points");
        }
    }
}
