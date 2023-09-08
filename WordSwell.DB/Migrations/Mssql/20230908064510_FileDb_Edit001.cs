using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordSwell.DB.Migrations.Mssql
{
    public partial class FileDb_Edit001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ErrorIs",
                table: "FileDb",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "idBoardPost",
                table: "FileDb",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 1L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 8, 15, 45, 9, 986, DateTimeKind.Local).AddTicks(7650));

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 2L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 8, 15, 45, 9, 986, DateTimeKind.Local).AddTicks(7657));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorIs",
                table: "FileDb");

            migrationBuilder.DropColumn(
                name: "idBoardPost",
                table: "FileDb");

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 1L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 7, 17, 31, 23, 388, DateTimeKind.Local).AddTicks(348));

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 2L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 7, 17, 31, 23, 388, DateTimeKind.Local).AddTicks(379));
        }
    }
}
