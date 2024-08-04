using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBlog.Domain.Migrations
{
    public partial class articl2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 删除外键约束
            migrationBuilder.DropForeignKey(
                name: "FK_T_Article_T_ArticleType_TypeId",
                table: "T_Article");

            // 更改列类型
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "T_ArticleType",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "TypeId",
                table: "T_Article",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "T_Article",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            // 重新创建外键约束
            migrationBuilder.AddForeignKey(
                name: "FK_T_Article_T_ArticleType_TypeId",
                table: "T_Article",
                column: "TypeId",
                principalTable: "T_ArticleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 删除外键约束
            migrationBuilder.DropForeignKey(
                name: "FK_T_Article_T_ArticleType_TypeId",
                table: "T_Article");

            // 恢复列类型
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "T_ArticleType",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "T_Article",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "T_Article",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            // 重新创建外键约束
            migrationBuilder.AddForeignKey(
                name: "FK_T_Article_T_ArticleType_TypeId",
                table: "T_Article",
                column: "TypeId",
                principalTable: "T_ArticleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
