using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecondV.Migrations
{
    public partial class SecondV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Courses_CourseId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_CourseCategories_CourseCategoryId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Courses_CourseId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_MasterInvoices_MasterInvoiceId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterInvoices_Users_UserId",
                table: "MasterInvoices");

            migrationBuilder.DropIndex(
                name: "IX_MasterInvoices_UserId",
                table: "MasterInvoices");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceDetails_CourseId",
                table: "InvoiceDetails");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceDetails_MasterInvoiceId",
                table: "InvoiceDetails");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseCategoryId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Carts_CourseId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "nama");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "password",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "nama",
                table: "Users",
                newName: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_MasterInvoices_UserId",
                table: "MasterInvoices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_CourseId",
                table: "InvoiceDetails",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_MasterInvoiceId",
                table: "InvoiceDetails",
                column: "MasterInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseCategoryId",
                table: "Courses",
                column: "CourseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CourseId",
                table: "Carts",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Courses_CourseId",
                table: "Carts",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_CourseCategories_CourseCategoryId",
                table: "Courses",
                column: "CourseCategoryId",
                principalTable: "CourseCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Courses_CourseId",
                table: "InvoiceDetails",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_MasterInvoices_MasterInvoiceId",
                table: "InvoiceDetails",
                column: "MasterInvoiceId",
                principalTable: "MasterInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterInvoices_Users_UserId",
                table: "MasterInvoices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
