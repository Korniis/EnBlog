using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBlog.Domain.Migrations
{
    public partial class testhot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "hot",
                table: "T_ArticleType",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hot",
                table: "T_ArticleType");
        }
    }
}
