using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteV3.Migrations
{
    public partial class SlugForMainComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostSlug",
                table: "PostMainComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PortfolioItemSlug",
                table: "PortfolioItemMainComments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostSlug",
                table: "PostMainComments");

            migrationBuilder.DropColumn(
                name: "PortfolioItemSlug",
                table: "PortfolioItemMainComments");
        }
    }
}
