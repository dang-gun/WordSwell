using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordSwell.DB.Migrations.Mssql
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    idBoard = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.idBoard);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    idUser = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.idUser);
                });

            migrationBuilder.CreateTable(
                name: "BoardPost",
                columns: table => new
                {
                    idBoardPost = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idBoard = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    idUser = table.Column<long>(type: "bigint", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WriteTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardPost", x => x.idBoardPost);
                    table.ForeignKey(
                        name: "FK_BoardPost_Board_idBoard",
                        column: x => x.idBoard,
                        principalTable: "Board",
                        principalColumn: "idBoard",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BoardPostContents",
                columns: table => new
                {
                    idBoardPostContents = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idBoardPost = table.Column<long>(type: "bigint", nullable: false),
                    Contents = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardPostContents", x => x.idBoardPostContents);
                    table.ForeignKey(
                        name: "FK_BoardPostContents_BoardPost_idBoardPost",
                        column: x => x.idBoardPost,
                        principalTable: "BoardPost",
                        principalColumn: "idBoardPost",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Board",
                columns: new[] { "idBoard", "State", "Title" },
                values: new object[] { 1L, 0, "첫 게시판" });

            migrationBuilder.InsertData(
                table: "BoardPost",
                columns: new[] { "idBoardPost", "EditTime", "Password", "Title", "WriteTime", "idBoard", "idUser" },
                values: new object[] { 1L, null, "1111", "프로젝트에서 생성한 게시물", new DateTime(2023, 8, 30, 19, 11, 29, 220, DateTimeKind.Local).AddTicks(5568), 1L, 0L });

            migrationBuilder.InsertData(
                table: "BoardPost",
                columns: new[] { "idBoardPost", "EditTime", "Password", "Title", "WriteTime", "idBoard", "idUser" },
                values: new object[] { 2L, null, "1111", "프로젝트에서 생성한 게시물2", new DateTime(2023, 8, 30, 19, 11, 29, 220, DateTimeKind.Local).AddTicks(5576), 1L, 0L });

            migrationBuilder.InsertData(
                table: "BoardPostContents",
                columns: new[] { "idBoardPostContents", "Contents", "idBoardPost" },
                values: new object[] { 1L, "프로젝트에서 생성한 게시물의 내용물", 1L });

            migrationBuilder.InsertData(
                table: "BoardPostContents",
                columns: new[] { "idBoardPostContents", "Contents", "idBoardPost" },
                values: new object[] { 2L, "프로젝트에서 생성한 게시물의 내용물2", 2L });

            migrationBuilder.CreateIndex(
                name: "IX_BoardPost_idBoard",
                table: "BoardPost",
                column: "idBoard");

            migrationBuilder.CreateIndex(
                name: "IX_BoardPostContents_idBoardPost",
                table: "BoardPostContents",
                column: "idBoardPost");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardPostContents");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "BoardPost");

            migrationBuilder.DropTable(
                name: "Board");
        }
    }
}
