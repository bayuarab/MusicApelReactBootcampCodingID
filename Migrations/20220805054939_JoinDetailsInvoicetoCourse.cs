using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecondV.Migrations
{
    public partial class JoinDetailsInvoicetoCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Course",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "CourseCategory",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "InvoiceDetails");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "InvoiceDetails",
                newName: "FKCourse");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FKCourse",
                table: "InvoiceDetails",
                newName: "Price");

            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "InvoiceDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseCategory",
                table: "InvoiceDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Schedule",
                table: "InvoiceDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
