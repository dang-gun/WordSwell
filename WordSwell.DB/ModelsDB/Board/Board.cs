using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using WordSwell.DB.ModelsDB_partial.Board;

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
}
