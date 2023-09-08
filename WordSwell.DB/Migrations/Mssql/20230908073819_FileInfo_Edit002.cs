using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordSwell.DB.Migrations.Mssql
{
    public partial class FileInfo_Edit002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDb");

            migrationBuilder.CreateTable(
                name: "FileDbInfo",
                columns: table => new
                {
                    idFileInfo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idBoardPost = table.Column<long>(type: "bigint", nullable: false),
                    NameOri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LengthOri = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThumbnailName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileDbState = table.Column<int>(type: "int", nullable: false),
                    ErrorIs = table.Column<bool>(type: "bit", nullable: false),
                    idUser_Edit = table.Column<long>(type: "bigint", nullable: true),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDbInfo", x => x.idFileInfo);
                    table.ForeignKey(
                        name: "FK_FileDbInfo_BoardPostContents_idBoardPost",
                        column: x => x.idBoardPost,
                        principalTable: "BoardPostContents",
                        principalColumn: "idBoardPostContents",
                        onDelete: ReferentialAction.Cascade);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDbInfo");

            migrationBuilder.CreateTable(
                name: "FileDb",
                columns: table => new
                {
                    idFileDb = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorIs = table.Column<bool>(type: "bit", nullable: false),
                    FileDbState = table.Column<int>(type: "int", nullable: false),
                    LengthOri = table.Column<long>(type: "bigint", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameOri = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ThumbnailName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idBoardPost = table.Column<long>(type: "bigint", nullable: false),
                    idUser_Edit = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDb", x => x.idFileDb);
                });

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
    }
}
