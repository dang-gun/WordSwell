using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using DbAssist;
using WordSwell.DB;

using ModelsDB.Board;
using ModelsDB.User;
using ModelsDB.FileDb;

namespace ModelsContext;

/// <summary>
/// 
/// </summary>
public class ModelsDbContext : DbContext
{

#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
	/// <summary>
	/// 
	/// </summary>
	public ModelsDbContext()
	{
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="options"></param>
	public ModelsDbContext(DbContextOptions<ModelsDbContext> options)
		: base(options)
	{
	}
#pragma warning restore CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.

	/// <summary>
	/// 
	/// </summary>
	/// <param name="options"></param>
	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		switch (GlobalDb.DBType)
		{
			case UseDbType.Mssql:
				options.UseSqlServer(GlobalDb.DBString);
				break;

            case UseDbType.Postgresql:
                options.UseNpgsql(GlobalDb.DBString);
                break;

            case UseDbType.Mariadb:
                //SELECT VERSION();
                options.UseMySql(
                    GlobalDb.DBString
                    , new MySqlServerVersion(new Version(11, 1, 2)));
                break;

            case UseDbType.Memory:
			default:
				//options.UseInMemoryDatabase("TestDb");
				break;
		}
	}


    /// <summary>
    /// 게시판
    /// </summary>
    public DbSet<Board> Board { get; set; }
    /// <summary>
    /// 게시판의 게시물
    /// </summary>
    public DbSet<BoardPost> BoardPost { get; set; }
    /// <summary>
    /// 게시판의 게시물의 내용물
    /// </summary>
    public DbSet<BoardPostContents> BoardPostContents { get; set; }

    /// <summary>
    /// 유저
    /// </summary>
    public DbSet<User> User { get; set; }



    /// <summary>
    /// 파일DB 처리
    /// </summary>
    public DbSet<FileDb> FileDb { get; set; }


    /// <summary>
    /// 게시판
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

        //첫 게시판 생성
        modelBuilder.Entity<Board>().HasData(
            new Board
            {
                idBoard = 1,
                Title = "첫 게시판",
            });

		//테스트용 게시물 생성
        modelBuilder.Entity<BoardPost>().HasData(
            new BoardPost
            {
                idBoardPost = 1,
                idBoard = 1,
                Title = "프로젝트에서 생성한 게시물",
				idUser = 0,
                WriteTime = DateTime.Now,
                EditTime = null,
            });
        //테스트용 게시물 생성
        modelBuilder.Entity<BoardPost>().HasData(
            new BoardPost
            {
                idBoardPost = 2,
                idBoard = 1,
                Title = "프로젝트에서 생성한 게시물2",
                idUser = 0,
                WriteTime = DateTime.Now,
                EditTime = null,
            });

        //테스트용 게시물 생성
        modelBuilder.Entity<BoardPostContents>().HasData(
            new BoardPostContents
            {
                idBoardPostContents = 1,
                idBoardPost = 1,
                Password = "1111",
                Contents = "프로젝트에서 생성한 게시물의 내용물",
            });
        //테스트용 게시물 생성
        modelBuilder.Entity<BoardPostContents>().HasData(
            new BoardPostContents
            {
                idBoardPostContents = 2,
                idBoardPost = 2,
                Password = "1111",
                Contents = "프로젝트에서 생성한 게시물의 내용물2",
            });
    }
}


