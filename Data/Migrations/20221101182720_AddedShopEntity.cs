using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fleaApi.Data.Migrations
{
    public partial class AddedShopEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "Points",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Desc = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    isOpen = table.Column<bool>(type: "INTEGER", nullable: false),
                    isDisabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MarketId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shops_Markets_MarketId",
                        column: x => x.MarketId,
                        principalTable: "Markets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_ShopId",
                table: "Points",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_MarketId",
                table: "Shops",
                column: "MarketId",
                unique: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Shops_ShopId",
                table: "Points",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Shops_ShopId",
                table: "Points");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Points_ShopId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "Points");
        }
    }
}
