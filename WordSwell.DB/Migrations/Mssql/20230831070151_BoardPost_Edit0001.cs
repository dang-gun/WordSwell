using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordSwell.DB.Migrations.Mssql
{
    public partial class BoardPost_Edit0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "BoardPost");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "BoardPostContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.UpdateData(
                table: "BoardPostContents",
                keyColumn: "idBoardPostContents",
                keyValue: 1L,
                column: "Password",
                value: "1111");

            migrationBuilder.UpdateData(
                table: "BoardPostContents",
                keyColumn: "idBoardPostContents",
                keyValue: 2L,
                column: "Password",
                value: "1111");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "BoardPostContents");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "BoardPost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 1L,
                columns: new[] { "Password", "WriteTime" },
                values: new object[] { "1111", new DateTime(2023, 8, 30, 19, 11, 29, 220, DateTimeKind.Local).AddTicks(5568) });

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 2L,
                columns: new[] { "Password", "WriteTime" },
                values: new object[] { "1111", new DateTime(2023, 8, 30, 19, 11, 29, 220, DateTimeKind.Local).AddTicks(5576) });
        }
    }
}
