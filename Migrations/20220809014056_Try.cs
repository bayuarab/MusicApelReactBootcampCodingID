using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecondV.Migrations
{
    public partial class Try : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseDesc",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseImage",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "desc",
                table: "CourseCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "CourseCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseDesc",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseImage",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "desc",
                table: "CourseCategories");

            migrationBuilder.DropColumn(
                name: "image",
                table: "CourseCategories");
        }
    }
}
