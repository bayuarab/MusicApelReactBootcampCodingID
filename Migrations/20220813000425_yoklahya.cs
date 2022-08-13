using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecondV.Migrations
{
    public partial class yoklahya : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ScheduleId",
                table: "Carts",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Schedules_ScheduleId",
                table: "Carts",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Schedules_ScheduleId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_ScheduleId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Carts");
        }
    }
}
