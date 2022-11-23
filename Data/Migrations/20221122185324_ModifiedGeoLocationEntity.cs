using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fleaApi.Data.Migrations
{
    public partial class ModifiedPointEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Neighbors",
                table: "Points",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Points",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Neighbors",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Points");
        }
    }
}
