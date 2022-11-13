using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fleaApi.Data.Migrations
{
    public partial class ModifiedEntitiesForUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "SubCategories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Shops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Markets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Categories",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isDisabled",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_AppUserId",
                table: "SubCategories",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_AppUserId",
                table: "Shops",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Markets_AppUserId",
                table: "Markets",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_AppUserId",
                table: "Categories",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_AppUserId",
                table: "Categories",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Markets_AspNetUsers_AppUserId",
                table: "Markets",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_AspNetUsers_AppUserId",
                table: "Shops",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_AspNetUsers_AppUserId",
                table: "SubCategories",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_AppUserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Markets_AspNetUsers_AppUserId",
                table: "Markets");

            migrationBuilder.DropForeignKey(
                name: "FK_Shops_AspNetUsers_AppUserId",
                table: "Shops");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_AspNetUsers_AppUserId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_SubCategories_AppUserId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Shops_AppUserId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Markets_AppUserId",
                table: "Markets");

            migrationBuilder.DropIndex(
                name: "IX_Categories_AppUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Markets");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "isDisabled",
                table: "AspNetUsers");
        }
    }
}
