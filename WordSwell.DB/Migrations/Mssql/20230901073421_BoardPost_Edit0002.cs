using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordSwell.DB.Migrations.Mssql
{
    public partial class BoardPost_Edit0002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostState",
                table: "BoardPost",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "idUser_Edit",
                table: "BoardPost",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 1L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 1, 16, 34, 21, 688, DateTimeKind.Local).AddTicks(1521));

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 2L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 1, 16, 34, 21, 688, DateTimeKind.Local).AddTicks(1528));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostState",
                table: "BoardPost");

            migrationBuilder.DropColumn(
                name: "idUser_Edit",
                table: "BoardPost");

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 1L,
                column: "WriteTime",
                value: new DateTime(2023, 8, 31, 16, 1, 51, 753, DateTimeKind.Local).AddTicks(6362));

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 2L,
                column: "WriteTime",
                value: new DateTime(2023, 8, 31, 16, 1, 51, 753, DateTimeKind.Local).AddTicks(6369));
        }
    }
}
