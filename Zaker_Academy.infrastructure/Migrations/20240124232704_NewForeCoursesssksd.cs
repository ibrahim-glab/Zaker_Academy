using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zaker_Academy.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewForeCoursesssksd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "applicationUserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_applicationUserId",
                table: "Comments",
                column: "applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_applicationUserId",
                table: "Comments",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_applicationUserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_applicationUserId",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "applicationUserId",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
