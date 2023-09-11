using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using ModelsDB;
using ModelsDB.FileDb;

namespace ModelsDB.Board;

/// <summary>
/// 게시판의 게시물의 내용물
/// </summary>
public class BoardPostContents
{
    /// <summary>
    /// 게시판의 게시물의 내용물 고유 번호
    /// </summary>
    [Key]
    public long idBoardPostContents { get; set; }

    /// <summary>
    /// 게시판의 게시물 고유 번호 - 외래키
    /// </summary>
    [ForeignKey("idBoardPost")]
    public long idBoardPost { get; set; }
    /// <summary>
    /// 연결된 게시물
    /// </summary>
    [JsonIgnore]
    public BoardPost? BoardPost { get; set; }


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
    /// 댓글 릴스트
    /// </summary>
    [ForeignKey("idBoardPostContents")]
    [JsonIgnore]
    public ICollection<BoardPostReply>? ReplyList { get; set; }

    /// <summary>
    /// 첨부파일 리스트
    /// </summary>
    [ForeignKey("idBoardPostContents")]
    [JsonIgnore]
    public ICollection<FileDbInfo>? FileList { get; set; }

}
