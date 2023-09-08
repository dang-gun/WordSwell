using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ModelsDB_partial.Board;

//using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ModelsDB.Board;

/// <summary>
/// 게시판의 댓글
/// </summary>
public class BoardPostReply
{
    /// <summary>
    /// 게시판의 게시물 고유 번호
    /// </summary>
    [Key]
    public long idBoardPostReply { get; set; }

    /// <summary>
    /// 종속된 게시물의 고유 번호 - 외래키
    /// </summary>
    /// <remarks>
    /// 검색 속도를 위해 연결한 FK
    /// </remarks>
    [ForeignKey("idBoardPostContents")]
    public long idBoardPostContents { get; set; }

    /// <summary>
    /// 종속된 게시물의 고유 번호
    /// </summary>
    /// <remarks>
    /// idBoardPostContents가 아니라 idBoardPost를 따라가는 이유는 
    /// 게시판은 idBoardPost를 기준으로 동작하기 때문이다.
    /// </remarks>
    public long idBoardPost { get; set; }


    /// <summary>
    /// 대상 댓글의 번호(최상위이면 0)
    /// </summary>
    public long idBoardPostReply_Target { get; set; }

    /// <summary>
    /// 게시물 상태
    /// </summary>
    public PostStateType PostState { get; set; }

    /// <summary>
    /// 작성자 고유번호
    /// </summary>
    /// <remarks>
    /// 0 = 비회원
    /// </remarks>
    public long idUser { get; set; }
    /// <summary>
    /// 비회원일때 유저 이름
    /// </summary>
    [MaxLength(32)]
    public string? UserName { get; set; } = string.Empty;
    /// <summary>
    /// 비밀번호
    /// </summary>
    /// <remarks>
    /// 비회원이 글쓴 경우 넣는다.
    /// </remarks>
    [MaxLength(32)]
    [JsonIgnore]
    public string? Password { get; set; }


    /// <summary>
    /// 게시물 내용
    /// </summary>
    public string Contents { get; set; } = string.Empty;

    /// <summary>
    /// 작성 시간
    /// </summary>
    public DateTime WriteTime { get; set; }

    /// <summary>
    /// 마지막으로 수정한 유저 번호
    /// </summary>
    /// <remarks>
    /// 0 = 비회원<br />
    /// 관리자에 의한 삭제, 수정, 블럭의 경우 다른 사람의 고유번호가 들어갈 수 있다.
    /// </remarks>
    public long? idUser_Edit { get; set; }
    /// <summary>
    /// 수정 시간
    /// </summary>
    public DateTime? EditTime { get; set; }
}
