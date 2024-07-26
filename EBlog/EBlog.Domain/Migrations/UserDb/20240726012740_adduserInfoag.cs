using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBlog.Domain.Migrations.UserDb
{
    public partial class adduserInfoag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyDescription",
                table: "AspNetUsers",
                newName: "MyDescriptions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MyDescriptions",
                table: "AspNetUsers",
                newName: "MyDescription");
        }
    }
}
