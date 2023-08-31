﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModelsContext;

#nullable disable

namespace WordSwell.DB.Migrations.Mssql
{
    [DbContext(typeof(ModelsDbContext_Mssql))]
    [Migration("20230831070151_BoardPost_Edit0001")]
    partial class BoardPost_Edit0001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ModelsDB.Board.Board", b =>
                {
                    b.Property<long>("idBoard")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("idBoard"), 1L, 1);

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("idBoard");

                    b.ToTable("Board");

                    b.HasData(
                        new
                        {
                            idBoard = 1L,
                            State = 0,
                            Title = "첫 게시판"
                        });
                });

            modelBuilder.Entity("ModelsDB.Board.BoardPost", b =>
                {
                    b.Property<long>("idBoardPost")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("idBoardPost"), 1L, 1);

                    b.Property<DateTime?>("EditTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("WriteTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("idBoard")
                        .HasColumnType("bigint");

                    b.Property<long>("idUser")
                        .HasColumnType("bigint");

                    b.HasKey("idBoardPost");

                    b.HasIndex("idBoard");

                    b.ToTable("BoardPost");

                    b.HasData(
                        new
                        {
                            idBoardPost = 1L,
                            Title = "프로젝트에서 생성한 게시물",
                            WriteTime = new DateTime(2023, 8, 31, 16, 1, 51, 753, DateTimeKind.Local).AddTicks(6362),
                            idBoard = 1L,
                            idUser = 0L
                        },
                        new
                        {
                            idBoardPost = 2L,
                            Title = "프로젝트에서 생성한 게시물2",
                            WriteTime = new DateTime(2023, 8, 31, 16, 1, 51, 753, DateTimeKind.Local).AddTicks(6369),
                            idBoard = 1L,
                            idUser = 0L
                        });
                });

            modelBuilder.Entity("ModelsDB.Board.BoardPostContents", b =>
                {
                    b.Property<long>("idBoardPostContents")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("idBoardPostContents"), 1L, 1);

                    b.Property<string>("Contents")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("idBoardPost")
                        .HasColumnType("bigint");

                    b.HasKey("idBoardPostContents");

                    b.HasIndex("idBoardPost");

                    b.ToTable("BoardPostContents");

                    b.HasData(
                        new
                        {
                            idBoardPostContents = 1L,
                            Contents = "프로젝트에서 생성한 게시물의 내용물",
                            Password = "1111",
                            idBoardPost = 1L
                        },
                        new
                        {
                            idBoardPostContents = 2L,
                            Contents = "프로젝트에서 생성한 게시물의 내용물2",
                            Password = "1111",
                            idBoardPost = 2L
                        });
                });

            modelBuilder.Entity("ModelsDB.User.User", b =>
                {
                    b.Property<long>("idUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("idUser"), 1L, 1);

                    b.HasKey("idUser");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ModelsDB.Board.BoardPost", b =>
                {
                    b.HasOne("ModelsDB.Board.Board", "Board")
                        .WithMany("Posts")
                        .HasForeignKey("idBoard")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("ModelsDB.Board.BoardPostContents", b =>
                {
                    b.HasOne("ModelsDB.Board.BoardPost", "BoardPost")
                        .WithMany("Contents")
                        .HasForeignKey("idBoardPost")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoardPost");
                });

            modelBuilder.Entity("ModelsDB.Board.Board", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("ModelsDB.Board.BoardPost", b =>
                {
                    b.Navigation("Contents");
                });
#pragma warning restore 612, 618
        }
    }
}
