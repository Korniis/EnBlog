using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBlog.Domain.Migrations
{
    public partial class UserId3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Article_AspNetUsers_UserId",
                table: "T_Article");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "T_Article",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Article_AspNetUsers_UserId",
                table: "T_Article",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Article_AspNetUsers_UserId",
                table: "T_Article");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "T_Article",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Article_AspNetUsers_UserId",
                table: "T_Article",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
