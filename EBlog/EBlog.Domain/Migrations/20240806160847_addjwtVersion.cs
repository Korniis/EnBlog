using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBlog.Domain.Migrations
{
    public partial class addjwtVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "jwtVersion",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "jwtVersion",
                table: "AspNetUsers");
        }
    }
}
