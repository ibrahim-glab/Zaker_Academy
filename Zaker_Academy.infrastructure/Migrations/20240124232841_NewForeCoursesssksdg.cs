using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zaker_Academy.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewForeCoursesssksdg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "applicationUserId",
                table: "Replies",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_applicationUserId",
                table: "Replies",
                column: "applicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_applicationUserId",
                table: "Replies",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_applicationUserId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_applicationUserId",
                table: "Replies");

            migrationBuilder.AlterColumn<string>(
                name: "applicationUserId",
                table: "Replies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
