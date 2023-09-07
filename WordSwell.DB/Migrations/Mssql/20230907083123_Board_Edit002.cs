using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordSwell.DB.Migrations.Mssql
{
    public partial class Board_Edit002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "BoardPost",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FileDb",
                columns: table => new
                {
                    idFileDb = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameOri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LengthOri = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThumbnailName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileDbState = table.Column<int>(type: "int", nullable: false),
                    idUser_Edit = table.Column<long>(type: "bigint", nullable: true),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDb", x => x.idFileDb);
                });

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 1L,
                columns: new[] { "UserName", "WriteTime" },
                values: new object[] { "", new DateTime(2023, 9, 7, 17, 31, 23, 388, DateTimeKind.Local).AddTicks(348) });

            migrationBuilder.UpdateData(
                table: "BoardPost",
                keyColumn: "idBoardPost",
                keyValue: 2L,
                columns: new[] { "UserName", "WriteTime" },
                values: new object[] { "", new DateTime(2023, 9, 7, 17, 31, 23, 388, DateTimeKind.Local).AddTicks(379) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDb");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "BoardPost");

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
    }
}
