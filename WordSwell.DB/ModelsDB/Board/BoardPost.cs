using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ModelsDB.Board;

/// <summary>
/// 게시판의 게시물
/// </summary>
public class BoardPost
{
    /// <summary>
    /// 게시판의 게시물 고유 번호
    /// </summary>
    [Key]
    public long idBoardPost { get; set; }
    

    /// <summary>
    /// 소속 게시판 고유번호
    /// </summary>
    public long idBoard { get; set; }


    /// <summary>
    /// 제목
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 유저 아이디
    /// </summary>
    /// <remarks>
    /// 0 = 비회원
    /// </remarks>
    public long idUser { get; set; }

    /// <summary>
    /// 비밀번호
    /// </summary>
    /// <remarks>
    /// 비회원이 글쓴 경우 넣는다.
    /// </remarks>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 작성 시간
    /// </summary>
    public DateTime WriteTime { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    /// <summary>
    /// 수정 시간
    /// </summary>
    public DateTime? EditTime { get; set; }
}
