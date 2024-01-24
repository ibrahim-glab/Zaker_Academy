using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zaker_Academy.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewForeCoursess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "EnrollmentCourses");

            migrationBuilder.AddColumn<string>(
                name: "applicationUserId",
                table: "EnrollmentCourses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentCourses_applicationUserId",
                table: "EnrollmentCourses",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentCourses_CourseId",
                table: "EnrollmentCourses",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentCourses_AspNetUsers_applicationUserId",
                table: "EnrollmentCourses",
                column: "applicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EnrollmentCourses_Courses_CourseId",
                table: "EnrollmentCourses",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentCourses_AspNetUsers_applicationUserId",
                table: "EnrollmentCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_EnrollmentCourses_Courses_CourseId",
                table: "EnrollmentCourses");

            migrationBuilder.DropIndex(
                name: "IX_EnrollmentCourses_applicationUserId",
                table: "EnrollmentCourses");

            migrationBuilder.DropIndex(
                name: "IX_EnrollmentCourses_CourseId",
                table: "EnrollmentCourses");

            migrationBuilder.DropColumn(
                name: "applicationUserId",
                table: "EnrollmentCourses");

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "EnrollmentCourses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
