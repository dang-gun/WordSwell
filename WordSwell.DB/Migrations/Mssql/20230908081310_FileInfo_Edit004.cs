using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordSwell.DB.Migrations.Mssql
{
    public partial class FileInfo_Edit004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FileDbInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 1L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 8, 17, 13, 9, 799, DateTimeKind.Local).AddTicks(192));

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 2L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 8, 17, 13, 9, 799, DateTimeKind.Local).AddTicks(198));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "FileDbInfo");

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 1L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 8, 16, 42, 39, 456, DateTimeKind.Local).AddTicks(7588));

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 2L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 8, 16, 42, 39, 456, DateTimeKind.Local).AddTicks(7617));
        }
    }
}
