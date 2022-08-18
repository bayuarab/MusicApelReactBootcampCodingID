using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecondV.Migrations
{
    public partial class kuy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Schedules_ScheduleId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Schedules_ScheduleId",
                table: "UserCourses");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Schedules_ScheduleId",
                table: "Carts",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Schedules_ScheduleId",
                table: "UserCourses",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Schedules_ScheduleId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Schedules_ScheduleId",
                table: "UserCourses");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Schedules_ScheduleId",
                table: "Carts",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Schedules_ScheduleId",
                table: "UserCourses",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "id");
        }
    }
}
