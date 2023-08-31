using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using ModelsDB_partial.Board;

namespace ModelsDB.Board;

/// <summary>
/// 게시판
/// </summary>
public class Board
{
    /// <summary>
    /// 게시판 고유번호
    /// </summary>
    [Key]
    public long idBoard { get; set; }

    /// <summary>
    /// 게시판 제목
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 게시판 상태
    /// </summary>
    public BoardStateType State { get; set; } = BoardStateType.None;

    /// <summary>
    /// 다른 개체에서 이 개체로 연결된 리스트
    /// </summary>
    /// <remarks>
    /// 이 개체에게 연결된 외래키
    /// </remarks>
    [ForeignKey("idBoard")]
    [JsonIgnore]
    public ICollection<BoardPost>? Posts { get; set; }
}
