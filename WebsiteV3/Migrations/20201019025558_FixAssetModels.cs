using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteV3.Migrations
{
    public partial class FixAssetModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioAssets_PortfolioItems_PortfolioItemId",
                table: "PortfolioAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_PostAssets_Posts_PostId",
                table: "PostAssets");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostAssets",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioItemId",
                table: "PortfolioAssets",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioAssets_PortfolioItems_PortfolioItemId",
                table: "PortfolioAssets",
                column: "PortfolioItemId",
                principalTable: "PortfolioItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostAssets_Posts_PostId",
                table: "PostAssets",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioAssets_PortfolioItems_PortfolioItemId",
                table: "PortfolioAssets");

            migrationBuilder.DropForeignKey(
                name: "FK_PostAssets_Posts_PostId",
                table: "PostAssets");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostAssets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioItemId",
                table: "PortfolioAssets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioAssets_PortfolioItems_PortfolioItemId",
                table: "PortfolioAssets",
                column: "PortfolioItemId",
                principalTable: "PortfolioItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PostAssets_Posts_PostId",
                table: "PostAssets",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
