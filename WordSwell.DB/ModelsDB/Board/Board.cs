using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

}
