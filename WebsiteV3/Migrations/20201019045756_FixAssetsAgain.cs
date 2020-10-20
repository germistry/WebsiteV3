using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteV3.Migrations
{
    public partial class FixAssetsAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioAssets_PortfolioItems_PortfolioItemId",
                table: "PortfolioAssets");

            migrationBuilder.DropColumn(
                name: "Post",
                table: "PostAssets");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioItemId",
                table: "PortfolioAssets",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioAssets_PortfolioItems_PortfolioItemId",
                table: "PortfolioAssets",
                column: "PortfolioItemId",
                principalTable: "PortfolioItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioAssets_PortfolioItems_PortfolioItemId",
                table: "PortfolioAssets");

            migrationBuilder.AddColumn<int>(
                name: "Post",
                table: "PostAssets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioItemId",
                table: "PortfolioAssets",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioAssets_PortfolioItems_PortfolioItemId",
                table: "PortfolioAssets",
                column: "PortfolioItemId",
                principalTable: "PortfolioItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
