﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecondV.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "passwordHash", "passwordSalt" },
                values: new object[] { new byte[] { 45, 53, 127, 218, 252, 118, 182, 156, 146, 112, 117, 242, 131, 0, 1, 164, 229, 232, 221, 191, 4, 246, 220, 158, 234, 31, 14, 168, 176, 195, 51, 250, 61, 159, 122, 166, 230, 33, 173, 53, 196, 6, 181, 234, 226, 155, 48, 30, 248, 83, 78, 235, 140, 240, 51, 86, 202, 187, 80, 223, 157, 0, 247, 81 }, new byte[] { 77, 103, 54, 172, 88, 155, 203, 86, 12, 76, 242, 165, 160, 182, 28, 210, 1, 191, 57, 188, 129, 47, 232, 139, 129, 19, 127, 96, 128, 252, 72, 43, 87, 29, 132, 94, 126, 109, 4, 197, 5, 161, 131, 51, 144, 200, 71, 8, 191, 63, 79, 190, 95, 99, 131, 115, 142, 117, 0, 197, 109, 208, 80, 59, 2, 148, 79, 163, 189, 57, 157, 102, 116, 0, 254, 12, 158, 111, 243, 52, 65, 34, 105, 71, 184, 86, 129, 72, 207, 172, 206, 204, 145, 150, 61, 59, 233, 209, 181, 142, 20, 119, 222, 149, 45, 84, 252, 193, 208, 109, 196, 126, 54, 126, 64, 122, 173, 176, 231, 17, 93, 99, 246, 33, 171, 136, 198, 204 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "passwordHash", "passwordSalt" },
                values: new object[] { new byte[] { 48, 119, 6, 175, 188, 22, 17, 228, 68, 186, 241, 120, 102, 116, 118, 141, 134, 111, 209, 177, 167, 70, 136, 232, 31, 153, 8, 236, 240, 178, 93, 155, 3, 99, 153, 201, 209, 100, 122, 77, 87, 145, 21, 93, 82, 248, 71, 234, 231, 60, 205, 55, 91, 178, 116, 177, 31, 158, 158, 248, 169, 69, 64, 3 }, new byte[] { 135, 143, 226, 202, 189, 242, 188, 166, 91, 7, 153, 94, 84, 11, 46, 99, 167, 117, 177, 169, 114, 32, 212, 36, 195, 150, 83, 173, 15, 92, 203, 122, 123, 192, 181, 69, 98, 36, 53, 248, 246, 70, 199, 27, 23, 24, 141, 144, 204, 126, 56, 152, 29, 200, 146, 219, 197, 33, 90, 159, 16, 144, 138, 248, 204, 251, 97, 23, 7, 52, 18, 199, 240, 152, 196, 46, 152, 19, 87, 187, 124, 52, 254, 11, 1, 148, 28, 30, 156, 123, 108, 127, 131, 237, 226, 213, 69, 94, 235, 153, 47, 19, 12, 159, 177, 253, 118, 71, 9, 28, 164, 228, 18, 232, 92, 135, 75, 243, 62, 82, 205, 244, 187, 141, 3, 179, 99, 42 } });
        }
    }
}