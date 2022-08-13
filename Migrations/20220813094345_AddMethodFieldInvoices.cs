using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecondV.Migrations
{
    public partial class AddMethodFieldInvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "MasterInvoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Method",
                table: "MasterInvoices");
        }
    }
}
