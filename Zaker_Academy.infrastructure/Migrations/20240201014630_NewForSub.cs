using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zaker_Academy.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewForSub : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SubCategoryId",
                table: "Courses",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_SubCategories_SubCategoryId",
                table: "Courses",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_SubCategories_SubCategoryId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_SubCategoryId",
                table: "Courses");

            migrationBuilder.AlterColumn<int>(
                name: "SubCategoryId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
