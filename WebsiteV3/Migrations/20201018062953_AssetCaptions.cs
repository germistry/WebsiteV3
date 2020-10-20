using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteV3.Migrations
{
    public partial class AssetCaptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "PostAssets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "PortfolioAssets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caption",
                table: "PostAssets");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "PortfolioAssets");
        }
    }
}
