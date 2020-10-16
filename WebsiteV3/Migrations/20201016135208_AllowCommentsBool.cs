using Microsoft.EntityFrameworkCore.Migrations;

namespace WebsiteV3.Migrations
{
    public partial class AllowCommentsBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CommentsAllowed",
                table: "Posts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CommentsAllowed",
                table: "PortfolioItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentsAllowed",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CommentsAllowed",
                table: "PortfolioItems");
        }
    }
}
