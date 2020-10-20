using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteV3.Migrations
{
    public partial class check : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAssets_Posts_PostId",
                table: "PostAssets");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostAssets",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Post",
                table: "PostAssets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PostAssets_Posts_PostId",
                table: "PostAssets",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostAssets_Posts_PostId",
                table: "PostAssets");

            migrationBuilder.DropColumn(
                name: "Post",
                table: "PostAssets");

            migrationBuilder.AlterColumn<int>(
                name: "PostId",
                table: "PostAssets",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostAssets_Posts_PostId",
                table: "PostAssets",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
