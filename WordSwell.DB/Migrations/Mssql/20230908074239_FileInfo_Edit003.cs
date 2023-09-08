using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordSwell.DB.Migrations.Mssql
{
    public partial class FileInfo_Edit003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDbInfo_BoardPostContents_idBoardPost",
                table: "FileDbInfo");

            migrationBuilder.DropIndex(
                name: "IX_FileDbInfo_idBoardPost",
                table: "FileDbInfo");

            migrationBuilder.AddColumn<long>(
                name: "idBoardPostContents",
                table: "FileDbInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

            migrationBuilder.CreateIndex(
                name: "IX_FileDbInfo_idBoardPostContents",
                table: "FileDbInfo",
                column: "idBoardPostContents");

            migrationBuilder.AddForeignKey(
                name: "FK_FileDbInfo_BoardPostContents_idBoardPostContents",
                table: "FileDbInfo",
                column: "idBoardPostContents",
                principalTable: "BoardPostContents",
                principalColumn: "idBoardPostContents",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileDbInfo_BoardPostContents_idBoardPostContents",
                table: "FileDbInfo");

            migrationBuilder.DropIndex(
                name: "IX_FileDbInfo_idBoardPostContents",
                table: "FileDbInfo");

            migrationBuilder.DropColumn(
                name: "idBoardPostContents",
                table: "FileDbInfo");

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 1L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 8, 16, 38, 19, 446, DateTimeKind.Local).AddTicks(9786));

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 2L,
                column: "WriteTime",
                value: new DateTime(2023, 9, 8, 16, 38, 19, 446, DateTimeKind.Local).AddTicks(9792));

            migrationBuilder.CreateIndex(
                name: "IX_FileDbInfo_idBoardPost",
                table: "FileDbInfo",
                column: "idBoardPost");

            migrationBuilder.AddForeignKey(
                name: "FK_FileDbInfo_BoardPostContents_idBoardPost",
                table: "FileDbInfo",
                column: "idBoardPost",
                principalTable: "BoardPostContents",
                principalColumn: "idBoardPostContents",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
